using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.Interfaces;

namespace MapHive.Server.Core.Validation
{
    public class ValidationFailedException : Exception
    {
        public IList<IValidationError> ValidationErrors { get; set; } = new List<IValidationError>();
    } 
}
