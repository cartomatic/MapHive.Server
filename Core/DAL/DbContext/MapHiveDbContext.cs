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

namespace MapHive.Server.Core.DAL.DbContext
{
    public class MapHiveDbContext : BaseDbContext, ILinksDbContext, ILocalised, IXWindow, IMapHiveUser<MapHiveUser>
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
        public DbSet<MapHiveUser> Users { get; set; }

        //ILinksDbContext
        public DbSet<Link> Links { get; set; }

        //ILocalised
        public DbSet<LocalisationClass> LocalisationClasses { get; set; }
        public DbSet<TranslationKey> TranslationKeys { get; set; }
        public DbSet<EmailTemplateLocalisation> EmailTemplates { get; set; }
        public DbSet<Lang> Langs { get; set; }

        public DbSet<XWindowOrigin> XWindowOrigins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //type configs
            modelBuilder.Configurations.Add(new ApplicationConfiguration());
            modelBuilder.Configurations.Add(new MapHiveUserConfiguration());

            modelBuilder.Configurations.Add(new LinkConfiguration());

            //Ilocalised type configs
            modelBuilder.Configurations.Add(new LocalisationClassConfiguration());
            modelBuilder.Configurations.Add(new EmailTemplateLocalisationConfiguration());
            modelBuilder.Configurations.Add(new LangConfiguration());
            modelBuilder.Configurations.Add(new TranslationKeyConfiguration());

            modelBuilder.Configurations.Add(new XWindowOriginConfiguration());
        }
    }

    
}
