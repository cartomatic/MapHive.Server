using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.DataModel.Validation;

namespace MapHive.Server.Core.DataModel
{
    public abstract partial class MapHiveUserBase
    {
        /// <summary>
        /// User's email. Email must be unique in the system and is also a username
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// MembershipReoot's equivalent of IsAccountClosed
        /// </summary>
        public bool IsAccountClosed { get; set; }

        /// <summary>
        /// MembershipReoot's equivalent of IsAccountVerified
        /// </summary>
        public bool IsAccountVerified { get; set; }
    }
}
