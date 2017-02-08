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
        /// Returns a configured class validator
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IValidator> GetValidators()
        {
            return null;
        }

        /// <summary>
        /// Validates the class data model; used prior to saving changes;
        /// </summary>
        /// <exception cref="ValidationFailedException"></exception>
        public virtual async Task ValidateAsync(DbContext dbCtx = null)
        {
            // Get validator class with config (validation criteria)
            var validators = GetValidators();

            // If validator is not found then model is valid by default
            if (validators == null)
                return;


            var validationFailedException = new ValidationFailedException();

            foreach (var validator in validators)
            {
                // Run fluend validation
                var result = validator.Validate(this);

                // If model is not valid then create exception response and throw generated exception 
                if (!result.IsValid)
                {


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
                }
            }

            if(validationFailedException.ValidationErrors.Count > 0)
                throw validationFailedException;

            await ValidateAgainstDbAsync(dbCtx);
        }

        /// <summary>
        /// Performs extra validation against database. An extenson point for performing validations that do depend on db state;
        /// if not valid should throw ValidationFailedException with appropriate ValidationError list
        /// </summary>
        /// <param name="dbCtx"></param>
        protected virtual async Task ValidateAgainstDbAsync(DbContext dbCtx)
        {
            return;
        }
    }
}
