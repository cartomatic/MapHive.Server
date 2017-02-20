using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.UserConfiguration
{
    public interface IUserConfiguration
    {
        /// <summary>
        /// Reads configuration for a specified user; user info is obtained off the CurrentPrincipal
        /// </summary>
        /// <returns></returns>
        Task<IDictionary<string, object>> Read(string appUrl);
    }
}
