﻿using System;
using System.Collections.Generic;
using System.Data.Common;
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
    public class MapHiveDbContext : BaseDbContext, ILinksDbContext, IMapHiveApps, ILocalised, IMapHiveUsers<MapHiveUser>
    {
        public MapHiveDbContext()
            : this("MapHiveMeta") //use a default conn str name; useful when passing ctx as a generic param that is then instantiated 
        {
        }

        public MapHiveDbContext(string connStringName)
            : base (connStringName)
        {
        }

        public MapHiveDbContext(DbConnection conn, bool contextOwnsConnection) 
            : base (conn, contextOwnsConnection)
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<MapHiveUser> Users { get; set; }

        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }

        //ILinksDbContext
        public DbSet<Link> Links { get; set; }

        //ILocalised
        public DbSet<LocalisationClass> LocalisationClasses { get; set; }
        public DbSet<TranslationKey> TranslationKeys { get; set; }
        public DbSet<EmailTemplateLocalisation> EmailTemplates { get; set; }
        public DbSet<Lang> Langs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("mh_meta");

            //type configs
            modelBuilder.Configurations.Add(new ApplicationConfiguration());
            modelBuilder.Configurations.Add(new MapHiveUserConfiguration());
            modelBuilder.Configurations.Add(new OrganisationConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new TeamConfiguration());

            modelBuilder.Configurations.Add(new LinkConfiguration());

            //Ilocalised type configs
            modelBuilder.Configurations.Add(new LocalisationClassConfiguration());
            modelBuilder.Configurations.Add(new EmailTemplateLocalisationConfiguration());
            modelBuilder.Configurations.Add(new LangConfiguration());
            modelBuilder.Configurations.Add(new TranslationKeyConfiguration());

        }
    }

    
}
