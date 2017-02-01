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
        protected internal override async Task<T> UpdateAsync<T>(DbContext dbCtx, Guid uuid)
        {
            var app = await base.UpdateAsync<T>(dbCtx, uuid);

            await HandleFlags(dbCtx);

            return app;
        }
    }
}
