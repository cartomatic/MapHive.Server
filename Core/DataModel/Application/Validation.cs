using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MapHive.Server.Core.DataModel.Validation;

namespace MapHive.Server.Core.DataModel
{
    public partial class Application
    {
        public override IEnumerable<IValidator> GetValidators()
        {
            var validators = new List<IValidator>();
            var baseValidators = base.GetValidators();
            if (baseValidators != null)
            {
                validators.AddRange(baseValidators);
            }
            validators.Add(new ApplicationValidator());
            return validators;
        }

        /// <summary>
        /// Application validator
        /// </summary>
        public class ApplicationValidator : AbstractValidator<Application>
        {
            public ApplicationValidator()
            {
                RuleFor(x => x.Name).WithValueRequired().WithLength(1, 254);

                RuleFor(x => x.ShortName).WithLength(1, 10);

                RuleFor(x => x.Urls).WithValueRequired().WithLength(1, 254);
            }
        }
    }
}
