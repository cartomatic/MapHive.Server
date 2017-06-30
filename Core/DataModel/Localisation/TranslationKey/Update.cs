using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static MapHive.Server.Core.DataModel.AppLocalisation;

namespace MapHive.Server.Core.DataModel
{
    public partial class TranslationKey
    {
        /// <summary>
        /// Updates an object; returns an updated object or null if the object does not exist
        /// invalidates app localisations cache, so client localisations will be regenerated on the next read
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected internal override async Task<T> UpdateAsync<T>(DbContext dbCtx, Guid uuid)
        {
            InvalidateAppLocalisationsCache(await GetLocalisationClassAppNameAsync(dbCtx, LocalisationClassUuid), await GetLocalisationClassClassNameAsync(dbCtx, LocalisationClassUuid));
            return await base.UpdateAsync<T>(dbCtx, uuid);
        }
    }
}
