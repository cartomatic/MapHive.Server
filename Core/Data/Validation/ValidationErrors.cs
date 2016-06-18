using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Validators;
using MapHive.Server.Core.Interfaces;

namespace MapHive.Server.Core.Validation
{
    /// <summary>
    /// Validation errors
    /// </summary>
    public static class ValidationErrors
    {
        public static IValidationError ValueRequired { get; } = new ValidationError()
        {
            Code = "value_required",
            Message = "Value for the field is required."
        };

        public static IValidationError InvalidLength { get; } = new ValidationError()
        {
            Code = "invalid_length",
            Message = "Invalid field length."
        };

        public static IValidationError InvalidEmail { get; } = new ValidationError()
        {
            Code = "invalid_email",
            Message = "Invalid email."
        };

        public static IValidationError EmailInUse { get; } = new ValidationError
        {
            Code = "email_in_use",
            Message = "Email in use."
        };

        public static IValidationError UnknownError { get; } = new ValidationError
        {
            Code = "unknown_error",
            Message = "Unknown error."
        };

    }
}
