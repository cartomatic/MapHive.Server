using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.Core.DataModel
{
    public partial class Application
    {
        protected internal override async Task<T> CreateAsync<T>(DbContext dbCtx)
        {
            var app = await base.CreateAsync<T>(dbCtx);

            await HandleFlags(dbCtx);

            return app;
        }
    }
}
