using System.Data.Entity;
using System.Threading.Tasks;

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
