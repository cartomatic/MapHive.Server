using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DataModel
{
    public partial class MapHiveUser
    {
        public string Forename { get; set; }

        public string Surname { get; set; }
        
        /// <summary>
        /// used only when user is an independent user. Slug name becomes an org slug. This way it is possible to maintain understandable urls when working in a context of a user/org
        /// </summary>
        public string Slug { get; set; }


        /// <summary>
        /// Some basic info on the user. Perhaps in a form of html.
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        /// Name of the company a user works for
        /// </summary>
        public string Company { get; set; }


        /// <summary>
        /// Dept a user work for
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// some info on user location; an address, coords, place name, whatever.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// When set, user's image will be pulled from the gravatar service
        /// </summary>
        public string GravatarEmail { get; set; }


        /// <summary>
        /// Profile picture. When present it will be used in the profile editor and whenever user info is required (log on info, msngrs, etc).
        /// This property is used to suck in the data when saving; pictures themselves are stored separately
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// Id of a profile picture. When present a picture is available. When not present picture, if any, is deleted
        /// </summary>
        public Guid? ProfilePictureId { get; set; }

        /// <summary>
        /// Whether or not user is an organisation user. Being an organisation user means user does not have its own organisation to work under and instead can only work in the context of other orgs he is linked to; this will usually be only one organisation, but technically a user can be linked to as many orgs as required
        /// </summary>
        public bool IsOrgUser { get; set; }


        /// <summary>
        /// Whether or not a user should be visible in the users catalogue; By default, when a user is an OrgUser ('belongs' to an organisation) he is not visible in the catalogue
        /// setting this property to true will cause the user will become findable in the catalogue.
        /// </summary>
        public bool VisibleInCatalogue { get; set; }


        /// <summary>
        /// identifier of an organisation that is connected to user profile
        /// </summary>
        public Guid? UserOrgId { get; set; }
    }
}
