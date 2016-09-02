using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.DataModel.Validation;
using FluentValidation;

namespace MapHive.Server.Core.DataModel
{
    public abstract partial class Base
    {
        /// <summary>
        /// Default null validator, so a class data model is valid by default
        /// </summary>
        protected IValidator Validator { get; set; } = null;

        /// <summary>
        /// Returns a configured class validator
        /// </summary>
        /// <returns></returns>
        public virtual IValidator GetValidator()
        {
            return Validator;
        }

        /// <summary>
        /// Validates the class data model; used prior to saving changes;
        /// </summary>
        /// <exception cref="ValidationFailedException"></exception>
        public virtual async Task ValidateAsync(DbContext dbCtx = null)
        {
            // Get validator class with config (validation criteria)
            var validator = GetValidator();

            // If validator is not found then model is valid by default
            if (validator == null)
                return;

            // Run fluend validation
            var result = validator.Validate(this);

            // If model is not valid then create exception response and throw generated exception 
            if (!result.IsValid)
            {
                var validationFailedException = new ValidationFailedException();

                foreach (var error in result.Errors)
                {
                    validationFailedException.ValidationErrors.Add(
                        new ValidationError
                        {
                            Message = error.ErrorMessage,
                            Code = error.ErrorCode,
                            PropertyName = error.PropertyName,
                            Info = error.FormattedMessagePlaceholderValues
                        }
                    );

                }

                throw validationFailedException;
            }

            await ValidateAgainstDbAsync(dbCtx);
        }

        /// <summary>
        /// Performs extra validation against database. An extenson point for performing validations that do depend on db state
        /// </summary>
        /// <param name="dbCtx"></param>
        protected virtual async Task ValidateAgainstDbAsync(DbContext dbCtx)
        {
            return;
        }
    }
}
