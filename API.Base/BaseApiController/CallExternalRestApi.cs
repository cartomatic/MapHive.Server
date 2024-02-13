using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiController
    {
        /// <summary>
        /// api call output - encapsulates the actual api output and the response itself for further investigation in a case it's required
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        public class ApiCallOutput<TOut>
        {
            /// <summary>
            /// deserialized api call output
            /// </summary>
            public TOut Output { get; set; }

            /// <summary>
            /// raw IRestResponse
            /// </summary>
            public IRestResponse Response { get; set; }
        }

        /// <summary>
        /// returns a resp message based on the response
        /// </summary>
        /// <param name="apiResponse"></param>
        /// <returns></returns>
        public IHttpActionResult ApiCallPassThrough(IRestResponse apiResponse)
        {
            //looks like there is a little problem with content type such as application/json; charset=utf-8
            var contentType = apiResponse.ContentType;
            if (apiResponse?.ContentType?.StartsWith("application/json", StringComparison.Ordinal) == true)
            {
                contentType = "application/json";
            }
            else if (apiResponse?.ContentType?.StartsWith("text/xml", StringComparison.Ordinal) == true)
            {
                contentType = "text/xml";
            }
            else if (apiResponse?.ContentType?.StartsWith("text/html", StringComparison.Ordinal) == true)
            {
                contentType = "text/html";
            }

            //looks like no content info was returned from the backend api
            //this may be because not content has actually been sent out...
            if (string.IsNullOrWhiteSpace(contentType))
            {
                //for such scenario pick the first content type accepted by the client.
                //otherwise client (or kestrel?) will return 406 code as the accepted return content types do not match the one actually returned
                contentType = apiResponse.Request.Parameters.FirstOrDefault(p => p.Name == "Accept")?.Value?.ToString().Split(',').FirstOrDefault();

                //if the above did not work, default to octet-stream
                if (string.IsNullOrEmpty(contentType))
                {
                    contentType = "application/octet-stream";
                }
            }

            var content = ExtractResponseContentAsString(apiResponse);

            if (apiResponse.IsSuccessful)
            {
                if (contentType == "application/json")
                {
                    //since this is a pass through, transfer over all the headers
                    foreach (var apiResponseHeader in apiResponse.Headers)
                    {
                        if (_headersToPassThrough.Contains(apiResponseHeader.Name))
                            HttpContext.Current.Response.Headers.Add(apiResponseHeader.Name,
                                apiResponseHeader.Value?.ToString());
                    }


                    return new NegotiatedContentResult<object>(
                        apiResponse.StatusCode, //note: this cast should be ok, the enum uses proper values,
                        string.IsNullOrEmpty(content)
                            ? (object) (apiResponse.RawBytes ?? new byte[0])
                            : !string.IsNullOrEmpty(content)
                                ? JsonConvert.DeserializeObject(content)
                                : null, //so nicely serialize object is returned
                        this
                    );
                }
            }

            return new NegotiatedContentResult<object>
            (
                apiResponse.StatusCode, //note: this cast should be ok, the enum uses proper values
                apiResponse.ErrorMessage,
                this
            );

        }

        private static string[] _headersToPassThrough =
        {
            "Access-Control-Expose-Headers",
            "MH-Total"
        };

        /// <summary>
        /// Calls a rest API
        /// </summary>
        /// <param name="url"></param>
        /// <param name="route"></param>
        /// <param name="method"></param>
        /// <param name="queryParams"></param>
        /// <param name="data"></param>
        /// <param name="authToken">Allows for performing authorized calls against rest apis - should be in a form of Scheme Token, for example Bearer XXX, Basic XXX, etc</param>
        /// <param name="customHeaders"></param>
        /// <returns></returns>
        public async Task<IRestResponse> RestApiCall(string url, string route,
            Method method = Method.GET,
            Dictionary<string, object> queryParams = null, object data = null, string authToken = null,
            Dictionary<string, string> customHeaders = null)
        {
            return await RestApiCall(null, url, route, method, queryParams, data, authToken, customHeaders, null,
                false, false);
        }

        /// <summary>
        /// Calls a rest API
        /// </summary>
        /// <param name="origHttpRequest">orig request to obtain some context from</param>
        /// <param name="url"></param>
        /// <param name="route"></param>
        /// <param name="method"></param>
        /// <param name="queryParams"></param>
        /// <param name="data"></param>
        /// <param name="authToken">Allows for performing authorized calls against rest apis - should be in a form of Scheme Token, for example Bearer XXX, Basic XXX, etc</param>
        /// <param name="customHeaders"></param>
        /// <param name="headersToSkip"></param>
        /// <param name="transferAuthHdr">Whether or not auth header should be automatically transferred to outgoing request; when a custom auth header is provided it will always take precedence</param>
        /// <param name="transferRequestHdrs">Whether or not should auto transfer request headers so they are sent out </param>
        /// <returns></returns>
        public async Task<IRestResponse> RestApiCall(HttpRequest origHttpRequest, string url, string route, Method method = Method.GET,
            Dictionary<string, object> queryParams = null, object data = null, string authToken = null, Dictionary<string, string> customHeaders = null, List<string> headersToSkip = null, bool transferAuthHdr = true, bool transferRequestHdrs = true)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var client = new RestClient($"{url}{(url.EndsWith("/") ? "" : "/")}{route}");
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var restRequest = new RestRequest(method);

            //assuming here only json ap input is supported.
            restRequest.AddHeader("Content-Type", "application/json");

            if (customHeaders != null)
            {
                foreach (var headerKey in customHeaders.Keys)
                {
                    restRequest.AddHeader(headerKey, customHeaders[headerKey]);
                }
            }

            //since the api call is done in scope of a maphive controller try to attach the default custom maphive headers
            TransferRequestHeaders(origHttpRequest, restRequest, customHeaders, headersToSkip, transferRequestHdrs);



            //add params if any
            if (queryParams != null && queryParams.Keys.Count > 0)
            {
                foreach (var key in queryParams.Keys)
                {
                    restRequest.AddParameter(key, queryParams[key], ParameterType.QueryString);
                }
            }

            if ((method == Method.POST || method == Method.PUT) && data != null)
            {
                //use custom serializer on output! This is important as the newtonsoft's json stuff is used for the object serialization!
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.JsonSerializer = new NewtonSoftJsonSerializer();
                restRequest.AddJsonBody(data);
            }


            //when auth token not provided try to obtain it off the request
            if (transferAuthHdr && string.IsNullOrEmpty(authToken))
            {
                authToken = ExtractAuthorizationHeader(origHttpRequest);
            }


            //add auth if need to perform an authorized call
            if (!string.IsNullOrEmpty(authToken))
            {
                restRequest.AddHeader("Authorization", authToken);
            }

#if DEBUG
            var debug = client.BuildUri(restRequest).AbsoluteUri;
#endif


            return await client.ExecuteTaskAsync(restRequest);
        }

        /// <summary>
        /// Calls a REST API, auto deserializes output
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="url"></param>
        /// <param name="route"></param>
        /// <param name="method"></param>
        /// <param name="queryParams"></param>
        /// <param name="data"></param>
        /// <param name="authToken">Allows for performing authorized calls against rest apis - should be in a form of Scheme Token, for example Bearer XXX, Basic XXX, etc</param>
        /// <param name="customHeaders"></param>
        /// <returns></returns>
        public async Task<ApiCallOutput<TOut>> RestApiCall<TOut>(string url, string route, Method method = Method.GET,
            Dictionary<string, object> queryParams = null, object data = null, string authToken = null, Dictionary<string, string> customHeaders = null)
        {
            return await RestApiCall<TOut>(null, url, route, method, queryParams, data, authToken, customHeaders,
                null, false, false);
        }

        /// <summary>
        /// Calls a REST API, auto deserializes output
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="origHttpRequest">orig request for context extraction - headers, etc.</param>
        /// <param name="url"></param>
        /// <param name="route"></param>
        /// <param name="method"></param>
        /// <param name="queryParams"></param>
        /// <param name="data"></param>
        /// <param name="authToken">Allows for performing authorized calls against rest apis - should be in a form of Scheme Token, for example Bearer XXX, Basic XXX, etc</param>
        /// <param name="customHeaders"></param>
        /// <param name="headersToSkip">header names to be skipped when transferring</param>
        /// <param name="transferAuthHdr"></param>
        /// <param name="transferRequestHdrs">Whether or not should auto transfer request headers so they are sent out </param>
        /// <returns></returns>
        public async Task<ApiCallOutput<TOut>> RestApiCall<TOut>(HttpRequest origHttpRequest, string url, string route, Method method = Method.GET,
            Dictionary<string, object> queryParams = null, object data = null, string authToken = null, Dictionary<string, string> customHeaders = null, List<string> headersToSkip = null, bool transferAuthHdr = true, bool transferRequestHdrs = true)
        {
            //because of some reason RestSharp is bitching around when deserializing the arr / list output...
            //using Newtonsoft.Json instead

            var output = default(TOut);

            var resp = await RestApiCall(origHttpRequest, url, route, method, queryParams, data, authToken, customHeaders, headersToSkip, transferAuthHdr, transferRequestHdrs);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                output = (TOut)Newtonsoft.Json.JsonConvert.DeserializeObject(ExtractResponseContentAsString(resp), typeof(TOut));
            }

            return new ApiCallOutput<TOut>
            {
                Output = output,
                Response = resp
            };
        }

        /// <summary>
        /// Extracts response content as string
        /// </summary>
        /// <param name="resp"></param>
        /// <returns></returns>
        protected string ExtractResponseContentAsString(IRestResponse resp)
        {
            var content = string.Empty;

            //only brotli so far, as this is the default mh apis outgoing compression
            //if (resp.ContentEncoding == "br")
            //{
            //    using (var ms = new MemoryStream(resp.RawBytes))
            //    using (var bs = new BrotliStream(ms, CompressionMode.Decompress))
            //    using (var sr = new StreamReader(bs))
            //    {
            //        content = sr.ReadToEnd();
            //    }
            //}
            //else
            //{
            //    content = resp.Content;
            //}

            content = resp.Content;

            return content;
        }

        // <summary>
        /// Extracts auth token off a request
        /// </summary>
        /// <returns></returns>
        protected internal string ExtractAuthorizationHeader(HttpRequest request)
        {
            if (request == null)
                return null;

            //grab the auth token used by the requestee
            var authToken = string.Empty;

            if (!string.IsNullOrWhiteSpace(request.Headers["Authorization"]))
            {
                authToken = request.Headers["Authorization"];
            }

            return authToken;
        }


        private string[] _headersToIgnoreWhenTransferring = new[]
        {
            //ignore auth; it is transferred separately
            "authorization",
            //always avoid passing host header as it means when DNS resolves url to ip and request hits the sever it will resolve back to the domain thie request has originated from - the initial api
            //it would be pretty much ok, if the endpoints used different web servers or machines, but will cause problems when deployed from a single IIS!
            "host",

            //ignore content type as it may have been compressed on input and this call will send just json serialized data
            "content-type",

            //ignore content length! After all when new content is added for POST/PUT it will be worked out appropriately during data serialization and / or compression!
            //also for scenarios, when original request that initiated another request being performed in the background and being of different type (be it whatever, GET, PUT) and different length than the initiator, passing an invalid content length will make RestSharp request take ages to complete (likely because either client or server will be waiting for data me thinks ;)
            "content-length",

            //remove encoding, as incoming may be different than outgoing (for example gzipped input)
            "encoding"
        };

        /// <summary>
        /// Transfers request headers
        /// </summary>
        /// <param name="origHttpRequest">orig request to transfer headers from</param>
        /// <param name="restRequest">restsharp request to transfer the headers onto</param>
        /// <param name="customHdrs"></param>
        /// <param name="headersToSkip"></param>
        /// <param name="transferHdrs">Whether or not should transfer the headers already in the request</param>
        public void TransferRequestHeaders(HttpRequest origHttpRequest, RestRequest restRequest, Dictionary<string, string> customHdrs, List<string> headersToSkip, bool transferHdrs)
        {
            if (origHttpRequest == null)
                return;

            foreach (NameValuePair hdr in origHttpRequest.Headers)
            {
                if (_headersToIgnoreWhenTransferring.Contains(hdr.Name))
                    continue;

                //assume that custom headers overwrite whatever are the incoming headers
                //this is done outside of this method
                if (customHdrs?.ContainsKey(hdr.Name) == true)
                    continue;

                //any explicit headers to skip?
                if (headersToSkip != null && headersToSkip.Contains(hdr.Name))
                    continue;

                //finally check if remaining headers should be transferred
                if (!transferHdrs)
                    continue;

                //avoid headers starting with ':'
                //looks like they may be added to a request by the framework when processing it
                if (hdr.Name.StartsWith(":"))
                    continue;

                restRequest.AddHeader(hdr.Name, hdr.Value);

            }
        }
    }

    /// <summary>
    /// Custom RestSharp serializer utilising NewtonSoft.JSON
    /// <para />
    /// partially based on https://raw.githubusercontent.com/restsharp/RestSharp/86b31f9adf049d7fb821de8279154f41a17b36f7/RestSharp/Serializers/JsonSerializer.cs
    /// </summary>
    public class NewtonSoftJsonSerializer : ISerializer, IDeserializer
    {
        private Formatting Formatting { get; set; }

        private JsonSerializerSettings JsonSerializerSettings { get; set; }

        /// <summary>
        /// Creates a new instance with Formatting.None and null value removal when serializing
        /// </summary>
        public NewtonSoftJsonSerializer()
            : this(Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            })
        {
        }

        /// <summary>
        /// Creates a new instance of the serializer with the supplied settings
        /// </summary>
        /// <param name="formatting"></param>
        /// <param name="settings"></param>
        public NewtonSoftJsonSerializer(Newtonsoft.Json.Formatting formatting, JsonSerializerSettings settings)
        {
            ContentType = "application/json";
            Formatting = formatting;
            JsonSerializerSettings = settings;
        }


        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string DateFormat { get; set; }
        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string RootElement { get; set; }
        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// Content type for serialized content
        /// </summary>
        public string ContentType { get; set; }


        /// <inheritdoc />
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting, JsonSerializerSettings);
        }

        /// <inheritdoc />
        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}