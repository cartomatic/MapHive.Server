using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.DataModel
{
    public partial class XWindowOrigin
    {
        /// <summary>
        /// Gets a list of allowed xwindow origins
        /// </summary>
        /// <typeparam name="TDbCtx"></typeparam>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> GetAllowedXWindowOriginsAsync<TDbCtx>(TDbCtx dbCtx)
            where TDbCtx : IXWindow
        {
            //TODO - plug in user based tests! via user registered apps, and such
            return await dbCtx.XWindowOrigins.Where(xw => !xw.Custom).Select(xw => xw.Origin).ToListAsync();
        }
    }
}
