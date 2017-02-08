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
    /// Roles are the containers for system level and app level priviliges;
    /// A role as such does not have to define any priviliges as it is up to an application to properly define and understand a role meaning;
    /// </summary>
    public partial class Role
    {
        /// <summary>
        /// identifier of a role; some roles may have soecial meaning and use identifiers to uniquely address them
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Name of a role
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Description of a role
        /// </summary>
        public string Description { get; set; }


        //TODO - privs? Not yet sure though in what form. perhaps a serialisable dict of some sort
        //TODO - or maybe privs will be linked. this would be a bit over the edgo though i think as privs are not likely to be reused.

    }
}
