using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel
{
    /// <summary>
    /// Basic implementation of MapHiveUser
    /// </summary>
    public abstract partial class MapHiveUserBase : Base, IMapHiveUser
    {
        public MapHiveUserBase() : base(Guid.Parse("c34273e1-6f57-43fb-8460-44eb7bac0315"))
        {
        }

        private const string WrongCrudMethodErrorInfo =
            "User CRUD ops require MembershipReboot UserAccountService. Won't do without! Sorry... ";

    }
}
