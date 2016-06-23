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
    }
}
