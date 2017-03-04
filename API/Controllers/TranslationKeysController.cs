using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MapHive.Identity.MembershipReboot;
using MapHive.Server.Core.API;
using MapHive.Server.Core.API.Serialisation;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;
using MapHive.Server.Core.DAL.Interface;
using MapHive.Server.Core.Email;


namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("translationkeys")]
    public class TranslationKeysController : BaseApiCrudController<TranslationKey, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public TranslationKeysController()
            : base("MapHiveMeta")
        {
        }

        /// <summary>
        /// Gets a collection of translation keys
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<TranslationKey>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await GetAsync(sort, filter, start, limit);
        }

        /// <summary>
        /// Gets a translation key by id
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(TranslationKey))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await GetAsync(uuid);
        }

        /// <summary>
        /// Updates a translation key
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(TranslationKey))]
        public async Task<IHttpActionResult> Put(TranslationKey obj, Guid uuid)
        {
            return await PutAsync(obj, uuid);
        }

        /// <summary>
        /// Creates a new translation key
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(TranslationKey))]
        public async Task<IHttpActionResult> Post(TranslationKey obj)
        {
            return await PostAsync(obj);
        }

        /// <summary>
        /// Deletes a translation key
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(TranslationKey))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await DeleteAsync(uuid);
        }

    }
}