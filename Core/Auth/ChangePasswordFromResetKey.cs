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
        public static async Task<bool> ChangePasswordFromResetKey<TAccount>(
            UserAccountService<TAccount> userAccountService, string newPass, string verificationKey)
            where TAccount : RelationalUserAccount
        {
            return userAccountService.ChangePasswordFromResetKey(verificationKey, newPass);
        }
    }
}
