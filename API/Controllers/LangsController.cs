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
    [RoutePrefix("langs")]
    public class LangsController : BaseApiCrudController<Lang, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public LangsController()
            : base("MapHiveMeta")
        {
        }

        /// <summary>
        /// Gets a collection of Langs
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<Lang>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await GetAsync(sort, filter, start, limit);
        }

        /// <summary>
        /// Gets Lang by id
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Lang))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await GetAsync(uuid);
        }

        /// <summary>
        /// Updates Lang
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(Lang))]
        public async Task<IHttpActionResult> Put(Lang obj, Guid uuid)
        {
            return await PutAsync(obj, uuid);
        }

        /// <summary>
        /// Creates a new Lang
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Lang))]
        public async Task<IHttpActionResult> Post(Lang obj)
        {
            return await PostAsync(obj);
        }

        /// <summary>
        /// Deletes Lang
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(Lang))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await DeleteAsync(uuid);
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
                return Ok(await Lang.GetDefaultLangAsync(_dbCtx as MapHiveDbContext));
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
                return Ok((await Lang.GetDefaultLangAsync(_dbCtx as MapHiveDbContext))?.LangCode);
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
                return Ok(await Lang.GetSupportedLangsAsync(_dbCtx as MapHiveDbContext));
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
                return Ok((await Lang.GetSupportedLangsAsync(_dbCtx as MapHiveDbContext)).Select(l => l.LangCode));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
