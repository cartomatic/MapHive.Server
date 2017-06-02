using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MapHive.Server.Core.API.Filters
{

    /// <summary>
    /// Used to impersonate a ghost user for the controller action. 
    /// </summary>
    public class ImpersonateGhostUserAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// impersonates the ghost user for a request
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            MapHive.Server.Core.Utils.Identity.ImpersonateGhostUser();
        }
    }
}
