﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DAL.TypeConfiguration
{
    public static class EntityTypeConfigurationExtensions
    {
        /// <summary>
        /// Takes care of setting up type configuration specific to the IBase model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static EntityTypeConfiguration<T> ApplyIBaseConfiguration<T>(this EntityTypeConfiguration<T> entity) where T : class, IBase
        {
            entity.HasKey(t => t.Uuid);

            entity.Property(en => en.InsOr).HasColumnName("insertion_order");
            entity.Property(en => en.InsOr).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            entity.Property(en => en.Uuid).HasColumnName("uuid");
            entity.Property(en => en.CreatedBy).HasColumnName("created_by");
            entity.Property(en => en.LastModifiedBy).HasColumnName("last_modified_by");
            entity.Property(en => en.CreateDate).HasColumnName("create_date");
            entity.Property(en => en.ModifyDate).HasColumnName("modify_date");
            entity.Property(en => en.EndDate).HasColumnName("end_date");

            entity.Ignore(p => p.TypeUuid);
            entity.Ignore(p => p.Links);

            return entity;
        }
    }
}
