using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.UserConfiguration
{
    public class UserConfigurationReader
    {
        /// <summary>
        /// Reads user specific configuration for the calling application; app url is the url extracted from the MH-Src header set by the client
        /// </summary>
        /// <param name="appUrl"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public static async Task<IDictionary<string, object>> ReadAsync(string appUrl, params IUserConfiguration[] configs)
        {
            var output = new Dictionary<string, object>();

            foreach (var config in configs)
            {
                try
                {
                    foreach (var cfg in await config.Read(appUrl))
                    {
                        if (output.ContainsKey(cfg.Key))
                        {
                            throw new Exception(
                                $"{cfg.Key} configuration key has already been reserved by another user configuration reader. Please change it.");
                        }
                        output.Add(cfg.Key, cfg.Value);
                    }
                }
                catch (Exception ex)
                {
                    output["Error"] = true;
                    output["ErrorMsg"] = ex.Message;
                }
            }

            return output;
        }
    }
}
