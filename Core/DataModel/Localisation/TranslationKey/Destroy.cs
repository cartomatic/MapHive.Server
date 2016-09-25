using System;
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
    public partial class TranslationKey
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
            //need to read self first
            var translationKey = await (dbCtx as ILocalised).TranslationKeys.FirstOrDefaultAsync(tk => tk.Uuid == uuid);
            InvalidateAppLocalisationsCache(await GetLocalisationClassNameAsync(dbCtx, translationKey.LocalisationClassUuid));
            return await base.DestroyAsync<T>(dbCtx, uuid);    
        }
    }
}
