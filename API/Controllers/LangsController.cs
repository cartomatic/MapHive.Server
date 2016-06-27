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
using MapHive.Server.Core.DAL.Interface;
using MapHive.Server.DataModel;
using MapHive.Server.DataModel.DAL;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("langs")]
    public class LangsController : BaseApiController<AppLocalisation, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public LangsController()
            : base("MapHiveMeta")
        {
        }

        // GET: /applocalisations
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<Lang>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await base.Get(sort, filter, start, limit);
        }

        // GET: /applocalisations/5
        [HttpGet]
        [ResponseType(typeof(Lang))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.Get(uuid);
        }

        // PUT: /applocalisations/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(Lang))]
        public async Task<IHttpActionResult> Put(Lang obj, Guid uuid)
        {
            return await base.Put(obj, uuid);
        }

        // POST: /applocalisations
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(AppLocalisation))]
        public async Task<IHttpActionResult> Post(Lang obj)
        {
            return await base.Post(obj);
        }

        // DELETE: /applocalisations/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(Lang))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await base.Delete(uuid);
        }

        /// <summary>
        /// Gets a default lang
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Lang))]
        [Route("default")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetDefaultLang()
        {
            try
            {
                return Ok(await Lang.GetDefaultLang(_dbCtx as MapHiveDbContext));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets a default lang code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(string))]
        [Route("default/langcode")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetDefaultLangCode()
        {
            try
            {
                return Ok((await Lang.GetDefaultLang(_dbCtx as MapHiveDbContext))?.LangCode);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets supported langs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Lang>))]
        [Route("supported")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetSupportedLangs()
        {
            try
            {
                return Ok(await Lang.GetSupportedLangs(_dbCtx as MapHiveDbContext));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<string>))]
        [Route("supported/langcodes")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetSupportedLangCodes()
        {
            try
            {
                return Ok((await Lang.GetSupportedLangs(_dbCtx as MapHiveDbContext)).Select(l => l.LangCode));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
