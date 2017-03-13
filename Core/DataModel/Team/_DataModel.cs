using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DataModel.SerialisableDict;

namespace MapHive.Server.Core.DataModel
{
    /// <summary>
    /// Team is a user grouping container.
    /// </summary>
    public partial class Team
    {
        /// <summary>
        /// Name of the object saved on an App -> Team link that stores team's app access credentials
        /// </summary>
        public const string AppAccessCredentialsLinkDataObject = "app_access_credentials";

        /// <summary>
        /// name of the amm admin access key
        /// </summary>

        public const string AppAdminAccess = "app_admin_access";

        /// <summary>
        /// Name of a team
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// team desscription
        /// </summary>
        public string Description { get; set; }
    }
}
