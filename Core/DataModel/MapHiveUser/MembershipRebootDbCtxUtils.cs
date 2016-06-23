using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel
{
    public abstract partial class MapHiveUser
    {
        /// <summary>
        /// Extracts a db context off the MembershipReboot's UserAccountService;
        /// uses reflection to grab a private db property of the Query property. May get nasty if mbr stuff changes internally. Oh well...
        /// </summary>
        /// <param name="userAccountService"></param>
        /// <returns></returns>
        protected DbContext GetMembershipRebootDbCtx<TAccount>(UserAccountService<TAccount> userAccountService)
            where TAccount : RelationalUserAccount
        {
            return userAccountService.Query.GetType()
                    .GetField("db", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(userAccountService.Query)
                    as DbContext;
        }
    }
}
