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
        DbSet<LocalisationClass> LocalisationClasses { get; set; }
        DbSet<TranslationKey> TranslationKeys { get; set; }
        DbSet<EmailTemplateLocalisation> EmailTemplates { get; set; }
        DbSet<Lang> Langs { get; set; }
    }
}
