using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MapHive.Server.Core.DataModel
{
    public partial class AppLocalisation
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
            InvalidateAppLocalisationsCache(ApplicationName);
            return await base.UpdateAsync<T>(dbCtx, uuid);
        }
    }
}
