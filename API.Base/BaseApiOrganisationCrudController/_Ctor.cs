using System;
using System.Data.Entity;
using MapHive.Server.Core.API.Filters;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.API
{
    /// <summary>
    /// Provides the base for the Web APIs that expose IBase like objects via RESTful ike API;
    /// Provides access to organisation data, so all the operations may be scoped to a specified organisation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbCtx">Context to be used for the basic CRUD ops; can always be substituted for particular method calls, as they usually have overloads that take in dbctx</typeparam>
    [OrganisationContext]
    public abstract partial class BaseApiOrganisatinCrudController<T, TDbCtx> : BaseApiCrudController<T, TDbCtx>
        where T : Base
        where TDbCtx : DbContext, new()
    {
        /// <summary>
        /// Database context to be used
        /// </summary>
        protected DbContext _dbCtx { get; private set; }

        public BaseApiOrganisatinCrudController()
            : this("MapHiveMeta")
        {
        }

        public BaseApiOrganisatinCrudController(string connectionStringName)
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
