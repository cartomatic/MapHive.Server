using System.Web.Http;
using System.Web.Http.Results;

namespace MapHive.Server.Core.API.Utils
{
    public static class IHttpActionResultExtensions
    {
        /// <summary>
        /// Extracts an object from an API call returning OkNegotiatedContentResult
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static TModel GetObjectFromOkNegotiatedContent<TModel>(this IHttpActionResult result)
            where TModel : class
        {
            return (result as OkNegotiatedContentResult<TModel>)?.Content;
        }
    }
}
