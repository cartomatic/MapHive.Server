using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MapHive.Server.Core.API.Serialisation
{
    /// <summary>
    /// decorate a method to output camel cased json
    /// http://stackoverflow.com/questions/14528779/use-camel-case-serialization-only-for-specific-actions
    /// </summary>
    public class CamelCasedOutputMethodAttribute : ActionFilterAttribute
    {
        protected static readonly JsonMediaTypeFormatter CamelCasingFormatter = new JsonMediaTypeFormatter();

        static CamelCasedOutputMethodAttribute()
        {
            //_camelCasingFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            CamelCasingFormatter.SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var content = actionExecutedContext.Response.Content as ObjectContent;
            if (content?.Formatter is JsonMediaTypeFormatter)
            {
                actionExecutedContext.Response.Content = new ObjectContent(content.ObjectType, content.Value, CamelCasingFormatter);
            }
        }
    }
}
