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
        protected internal override async Task<T> CreateAsync<T>(DbContext dbCtx)
        {
            var org = await base.CreateAsync<T>(dbCtx) as Organisation;

            //once an org is present need to create the default roles and link them with an org
            var rOwner = await CreateRoleAsync(dbCtx, OrgRoleIdentifierOwner, OrgRoleNameOwner);
            var rAdmin = await CreateRoleAsync(dbCtx, OrgRoleIdentifierAdmin, OrgRoleNameAdmin);
            var rMember = await CreateRoleAsync(dbCtx, OrgRoleIdentifierMember, OrgRoleNameMember);

            //link roles to org
            org.AddLink(rOwner);
            org.AddLink(rAdmin);
            org.AddLink(rMember);

            //and update the org
            await org.UpdateAsync(dbCtx);

            return (T)(Base)org;
        }

        private async Task<Role> CreateRoleAsync(DbContext dbCtx, string roleIdentifier, string roleName)
        {
            return await new Role
            {
                Identifier = roleIdentifier,
                Name = roleName
            }.CreateAsync(dbCtx);
        }
    }
}
