using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Newtonsoft.Json.Serialization;

namespace MapHive.Server.Core.API.Serialisation
{
    /// <summary>
    /// decorate a controller to output camel cased json
    /// http://stackoverflow.com/questions/19956838/force-camalcase-on-asp-net-webapi-per-controller
    /// </summary>
    public class CamelCasedOutputControllerAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings httpControllerSettings, HttpControllerDescriptor httpControllerDescriptor)
        {
            //first remove the default json formatter
            var jsonMediaTypeFormatter = httpControllerSettings.Formatters.OfType<JsonMediaTypeFormatter>().Single();
            httpControllerSettings.Formatters.Remove(jsonMediaTypeFormatter);

            //and add a custgomised one 
            httpControllerSettings.Formatters.Add(new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            });
        }
    }
}
