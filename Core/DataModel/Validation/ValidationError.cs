using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel.Validation
{
    public class ValidationError : IValidationError
    {
        /// <summary>
        /// Unique error code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Name of a property that failed to validate
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Additional information that may be useful when providing customised error messages
        /// </summary>
        public IDictionary<string, object> Info { get; set; } = new Dictionary<string, object>();
    }
}
