﻿using System;
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

        // GET: /langs
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<Lang>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await base.GetAsync(sort, filter, start, limit);
        }

        // GET: /langs/5
        [HttpGet]
        [ResponseType(typeof(Lang))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.GetAsync(uuid);
        }

        // PUT: /langs/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(Lang))]
        public async Task<IHttpActionResult> Put(Lang obj, Guid uuid)
        {
            return await base.PutAsync(obj, uuid);
        }

        // POST: /langs
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Lang))]
        public async Task<IHttpActionResult> Post(Lang obj)
        {
            return await base.PostAsync(obj);
        }

        // DELETE: /langs/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(Lang))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await base.DeleteAsync(uuid);
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
