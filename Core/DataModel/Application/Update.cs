using System;
using System.Data.Entity;
using System.Threading.Tasks;

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
