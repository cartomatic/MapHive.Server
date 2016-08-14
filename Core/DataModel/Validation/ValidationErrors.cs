using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Validators;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel.Validation
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

        public static IValidationError UniqueConstraint { get; } = new ValidationError
        {
            Code = "unique_constraint",
            Message = "Field value must be unique."
        };

        public static IValidationError NoIdentifier { get; } = new ValidationError
        {
            Code = "no_identifier",
            Message = "Dataset requires a unique identifier field."
        };

        public static IValidationError UnknownError { get; } = new ValidationError
        {
            Code = "unknown_error",
            Message = "Unknown error."
        };

    }
}
