using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel
{
    public partial class EmailTemplateLocalisation
    {
        /// <summary>
        /// Name of a template
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of a template
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name of the application a template is meant for; should be used along with the Identifier to narrow searches when looking up a template;
        /// a part of a unique key with identifier
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Template identifier; forms a unique key with Application
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Whether or not body template contains HTML
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// Template translations
        /// </summary>
        public EmailTranslations Translations { get; set; }

        /// <summary>
        /// explicit interface implementation
        /// </summary>
        ITranslations ILocalisation.Translations
        {
            get { return Translations; }

            set { Translations = (EmailTranslations)value; }
        }
    }
}
