using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.API
{
    /// <summary>
    /// Provides the base for the Web APIs that expose IBase like objects via RESTful ike API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseApiController<T> : ApiController where T : IBase
    {
        /// <summary>
        /// Database context to be used
        /// </summary>
        protected DbContext _dbCtx { get; private set; }

        public BaseApiController(DbContext dbCtx, DbContext dbCtx1)
        {
            _dbCtx = dbCtx1;
        }




    }
}
