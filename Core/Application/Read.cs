using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core
{
    public partial class Application
    {
        /// <summary>
        /// Returns a list of all the apps registered in the system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Application> Read()
        {
            //TODO: this should be dependant on DAL, so need to work out an interface for DAL, so the class is testable! ninject will come in handy
            return new List<Application>
            {
                
            };
        }
    }
}
