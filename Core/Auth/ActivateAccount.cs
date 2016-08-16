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
        public class AccountActivationOutput
        {
            public bool Success { get; set; }

            public bool VerificationKeyStale { get; set; }

            public bool UnknownUser { get; set; }

            public string VerificationKey { get; set; }

            public string Email { get; set; }
        }

        public static async Task<AccountActivationOutput> ActivateAccount<TAccount>(
            UserAccountService<TAccount> userAccountService, string verificationKey, string initialPassword)
            where TAccount : RelationalUserAccount
        {
            var output = new AccountActivationOutput();

            var user = userAccountService.GetByVerificationKey(verificationKey);
            if (user == null)
            {
                output.UnknownUser = true;
                return output;
            }

            //see if the verification key is not stale (expired). If so issue a reset pass command, that will fire an account created event with a new key, but without password (that should have been sent out on account create)
            if (userAccountService.IsVerificationKeyStale(user.ID))
            {
                //in a case verification key is stale (outdated pretty much) need to trigger additional pass reset
                //and send some info to a user, so ne verification key can be provided
                AccountCreatedEvent<TAccount> e = null;
                userAccountService.Configuration.AddEventHandler(new MembershipRebootEventHandlers.AccountCreatedEventHandler<TAccount>
                    ((evt) =>
                    {
                        e = evt;
                    })
                );
                userAccountService.ResetPassword(user.ID);

                //since got here the reset pass dod not fail and can add som data to output
                output.Email = user.Email;
                output.VerificationKey = e.VerificationKey;
                output.VerificationKeyStale = true;
                return output;
            }
            else
            {
                userAccountService.VerifyEmailFromKey(verificationKey, initialPassword);
                output.Success = true;
                return output;
            }
        }
    }
}
