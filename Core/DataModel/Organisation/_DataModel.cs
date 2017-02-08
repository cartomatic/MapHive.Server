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
    public partial class Organisation
    {
        /// <summary>
        /// org owner role identifier. used to mark an owner role
        /// </summary>
        public const string OrgRoleIdentifierOwner = "org_owner";

        /// <summary>
        /// org admin role identifier; used to mark admin roles
        /// </summary>
        public static string OrgRoleIdentifierAdmin = "org_admin";

        /// <summary>
        /// org member role identifier; used to mark standard members of an org
        /// </summary>
        public const string OrgRoleIdentifierMember = "org_member";



        /// <summary>
        /// used as the org identifier. this must be unique within the system.
        /// When an org is created for a user its name will be the same as the user slug.
        /// No spaces are allowed and chars must be allowed in the url
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Organisation display name
        /// </summary>
        public string DisplayName { get; set; }
        
        
        /// <summary>
        /// Org descrioption
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// some info on user location; an address, coords, place name, whatever.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Url of an org's public site
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// org contact email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// When set, user's image will be pulled from the gravatar service
        /// </summary>
        public string GravatarEmail { get; set; }


        /// <summary>
        /// Profile picture. When present it will be used in the profile editor and whenever user info is required (log on info, msngrs, etc);
        /// This property is used to suck in the data when saving; pictures themselves are stored separately
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// Id of a profile picture. When present a picture is available. When not present picture, if any, is deleted
        /// </summary>
        public Guid? ProfilePictureId { get; set; }

        //billing related stuff

        /// <summary>
        /// billing email if different than the contact email
        /// </summary>
        public string BillingEmail { get; set; }
        

        /// <summary>
        /// Billing address
        /// </summary>
        public string BillingAddress { get; set; }


        /// <summary>
        /// Extra information to be put on an invoice such as VAT No, Registration No, etc
        /// </summary>
        public StringPropertyCollection BillingExtraInfo { get; set; }

    }
}
