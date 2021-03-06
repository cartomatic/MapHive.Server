﻿using System;
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
    /// Provides the base for the Web APIs that expose IBase like objects via RESTful ike API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbCtx">Context to be used for the basic CRUD ops; can always be substituted for particular method calls, as they usually have overloads that take in dbctx</typeparam>
    public abstract partial class BaseApiCrudController<T, TDbCtx> : BaseApiController
        where T : Base
        where TDbCtx : DbContext, new()
    {
        /// <summary>
        /// Database context to be used
        /// </summary>
        protected TDbCtx _dbCtx { get; private set; }

        public BaseApiCrudController()
            : this("MapHiveMeta")
        {
        }

        public BaseApiCrudController(string connectionStringName)
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
