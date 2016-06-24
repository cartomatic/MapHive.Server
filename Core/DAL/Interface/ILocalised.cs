using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DAL.Interface
{
    /// <summary>
    /// Interface for a DbContext that provides access to localisation data
    /// </summary>
    public interface ILocalised
    {
        DbSet<AppLocalisation> AppLocalisations { get; set; }
        DbSet<EmailTemplateLocalisation> EmailTemplates { get; set; }
    }
}
