using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MapHive.Server.Core.API;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("emailtemplatelocalisations")]
    public class EmailTemplateLocalisationsController : BaseApiCrudController<EmailTemplateLocalisation, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public EmailTemplateLocalisationsController()
            : base("MapHiveMeta")
        {
        }

        // GET: /emailtemplatelocalisations
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<EmailTemplateLocalisation>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await base.GetAsync(sort, filter, start, limit);
        }

        // GET: /emailtemplatelocalisations/5
        [HttpGet]
        [ResponseType(typeof(EmailTemplateLocalisation))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.GetAsync(uuid);
        }

        // PUT: /emailtemplatelocalisations/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(EmailTemplateLocalisation))]
        public async Task<IHttpActionResult> Put(EmailTemplateLocalisation obj, Guid uuid)
        {
            return await base.PutAsync(obj, uuid);
        }

        // POST: /emailtemplatelocalisations
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(EmailTemplateLocalisation))]
        public async Task<IHttpActionResult> Post(EmailTemplateLocalisation obj)
        {
            return await base.PostAsync(obj);
        }

        // DELETE: /emailtemplatelocalisations/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(EmailTemplateLocalisation))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await base.DeleteAsync(uuid);
        }
    }
}
