using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;
using MapHive.Server.Core.DAL.Interface;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.DAL.TypeConfiguration;
using MapHive.Server.DataModel.DAL.TypeConfiguration;

namespace MapHive.Server.DataModel.DAL
{
    public class MapHiveDbContext : BaseDbContext, ILinksDbContext, ILocalised, IXWindow
    {
        public MapHiveDbContext()
            : base() 
        {
        }

        public MapHiveDbContext(string connStringName)
            : base (connStringName)
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<User> Users { get; set; }

        //ILinksDbContext
        public DbSet<Link> Links { get; set; }

        //ILocalised
        public DbSet<AppLocalisation> AppLocalisations { get; set; }
        public DbSet<EmailTemplateLocalisation> EmailTemplates { get; set; }
        public DbSet<Lang> Langs { get; set; }

        public DbSet<XWindowOrigin> XWindowOrigins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("metadata");

            //type configs
            modelBuilder.Configurations.Add(new ApplicationConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());

            modelBuilder.Configurations.Add(new LinkConfiguration());

            modelBuilder.Configurations.Add(new AppLocalisationConfiguration());
            modelBuilder.Configurations.Add(new EmailTemplateLocalisationConfiguration());
            modelBuilder.Configurations.Add(new LangConfiguration());

            modelBuilder.Configurations.Add(new XWindowOriginConfiguration());
        }
    }

    
}
