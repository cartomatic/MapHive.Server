using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Owin;
using IdentityServer3.AccessTokenValidation;
using MapHive.Server.Core.API.Startup;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MapHive.Server.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.ConfigureMapHiveApi(
                "v1", "MapHiveApi",
                System.Web.Hosting.HostingEnvironment.MapPath("/bin/MapHive.Server.API.xml")
            );
        }
    }
}