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
        public override IValidator GetValidator()
        {
            if (Validator == null)
            {
                Validator = new ApplicationValidator();
            }
            return base.GetValidator();
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
