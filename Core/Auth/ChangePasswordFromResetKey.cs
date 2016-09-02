using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace MapHive.Server.Core
{
    public partial class Auth
    {
        public class ChangePasswordFromResetKeyOutput
        {
            public bool Success { get; set; }

            public string FailureReason { get; set; }
        }

        public static async Task<ChangePasswordFromResetKeyOutput> ChangePasswordFromResetKeyAsync<TAccount>(
            UserAccountService<TAccount> userAccountService, string newPass, string verificationKey)
            where TAccount : RelationalUserAccount
        {
            var output = new ChangePasswordFromResetKeyOutput();
            try
            {
                output.Success = userAccountService.ChangePasswordFromResetKey(verificationKey, newPass);
            }
            catch (Exception ex)
            {
                var stop = true;

                if (ex.Message == "The new password must be different from the old password.")
                {
                    output.FailureReason = "new_pass_same_as_old_pass";
                }
            }

            return output;
        }
    }
}
