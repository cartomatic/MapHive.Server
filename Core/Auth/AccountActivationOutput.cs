using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
