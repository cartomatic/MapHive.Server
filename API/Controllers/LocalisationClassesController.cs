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
    [RoutePrefix("localisationclasses")]
    public class LocalisationClassesController : BaseApiCrudController<LocalisationClass, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public LocalisationClassesController()
            : base("MapHiveMeta")
        {
        }

        /// <summary>
        /// Gets a collection of LocalisationClasses
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<LocalisationClass>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await GetAsync(sort, filter, start, limit);
        }

        /// <summary>
        /// Gets a LocalisationClass by id
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(LocalisationClass))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await GetAsync(uuid);
        }

        /// <summary>
        /// Updates a LocalisationClass
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(LocalisationClass))]
        public async Task<IHttpActionResult> Put(LocalisationClass obj, Guid uuid)
        {
            return await PutAsync(obj, uuid);
        }

        /// <summary>
        /// Creates a LocalisationClass
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(LocalisationClass))]
        public async Task<IHttpActionResult> Post(LocalisationClass obj)
        {
            return await PostAsync(obj);
        }

        /// <summary>
        /// Delets a LocalisationClass
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(LocalisationClass))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await DeleteAsync(uuid);
        }

    }
}