using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using MapHive.Server.Core.Utils;

namespace MapHive.Server.Core
{
    public partial class Auth
    {
        public class ChangePassOutput
        {
            public bool Success { get; set; }

            public string FailureReason { get; set; }
        }

        /// <summary>
        /// Changes user's password
        /// </summary>
        /// <typeparam name="TAccount"></typeparam>
        /// <param name="userAccountService"></param>
        /// <param name="newPass"></param>
        /// <param name="oldPass"></param>
        /// <returns></returns>
        public static async Task<ChangePassOutput> ChangePassword<TAccount>(
            UserAccountService<TAccount> userAccountService, string newPass, string oldPass)
            where TAccount : RelationalUserAccount
        {
            var output = new ChangePassOutput {Success = true};

            //need to verify the user pass first and in order to do so, need to simulate user auth
            var uuid = Identity.GetUserGuid();
            if (!uuid.HasValue)
                //this shouldn't happen really as the service should only allow authenticated access, but...
            {
                output.Success = false;
                output.FailureReason = "unknown_user";
            }
            else
            {
                try
                {
                    userAccountService.ChangePassword(uuid.Value, oldPass, newPass);
                }
                catch (Exception ex)
                {
                    output.Success = false;

                    if (ex.Message == "Invalid old password.")
                    {
                        output.FailureReason = "invalid_old_pass";
                    }
                    if (ex.Message == "The new password must be different from the old password.")
                    {
                        output.FailureReason = "new_pass_same_as_old_pass";
                    }
                }
            }

            return output;
        }
    }
}
