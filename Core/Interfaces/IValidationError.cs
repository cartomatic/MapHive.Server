using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Interfaces
{
    public interface IValidationError
    {
        /// <summary>
        /// Unique error code
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Name of a property that failed to validate
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Additional information that may be useful when providing customised error messages
        /// </summary>
        IDictionary<string, object> Info { get; set; }
    }
}
