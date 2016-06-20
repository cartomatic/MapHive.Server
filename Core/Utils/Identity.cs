using System;
using System.Collections.Generic;
using System.Linq;
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
            Thread.CurrentPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim(IdentityServerSub, default(Guid).ToString())
                    }
                )
            );
        }
    }
}
