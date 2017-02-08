using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DAL.DbContext
{
    /// <summary>
    /// Customised DbContext that should be used for the IBase models. Takes care of automated creator / editor nad create / modify dates information application
    /// </summary>
    public class BaseDbContext : System.Data.Entity.DbContext
    {
        public BaseDbContext()
            //Note:
            //need a paramless constructor that passes a non empty conn string or conn string name to DbContext
            //Obviously using such a db context will make it throw, so this is not a constructor one would use.
            //although providing a valid paramles ctor in a derived class (one that calls base("some_real_conn_str_name") will do.
            : this("need_this_for_migrations!!!")
        {
        }
        public BaseDbContext(string connStringName) : base(connStringName)
        {
        }

        public BaseDbContext(DbConnection conn, bool contextOwnsConnection) : base (conn, contextOwnsConnection) { }

        /// <summary>
        /// Updates some custom IBase related properties
        /// </summary>
        private void UpdateDateProperties()
        {
            //grab all the IBase types
            var changeSet = ChangeTracker.Entries<IBase>();

            if (changeSet == null)
                return;

            //grab user identifier. since got here, user roles should have been checked by the appropriate crud methods, so guid should be obtainable
            //if not present though, must throw
            var guid = Utils.Identity.GetUserGuid();

            if (!guid.HasValue)
                throw new Exception("Failed to impersonate user.");


            foreach (var entry in changeSet.Where(c => c.State != EntityState.Unchanged))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreateDateUtc = DateTime.UtcNow;
                    entry.Entity.CreatedBy = guid;
                }
                else
                {
                    entry.Property(x => x.CreateDateUtc).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                }

                entry.Entity.ModifyDateUtc = DateTime.UtcNow;
                entry.Entity.LastModifiedBy = guid;
            }
        }

        /// <summary>
        /// Saves changes applying all the scustom stuff...
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            UpdateDateProperties();

            return base.SaveChanges();
        }

        /// <summary>
        /// Saves changes applying all the scustom stuff...
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync()
        {
            UpdateDateProperties();

            return await base.SaveChangesAsync();
        }
    }
}
