using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;

namespace MapHive.Server.Core.Events
{
    public class MembershipRebootEventHandlers
    {
        public class AccountCreatedEventHandler<TAccount> : IEventHandler<AccountCreatedEvent<TAccount>>
        {
            private readonly Action<AccountCreatedEvent<TAccount>> _a;

            public AccountCreatedEventHandler(Action<AccountCreatedEvent<TAccount>> a)
            {
                _a = a;
            }

            public void Handle(AccountCreatedEvent<TAccount> evt)
            {
                _a?.Invoke(evt);
            }
        }

        public class PasswordResetRequestedEventHandler<TAccount> : IEventHandler<PasswordResetRequestedEvent<TAccount>>
        {
            private readonly Action<PasswordResetRequestedEvent<TAccount>> _a;

            public PasswordResetRequestedEventHandler(Action<PasswordResetRequestedEvent<TAccount>> a)
            {
                _a = a;
            }

            public void Handle(PasswordResetRequestedEvent<TAccount> evt)
            {
                _a?.Invoke(evt);
            }
        }

        /// <summary>
        /// A generic evt handler - used to preview what events are actually fired by the userservice; debugging use mainly.
        /// </summary>
        /// <typeparam name="TAccount"></typeparam>
        public class UserAccountEventHandler<TAccount> : IEventHandler<UserAccountEvent<TAccount>>
        {
            private readonly Action<UserAccountEvent<TAccount>> _a;

            public UserAccountEventHandler(Action<UserAccountEvent<TAccount>> a)
            {
                _a = a;
            }

            public void Handle(UserAccountEvent<TAccount> evt)
            {
                _a?.Invoke(evt);
            }
        }
    }
}
