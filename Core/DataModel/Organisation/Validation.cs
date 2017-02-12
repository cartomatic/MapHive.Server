using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.DataModel.Validation;

namespace MapHive.Server.Core.DataModel
{
    public partial class Organisation
    {
        public override IEnumerable<IValidator> GetValidators()
        {
            var validators = new List<IValidator>();
            var baseValidators = base.GetValidators();
            if (baseValidators != null)
            {
                validators.AddRange(baseValidators);
            }
            validators.Add(new OrganisationValidator());
            return validators;
        }

        /// <summary>
        /// Configuration for validation
        /// </summary>
        public class OrganisationValidator : AbstractValidator<Organisation>
        {
            public OrganisationValidator()
            {
                RuleFor(x => x.Slug).WithValueRequired();
            }
        }

        /// <summary>
        /// validates the org slug for uniqueness
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        protected override async Task ValidateAgainstDbAsync(DbContext dbCtx)
        {
            var slugTaken = !string.IsNullOrEmpty(Slug) &&
                await dbCtx.Set<Organisation>().AnyAsync(o => o.Uuid != Uuid && o.Slug == Slug)
                || await dbCtx.Set<MapHiveUser>().AnyAsync(u => u.UserOrgId != Uuid && u.Slug == Slug);


            if (slugTaken)
            {
                var validationFailedException = new ValidationFailedException();
                validationFailedException.ValidationErrors.Add(new ValidationError
                {
                    Message = "Organisation slug already taken.",
                    Code = "slug_taken",
                    PropertyName = nameof(Slug)
                });

                throw validationFailedException;
            }
        }
    }
}
