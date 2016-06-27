using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.DataModel
{
    public partial class Lang
    {
        private static List<Lang> SupportedLangs { get; set; }

        /// <summary>
        /// Gets a list of supported languages
        /// </summary>
        /// <typeparam name="TDbCtx"></typeparam>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Lang>> GetSupportedLangs<TDbCtx>(TDbCtx dbCtx)
            where TDbCtx : DbContext, ILocalised
        {
            return (SupportedLangs ?? (SupportedLangs = await dbCtx.Langs.ToListAsync()));
        }
    }
}
