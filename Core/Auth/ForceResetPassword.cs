using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using MapHive.Server.Core.Events;

namespace MapHive.Server.Core
{
    public partial class Auth
    {

        public class ForceResetPasswordOutput
        {
            public bool Success { get; set; }

            public string FailureReason { get; set; }
        }

        /// <summary>
        /// Force resets a user password to a specified one
        /// </summary>
        /// <typeparam name="TAccount"></typeparam>
        /// <param name="userAccountService"></param>
        /// <param name="userId"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public static async Task<ForceResetPasswordOutput> ForceResetPasswordAsync<TAccount>(
            UserAccountService<TAccount> userAccountService, Guid userId, string newPass)
            where TAccount : RelationalUserAccount
        {
            var output = new ForceResetPasswordOutput();

            if (string.IsNullOrWhiteSpace(newPass))
            {
                output.FailureReason = "new_pass_null";
                return output;
            }

            try
            {
                PasswordResetRequestedEvent<TAccount> e = null;
                userAccountService.Configuration.AddEventHandler(new MembershipRebootEventHandlers.PasswordResetRequestedEventHandler<TAccount>
                    ((evt) =>
                    {
                        e = evt;
                    })
                );
                userAccountService.ResetPassword(userId);

                //got the reset token, so can now change the pass..
                output.Success = userAccountService.ChangePasswordFromResetKey(e.VerificationKey, newPass);
            }
            catch (Exception ex)
            {
                if (ex.Message == "The new password must be different from the old password.")
                {
                    output.FailureReason = "new_pass_same_as_old_pass";
                }
            }

            return output;
        }
    }
}
