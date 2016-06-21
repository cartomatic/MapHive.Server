using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiController<T, TDbCtx>
    {
        /// <summary>
        /// Defualt post action
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="db">DbContext to be used; when not provided a default instance of TDbCtx will be used</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Post(T obj, DbContext db = null)
        {
            return await Create(db ?? _dbCtx, obj);
        }

        /// <summary>
        /// Defualt post action with automated conversion from DTO
        /// </summary>
        /// <typeparam name="DTO">DTO type to convert from to the core type</typeparam>
        /// <param name="obj"></param>
        /// <param name="db">DbContext to be used; when not provided a default instance of TDbCtx will be used</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Post<DTO>(DTO obj, DbContext db = null) where DTO : class
        {
            return await Create(db ?? _dbCtx, obj);
        }

        /// <summary>
        /// Creates an object
        /// </summary>
        /// <param name="db">DbContext to be used; when not provided a default instance of TDbCtx will be used</param>
        /// <param name="obj"></param>
        /// <typeparam name="TDto">Type to transfer the data from when creating an instance of T; must implement IDto of TDto</typeparam>
        /// <returns></returns>
        protected virtual async Task<IHttpActionResult> Create<TDto>(DbContext db, TDto obj) where TDto : class
        {
            try
            {
                T coreObj;

                //Note: this could and should be done in a more elegant way. but had no smart ideas at a time. will come back to this at some stage...
                if (typeof(T) != typeof(TDto))
                {
                    //Note: IDto is on the TDto and is implemented on instance obviously. so need one
                    var inst = CrateIDtoInstance<TDto>();

                    coreObj = inst.FromDto<T>(obj);
                }
                else
                {
                    coreObj = obj as T;
                }

                var entity = await coreObj.Create(db);

                if (entity != null)
                    return Ok(entity);

                //uups, the object has not been created as an object with the same uuid seems to exist...
                return Conflict();
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }
    }
}
