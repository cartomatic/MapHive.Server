using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    public partial class Lang
    {
        /// <summary>
        /// Language identifier code
        /// </summary>
        public string LangCode { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Descripton
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Whether or not the language should be treated as default
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
