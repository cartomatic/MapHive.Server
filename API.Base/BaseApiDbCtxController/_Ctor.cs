using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.API
{
    /// <summary>
    /// Base api ctrlr with db ctx access
    /// </summary>
    /// <typeparam name="TDbCtx"></typeparam>
    public abstract partial class BaseApiDbCtxController<TDbCtx> : BaseApiController
        where TDbCtx : DbContext, new()
    {
        /// <summary>
        /// Database context to be used
        /// </summary>
        protected TDbCtx _dbCtx { get; private set; }

        public BaseApiDbCtxController()
            : this("MapHiveMeta")
        {
        }

        public BaseApiDbCtxController(string connectionStringName)
        {
            //pass the conn string to the constructor.
            _dbCtx = default(TDbCtx);
            if (!string.IsNullOrEmpty(connectionStringName))
            {
                try
                {
                    //FIXME - this requires dbctx to have a ctor with a string param... so this will fail if ctx is declared with a hardcoded string
                    _dbCtx = (TDbCtx)Activator.CreateInstance(typeof(TDbCtx), connectionStringName);
                }
                catch
                {
                    //ignore
                }
            }

            if (_dbCtx == null)
            {
                _dbCtx = new TDbCtx();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _dbCtx.Dispose();

            base.Dispose(disposing);
        }
    }
}
