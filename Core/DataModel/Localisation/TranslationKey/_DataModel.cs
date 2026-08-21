using System;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel
{
    public partial class TranslationKey : Base, ILocalisation
    {
        /// <summary>
        /// Identifier of a Localisation class name a translation applies to;
        /// </summary>
        public Guid LocalisationClassUuid { get; set; }

        /// <summary>
        /// A key that identifies the translation string within a translations class
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Set of translations for a particular key
        /// </summary>
        public Translations Translations { get; set; }


        ITranslations ILocalisation.Translations
        {
            get { return Translations; }

            set { Translations = (Translations)value; }
        }
    }
    
}
