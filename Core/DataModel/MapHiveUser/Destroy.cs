using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel
{
    public static partial class MapHiveUserCrudExtensions
    {
        /// <summary>
        /// Destroys an object; returns destroyed object or null in a case it has not been found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAccount"></typeparam>
        /// <param name="obj"></param>
        /// <param name="dbCtx"></param>
        /// <param name="userAccountService"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static async Task<T> Destroy<T, TAccount>(this T obj, DbContext dbCtx, UserAccountService<TAccount> userAccountService, Guid uuid)
            where T : MapHiveUser
            where TAccount : RelationalUserAccount
        {
            return await obj.Destroy<T, TAccount>(dbCtx, userAccountService, uuid);
        }

    }

    public abstract partial class MapHiveUser
    {
        /// <summary>
        /// Overrides the default Destroy method of Base and throws an exception. The default method cannot be used for a MapHive user object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected internal override Task<T> Destroy<T>(DbContext dbCtx, Guid uuid)
        {
            throw new InvalidOperationException(WrongCrudMethodErrorInfo);
        }

        /// <summary>
        /// Destroys an object; returns destroyed object or null in a case it has not been found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAccount"></typeparam>
        /// <param name="uuid"></param>
        /// <param name="dbCtx"></param>
        /// <param name="userAccountService"></param>
        /// <returns></returns>
        protected internal virtual async Task<T> Destroy<T, TAccount>(DbContext dbCtx, UserAccountService<TAccount> userAccountService, Guid uuid)
            where T : MapHiveUser
            where TAccount : RelationalUserAccount
        {

            //get a user
            var user = await Read<T>(dbCtx, uuid);

            //and make sure user exists and has not been 'closed' before!
            if (user == null || user.IsAccountClosed)
                throw new BadRequestException(string.Empty);

            //Note: Not destroying the account really, just flagging it as closed. 
            //flag the user account as closed
            user.IsAccountClosed = true;

            //and simply update it
            return await user.Update<T, TAccount>(dbCtx, userAccountService, uuid);
        }
    }
}
