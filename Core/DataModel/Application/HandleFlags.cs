using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.DataModel.Validation;
using MapHive.Server.Core.DAL.DbContext;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.DataModel
{
    public partial class Application
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        protected internal virtual async Task HandleFlags(DbContext dbCtx)
        {
            if (IsDefault)
            {
                var previouslyDefault = await dbCtx.Set<Application>().FirstOrDefaultAsync(a => a.IsDefault && a.Uuid != Uuid);
                if (previouslyDefault != null)
                {
                    previouslyDefault.IsDefault = false;
                    await previouslyDefault.UpdateAsync<Application>(dbCtx);
                }
            }
            if (IsHome)
            {
                var previsoulyHome = await dbCtx.Set<Application>().FirstOrDefaultAsync(a => a.IsHome && a.Uuid != Uuid);
                if (previsoulyHome != null)
                {
                    previsoulyHome.IsHome = false;
                    await previsoulyHome.UpdateAsync<Application>(dbCtx);
                }
            }
        }
    }
}
