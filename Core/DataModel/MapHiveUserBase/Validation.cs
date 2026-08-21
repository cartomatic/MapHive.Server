using System.Collections.Generic;
using FluentValidation;
using MapHive.Server.Core.DataModel.Validation;

namespace MapHive.Server.Core.DataModel
{
    public abstract partial class MapHiveUserBase
    {
        public override IEnumerable<IValidator> GetValidators()
        {
            var validators = new List<IValidator>();
            var baseValidators = base.GetValidators();
            if (baseValidators != null)
            {
                validators.AddRange(baseValidators);
            }
            validators.Add(new UserValidator());
            return validators;
        }

        /// <summary>
        /// Configuration for validation
        /// </summary>
        public class UserValidator : AbstractValidator<MapHiveUserBase>
        {
            public UserValidator()
            {
                RuleFor(x => x.Email).WithValueRequired().WithValidEmailAddress();
            }
        }
    }
}
