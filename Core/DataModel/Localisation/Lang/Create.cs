using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.DataModel
{
    public partial class Lang
    {
        /// <summary>
        /// Creates an object; returns a created object or null if it was not possible to create it due to the fact a uuid is already reserved
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        protected internal override async  Task<T> Create<T>(DbContext dbCtx)
        {
            await ResetCurrentDefaultLang(dbCtx);
            return await base.Create<T>(dbCtx);
        }
        
    }
}
