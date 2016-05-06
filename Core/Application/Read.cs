using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dm = MapHive.Server.DataModel;

namespace MapHive.Server.Core
{
    public partial class Application
    {
        /// <summary>
        /// Returns a list of all the apps registered in the system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<dm.Application> Read()
        {
            //TODO: this should be dependant on DAL, so need to work out an interface for DAL, so the class is testable! ninject will come in handy
            return new List<dm.Application>
            {
                new dm.Application()
                {
                    Id = Guid.Parse("e645c160-e03d-40cf-8a5f-35eac0be7118"),
                    ShortName = "app1",
                    Name ="App 1", 
                    Description = "A test HOST app that suppresses nested framed apps",
                    Url = "https://app1.maphive.local/?suppressnested=true#some/hash/123/456"
                },
                new dm.Application()
                {
                    Id = Guid.Parse("58ec63c7-afc9-4ba6-86be-c4822034c086"),
                    //no short name, so can test id in the url part!
                    Name ="App 2",
                    Url = "https://app2.maphive.local/?param=test param so can be sure parameterised app urls also work&suppressnested=true",
                    UseSplashScreen = true,
                    RequiresAuth = true
                },
                new dm.Application()
                {
                    Id = Guid.Parse("9e263268-2da3-4134-9f05-fa65dfab44b6"),
                    ShortName = "app3x3",
                    Name ="App 3",
                    Url = "https://app3.maphive.local/",
                    RequiresAuth = true
                },
                new dm.Application()
                {
                    Id = Guid.Parse("413f8788-46c8-4bff-a516-56f3bcc30c9e"),
                    ShortName = "hgis1",
                    Name ="HGIS v1",
                    Description = "An initial port of the Cartomatic\'s HGIS",
                    Url = "https://hgis.maphive.local/"
                }
            };
        }
    }
}
