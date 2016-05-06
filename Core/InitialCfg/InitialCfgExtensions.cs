using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace MapHive.Server.Core
{
    public static class InitialCfgExtensions
    {
        /// <summary>
        /// Injects dynamic mh configuration script
        /// </summary>
        /// <param name="p"></param>
        /// <param name="appUrl"></param>
        public static void InjectMhCfg(this System.Web.UI.Page p, string appUrl)
        {
            //search for title index, so can inject the script just after that
            //assuming that all the meta stuff goes before the title though...
            var idx = 0;
            foreach (var control in p.Header.Controls)
            {
                if (control.GetType() == typeof(System.Web.UI.HtmlControls.HtmlTitle))
                {
                    break;
                }
                idx += 1;
            }

            //Just inject a script with mh dynamic configuration
            p.Header.Controls.AddAt(++idx, new LiteralControl(Environment.NewLine + Environment.NewLine + '\t'));

            System.Web.UI.HtmlControls.HtmlGenericControl mhCfgScript =
            new System.Web.UI.HtmlControls.HtmlGenericControl("script");
            mhCfgScript.Attributes["type"] = "text/javascript";
            mhCfgScript.InnerHtml = MapHive.Server.Core.InitialCfg.GetScriptContent(appUrl);
            p.Header.Controls.AddAt(++idx, mhCfgScript);
        }
    }
}
