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
    }
}
