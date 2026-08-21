using System.Data.Entity;
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
