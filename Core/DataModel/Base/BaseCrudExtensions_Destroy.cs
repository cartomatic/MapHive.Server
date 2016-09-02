using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    public static partial class BaseObjectCrudExtensions
    {
        //Note:
        //Crud APIs exposed as extension methods, so the type of the object is actually worked out without having to specify it explicitly;
        //the actual Crud methods are protected

        /// <summary>
        /// Destroys an object; returns destroyed object or null in a case it has not been found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static async Task<T> DestroyAsync<T>(this T obj, DbContext dbCtx, Guid uuid)
            where T : Base
        {
            return await obj.DestroyAsync<T>(dbCtx, uuid);
        }

    }
}
