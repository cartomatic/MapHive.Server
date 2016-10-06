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

        // GET: /localisationclasses
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<LocalisationClass>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await base.GetAsync(sort, filter, start, limit);
        }

        // GET: /localisationclasses/5
        [HttpGet]
        [ResponseType(typeof(LocalisationClass))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.GetAsync(uuid);
        }

        // PUT: /localisationclasses/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(LocalisationClass))]
        public async Task<IHttpActionResult> Put(LocalisationClass obj, Guid uuid)
        {
            return await base.PutAsync(obj, uuid);
        }

        // POST: /localisationclasses
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(LocalisationClass))]
        public async Task<IHttpActionResult> Post(LocalisationClass obj)
        {
            return await base.PostAsync(obj);
        }

        // DELETE: /localisationclasses/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(LocalisationClass))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            try
            {
                return await base.DeleteAsync(uuid);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}