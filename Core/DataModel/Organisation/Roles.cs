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
        public async Task<Role> GetOrgOwnerRoleAsync(DbContext dbCtx)
        {
            return (await this.GetChildrenAsync<Organisation, Role>(dbCtx)).FirstOrDefault(r=>r.Identifier == OrgRoleIdentifierOwner);
        }

        /// <summary>
        /// Gets organisation's admin role
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<Role> GetOrgAdminRoleAsync(DbContext dbCtx)
        {
            return (await this.GetChildrenAsync<Organisation, Role>(dbCtx)).FirstOrDefault(r => r.Identifier == OrgRoleIdentifierAdmin);
        }

        /// <summary>
        /// Gets organisation's member role
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<Role> GetOrgMemberRoleAsync(DbContext dbCtx)
        {
            return (await this.GetChildrenAsync<Organisation, Role>(dbCtx)).FirstOrDefault(r => r.Identifier == OrgRoleIdentifierMember);
        }

        /// <summary>
        /// Gets an organisation role
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<Role> GetOrgRoleAsync(DbContext dbCtx, OrganisationRole role)
        {
            switch (role)
            {
                case OrganisationRole.Admin:
                    return await GetOrgAdminRoleAsync(dbCtx);

                case OrganisationRole.Owner:
                    return await GetOrgOwnerRoleAsync(dbCtx);

                case OrganisationRole.Member:
                default:
                    return await GetOrgMemberRoleAsync(dbCtx);
            }
        }

        /// <summary>
        /// Gets OrgRoles
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Role>> GetOrgRolesAsync(DbContext dbCtx)
        {
            return (await this.GetChildrenAsync<Organisation, Role>(dbCtx)).Where(r => r.Identifier == OrgRoleIdentifierOwner || r.Identifier == OrgRoleIdentifierAdmin || r.Identifier == OrgRoleIdentifierMember);
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
            return await user.HasChildLinkAsync(dbctx, await GetOrgOwnerRoleAsync(dbctx));
        }

        /// <summary>
        /// Checks if a user is an organisation admin (user has the org admin role assigned)
        /// </summary>
        /// <param name="dbctx"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> IsOrgAdmin(DbContext dbctx, MapHiveUser user)
        {
            return await user.HasChildLinkAsync(dbctx, await GetOrgAdminRoleAsync(dbctx));
        }

        /// <summary>
        /// Gets a type of OrgRole from role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public OrganisationRole? GetOrgRoleFromRole(Role role)
        {
            if (role.Identifier == OrgRoleIdentifierMember)
                return OrganisationRole.Member;
            if (role.Identifier == OrgRoleIdentifierAdmin)
                return OrganisationRole.Admin;
            if (role.Identifier == OrgRoleIdentifierOwner)
                return OrganisationRole.Owner;
            return null;
        }

        /// <summary>
        /// Gets a mapping between org roles and users
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<Dictionary<OrganisationRole, IEnumerable<Link>>> GetOrgRoles2UsersMap(DbContext dbCtx)
        {
            //need to obtain a user role within an organisation!
            //so need the orgroles first, and then user ids linked to them
            var roles = await GetOrgRolesAsync(dbCtx);

            var roles2users = new Dictionary<OrganisationRole, IEnumerable<Link>>();

            foreach (var role in roles)
            {
                var orgRole = GetOrgRoleFromRole(role);
                if (orgRole.HasValue)
                {
                    roles2users.Add(orgRole.Value, await role.GetParentLinksAsync<Role, MapHiveUser>(dbCtx));
                }
            }

            return roles2users;
        }


        /// <summary>
        /// Works out a user role within an organisation
        /// </summary>
        /// <param name="roles2users"></param>
        /// <returns></returns>
        public OrganisationRole? GetUserOrgRole(Dictionary<OrganisationRole, IEnumerable<Link>> roles2users, Guid userId)
        {
            //Note: roles are linked to users not the other way round; it is a user that has a role

            //check owner firs
            if(roles2users[OrganisationRole.Owner].Any(l => l.ParentUuid == userId))
                return OrganisationRole.Owner;
            if (roles2users[OrganisationRole.Admin].Any(l => l.ParentUuid == userId))
                return OrganisationRole.Admin;
            if (roles2users[OrganisationRole.Member].Any(l => l.ParentUuid == userId))
                return OrganisationRole.Member;

            return null;
        }
    }
}
