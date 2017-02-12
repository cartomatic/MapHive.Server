using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Utils
{
    public static class Identity
    {
        private static string IdentityServerSub { get; set; } = "sub";

        /// <summary>
        /// Gets a user uuid off the thread's CurrentPrincipal
        /// </summary>
        /// <returns></returns>
        public static Guid? GetUserGuid()
        {
            Guid? guid = null;

            try
            {
                var cp = ClaimsPrincipal.Current;
                //same as 
                //Thread.CurrentPrincipal as ClaimsPrincipal;
                //but will throw if the concrete IPrincipal is not ClaimsPrincipal

                //grab the sub claim
                var subjectClaim = cp.FindFirst(IdentityServerSub);

                //if no subject claim is present try to obtain it off the logical ctx
                //see the comments in ImpersonateGhostUser
                if (subjectClaim == null)
                {
                    cp = (ClaimsPrincipal) CallContext.GetData("CurrentPrincipal");
                    subjectClaim = cp.FindFirst(IdentityServerSub);
                }

                if (subjectClaim != null)
                {
                    guid = Guid.Parse(subjectClaim.Value);
                }
            }
            catch
            {
                //ignore
                //it is the cast that can potentially fail
            }


            return guid;
        }

        /// <summary>
        /// Sets a new Claims Principal on the thread's CurrentPrincipal with an IdentityServer sub claim containing a default guid value - aka 'ghost' uuid
        /// </summary>
        public static void ImpersonateGhostUser()
        {
            var cp = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim(IdentityServerSub, default(Guid).ToString())
                    }
                )
            );

            //set cp on the thread
            Thread.CurrentPrincipal = cp;

            //and in the call ctx

            //Note:
            //basically starting with .NET 4.5 any changes done to the logical call context within an async method are discarded
            //as soon as the method completes.

            //"Stephen Cleary:
            //In.NET 4.5., async methods interact with the logical call context so that it will more properly flow with async methods.
            //...
            //In.NET 4.5, at the beginning of every async method, it activates a "copy-on-write" behavior for its logical call context.
            //When(if) the logical call context is modified, it will create a local copy of itself first.

            //Some more info:
            //http://stackoverflow.com/questions/16653308/why-is-an-await-task-yield-required-for-thread-currentprincipal-to-flow-corr/16654386#16654386
            //http://blog.stephencleary.com/2013/04/implicit-async-context-asynclocal.html
            //http://www.hanselman.com/blog/SystemThreadingThreadCurrentPrincipalVsSystemWebHttpContextCurrentUserOrWhyFormsAuthenticationCanBeSubtle.aspx

            CallContext.LogicalSetData("CurrentPrincipal", cp);
        }
    }
}
