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

        // GET: /translationkeys
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<TranslationKey>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await base.GetAsync(sort, filter, start, limit);
        }

        // GET: /translationkeys/5
        [HttpGet]
        [ResponseType(typeof(TranslationKey))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.GetAsync(uuid);
        }

        // PUT: /translationkeys/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(TranslationKey))]
        public async Task<IHttpActionResult> Put(TranslationKey obj, Guid uuid)
        {
            return await base.PutAsync(obj, uuid);
        }

        // POST: /translationkeys
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(TranslationKey))]
        public async Task<IHttpActionResult> Post(TranslationKey obj)
        {
            return await base.PostAsync(obj);
        }

        // DELETE: /translationkeys/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(TranslationKey))]
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