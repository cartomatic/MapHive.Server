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
        /// Creates an object; returns a created object or null if it was not possible to create it due to the fact a uuid is already reserved
        /// invalidates app localisations cache, so client localisations will be regenerated on the next read
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        protected internal override async Task<T> CreateAsync<T>(DbContext dbCtx)
        {
            var obj = await base.CreateAsync<T>(dbCtx);
            InvalidateAppLocalisationsCache(await GetLocalisationClassNameAsync(dbCtx, LocalisationClassUuid));
            return obj;
        }
    }
}
