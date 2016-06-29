using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.DataModel.Validation;

namespace MapHive.Server.Core.DataModel
{
    public partial class XWindowOrigin
    {
        public override IValidator GetValidator()
        {
            if (Validator == null)
            {
                Validator = new UserValidator();
            }
            return base.GetValidator();
        }

        /// <summary>
        /// Configuration for validation
        /// </summary>
        public class UserValidator : AbstractValidator<XWindowOrigin>
        {
            public UserValidator()
            {
                RuleFor(x => x.Origin).WithValueRequired();
            }
        }
    }
}
