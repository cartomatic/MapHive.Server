using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiController
    {
        /// <summary>
        /// Appends a mhapi-total header with a supplied value to the headers collection 
        /// </summary>
        /// <param name="total"></param>
        protected static void AppendTotalHeader(int total)
        {
            HttpContext.Current.Response.AppendHeader(WebClientConfiguration.HeaderTotal, $"{total}");

            //need to expose this header too, otherwise client will not have access to it!
            HttpContext.Current.Response.AppendHeader("Access-Control-Expose-Headers", WebClientConfiguration.HeaderTotal);
        }
    }
}
