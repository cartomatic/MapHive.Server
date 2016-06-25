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
        /// <summary>
        /// lang code of the default lang
        /// </summary>
        protected static string DefaultLangCode { get; set; } = "en";

        /// <summary>
        /// Default lang
        /// </summary>
        protected static Lang DefaultLang { get; set; }

        /// <summary>
        /// Gets a default lang as configured in the db
        /// </summary>
        /// <typeparam name="TDbCtx"></typeparam>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public static async Task<Lang> GetDefaultLang<TDbCtx>(TDbCtx dbCtx)
            where TDbCtx : DbContext, ILocalised
        {
            return DefaultLang ?? (DefaultLang = await dbCtx.Langs.FirstOrDefaultAsync(l => l.LangCode == DefaultLangCode)) ?? (DefaultLang = await dbCtx.Langs.FirstOrDefaultAsync(l => l.LangCode == DefaultLangCode));
        }
    }
}
