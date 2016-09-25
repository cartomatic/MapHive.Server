﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    public partial class AppLocalisation
    {
        /// <summary>
        /// Application name a translation applies to; Fully qualified namespaces is achieved by combining it with the ClassName 
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Class name a translation applies to; Fully qualified namespaces is achieved by combining it with the ApplicationName 
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// A key that identifies the translation string within a translations class
        /// </summary>
        public string TranslationKey { get; set; }

        /// <summary>
        /// Set of translations for a particular key
        /// </summary>
        public Translations Translations { get; set; }
    }
}
