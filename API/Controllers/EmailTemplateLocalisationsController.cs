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

        /// <summary>
        /// Gets a collection of EmailTemplates
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<EmailTemplateLocalisation>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await GetAsync(sort, filter, start, limit);
        }

        /// <summary>
        /// Gets EmailTemplate by id
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(EmailTemplateLocalisation))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await GetAsync(uuid);
        }

        /// <summary>
        /// Updates an EmailTemplate
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(EmailTemplateLocalisation))]
        public async Task<IHttpActionResult> Put(EmailTemplateLocalisation obj, Guid uuid)
        {
            return await PutAsync(obj, uuid);
        }

        /// <summary>
        /// Creates a new EmailTemplate
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(EmailTemplateLocalisation))]
        public async Task<IHttpActionResult> Post(EmailTemplateLocalisation obj)
        {
            return await PostAsync(obj);
        }

        /// <summary>
        /// Deletes an EmailTemplate
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(EmailTemplateLocalisation))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await DeleteAsync(uuid);
        }
    }
}
