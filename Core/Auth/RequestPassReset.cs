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
        public class PassResetRequestOutput
        {
            public string VerificationKey { get; set; }
        }

        public static async Task<PassResetRequestOutput> RequestPassResetAsync<TAccount>(
            UserAccountService<TAccount> userAccountService, string email)
            where TAccount : RelationalUserAccount
        {
         
            PasswordResetRequestedEvent<TAccount> e = null;
            userAccountService.Configuration.AddEventHandler(new MembershipRebootEventHandlers.PasswordResetRequestedEventHandler<TAccount>
                ((evt) =>
                {
                    e = evt;
                })
            );
            userAccountService.ResetPassword(email);

            return new PassResetRequestOutput
            {
                VerificationKey = e.VerificationKey
            };
        }
    }
}
