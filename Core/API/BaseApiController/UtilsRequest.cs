using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Cartomatic.Utils.Web;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiController
    {
        /// <summary>
        /// Extracts a source header off the request. Source header is used by the MH env to pass a full request source including hash, because hash is never sent to the client
        /// </summary>
        /// <returns></returns>
        public string GetRequestSource()
        {
            return HttpContext.Current.Request.Headers[WebClientConfiguration.HeaderSource];
        }
    }
}
