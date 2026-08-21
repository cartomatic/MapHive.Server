using System;
using System.Collections.Generic;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel.Validation
{
    public class ValidationFailedException : Exception
    {
        public IList<IValidationError> ValidationErrors { get; set; } = new List<IValidationError>();
    } 
}
