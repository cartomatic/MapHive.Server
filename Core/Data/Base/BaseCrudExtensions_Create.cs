using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Data
{
    public static partial class BaseObjectCrudExtensions
    {
        //Note:
        //Crud APIs exposed as extension methods, so the type of the object is actually worked out without having to specify it explicitly;
        //the actual Crud methods are protected

        /// <summary>
        /// Creates an object; returns a created object or null if it was not possible to create it due to the fact a uuid is already reserved
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public static async Task<T> Create<T>(this T obj, DbContext dbCtx)
            where T : Base
        {
            return await obj.Create<T>(dbCtx);
        }
    }
}
