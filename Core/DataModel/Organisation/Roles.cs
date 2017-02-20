using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DataModel
{
    
    public partial class Organisation
    {
        public enum OrganisationRole
        {
            Owner,
            Admin,
            Member
        }

        /// <summary>
        /// org owner role identifier. used to mark an owner role
        /// </summary>
        public const string OrgRoleIdentifierOwner = "org_owner";

        /// <summary>
        /// default en owner role name
        /// </summary>
        public const string OrgRoleNameOwner = "Owner";


        /// <summary>
        /// org admin role identifier; used to mark admin roles
        /// </summary>
        public static string OrgRoleIdentifierAdmin = "org_admin";

        /// <summary>
        /// default en admin role name
        /// </summary>
        public static string OrgRoleNameAdmin = "Admin";

        /// <summary>
        /// org member role identifier; used to mark standard members of an org
        /// </summary>
        public const string OrgRoleIdentifierMember = "org_member";

        /// <summary>
        /// default en member role name
        /// </summary>
        public const string OrgRoleNameMember = "Member";


        /// <summary>
        /// Creates an org role
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<Role> CreateRoleAsync(DbContext dbCtx, OrganisationRole role)
        {
            var roleName = string.Empty;
            var roleIdentifier = string.Empty;

            switch (role)
            {
                case OrganisationRole.Owner:
                    roleIdentifier = OrgRoleIdentifierOwner;
                    roleName = OrgRoleNameOwner;
                    break;

                    case OrganisationRole.Admin:
                    roleIdentifier = OrgRoleIdentifierAdmin;
                    roleName = OrgRoleNameAdmin;
                    break;

                default:
                    roleIdentifier = OrgRoleIdentifierMember;
                    roleName = OrgRoleNameMember;
                    break;
            }

            return await CreateRoleAsync(dbCtx, roleIdentifier, roleName);
        }

        /// <summary>
        /// Creates a role object for an organisation and links to an org; does not save org changes!
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<Role> CreateRoleAsync(DbContext dbCtx, string roleName)
        {
            return await CreateRoleAsync(dbCtx, null, roleName);
        }

        /// <summary>
        /// Creates a role object for an organisation and links to an org; does not save org changes!
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="roleIdentifier"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<Role> CreateRoleAsync(DbContext dbCtx, string roleIdentifier, string roleName)
        {
            var role = await new Role
            {
                Identifier = roleIdentifier,
                Name = roleName
            }.CreateAsync(dbCtx);

            this.AddLink(role);
            
            return role;
        }

        /// <summary>
        /// Gets organisation's owner role
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<Role> GetRoleOwnerAsync(DbContext dbCtx)
        {
            return (await this.GetChildrenAsync<Organisation, Role>(dbCtx)).FirstOrDefault(r=>r.Identifier == OrgRoleIdentifierOwner);
        }

        /// <summary>
        /// Gets organisation's admin role
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<Role> GetRoleAdminAsync(DbContext dbCtx)
        {
            return (await this.GetChildrenAsync<Organisation, Role>(dbCtx)).FirstOrDefault(r => r.Identifier == OrgRoleIdentifierAdmin);
        }

        /// <summary>
        /// Gets organisation's member role
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<Role> GetRoleMemberAsync(DbContext dbCtx)
        {
            return (await this.GetChildrenAsync<Organisation, Role>(dbCtx)).FirstOrDefault(r => r.Identifier == OrgRoleIdentifierMember);
        }

        /// <summary>
        /// Determines if a user is an org member (is assigned to an org)
        /// </summary>
        /// <param name="dbctx"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> IsOrgMember(DbContext dbctx, MapHiveUser user)
        {
            return await this.HasChildLinkAsync(dbctx, user);
        }

        /// <summary>
        /// Checks if user is an organisation owner (user has the org owner role assigned)
        /// </summary>
        /// <param name="dbctx"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> IsOrgOwner(DbContext dbctx, MapHiveUser user)
        {
            return await user.HasChildLinkAsync(dbctx, await GetRoleOwnerAsync(dbctx));
        }

        /// <summary>
        /// Checks if a user is an organisation admin (user has the org admin role assigned)
        /// </summary>
        /// <param name="dbctx"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> IsOrgAdmin(DbContext dbctx, MapHiveUser user)
        {
            return await user.HasChildLinkAsync(dbctx, await GetRoleAdminAsync(dbctx));
        }
    }
}
