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
        private static string DefaultLangCode { get; set; } = "en";

        /// <summary>
        /// Default lang
        /// </summary>
        private static Lang DefaultLang { get; set; }

        /// <summary>
        /// Gets a default lang as configured in the db
        /// </summary>
        /// <typeparam name="TDbCtx"></typeparam>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public static async Task<Lang> GetDefaultLang<TDbCtx>(TDbCtx dbCtx)
            where TDbCtx : DbContext, ILocalised
        {
            return await GetDefaultLang(dbCtx as DbContext);
        }

        /// <summary>
        /// Gets a default lang
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        private static async Task<Lang> GetDefaultLang(DbContext dbCtx)
        {
            var langDbCtx = dbCtx as ILocalised;
            return DefaultLang ?? (DefaultLang = await langDbCtx?.Langs.FirstOrDefaultAsync(l => l.IsDefault)) ?? (DefaultLang = await langDbCtx?.Langs.FirstOrDefaultAsync(l => l.LangCode == DefaultLangCode));
        }

        /// <summary>
        /// if lang is marked as default, makes sure to check if there is another lang marked as default and if so remove the flag
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        private async Task ResetCurrentDefaultLang(DbContext dbCtx)
        {
            if (IsDefault)
            {
                var currentDefault = await GetDefaultLang(dbCtx);
                if (currentDefault != null && currentDefault.Uuid != Uuid)
                {
                    currentDefault.IsDefault = false;
                    await currentDefault.Update(dbCtx, currentDefault.Uuid);

                    //finally update the current lang cache
                    DefaultLang = this;
                }
            }
        }
    }
}
