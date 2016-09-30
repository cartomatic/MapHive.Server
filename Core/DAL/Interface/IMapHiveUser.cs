﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DAL.Interface
{
    /// <summary>
    /// Whether or not a dbctx has access to users property
    /// </summary>
    public interface IMapHiveUser<T>
        where T : MapHiveUserBase
    { 
        DbSet<T> Users { get; set; } 
    }
}
