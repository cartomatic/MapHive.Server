using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.DAL.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static MapHive.Server.Core.DataModel.AppLocalisation;

namespace MapHive.Server.Core.DataModel
{
    public partial class LocalisationClass
    {
        /// <summary>
        /// Destroys an object; returns destroyed object or null in a case it has not been found
        /// invalidates app localisations cache, so client localisations will be regenerated on the next read
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected internal override async Task<T> DestroyAsync<T>(DbContext dbCtx, Guid uuid)
        {
            InvalidateAppLocalisationsCache(await GetLocalisationClassNameAsync(dbCtx, uuid));

            //need to destroy all the translation keys too...
            var localisedDbCtx = (ILocalised) dbCtx;
            localisedDbCtx.TranslationKeys.RemoveRange(
                localisedDbCtx.TranslationKeys.Where(tk => tk.LocalisationClassUuid == uuid));

            await dbCtx.SaveChangesAsync();

            return await base.DestroyAsync<T>(dbCtx, uuid);    
        }
    }
}
