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
    public partial class MapHiveUser
    {
        public override IEnumerable<IValidator> GetValidators()
        {
            var validators = new List<IValidator>();
            var baseValidators = base.GetValidators();
            if (baseValidators != null)
            {
                validators.AddRange(baseValidators);
            }
            validators.Add(new MapHiveUserValidator());
            return validators;
        }

        /// <summary>
        /// Configuration for validation
        /// </summary>
        public class MapHiveUserValidator : AbstractValidator<MapHiveUser>
        {
            public MapHiveUserValidator()
            {
                
            }
        }

        /// <summary>
        /// validates the org slug for uniqueness
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        protected override async Task ValidateAgainstDbAsync(DbContext dbCtx)
        {
            var validationFailedException = new ValidationFailedException();

            if (await dbCtx.Set<MapHiveUser>().AnyAsync(u => u.Uuid != Uuid && u.Email == Email))
            {
                throw Validation.Utils.GenerateValidationFailedException(nameof(Email), ValidationErrors.EmailInUse);
            }

            Organisation org = null;
            if (Uuid != default(Guid))
            {
                org = await GetUserOrganisationAsync(dbCtx);
            }

            var slugTaken = !string.IsNullOrEmpty(Slug) &&
                //another user with given slug exists
                await dbCtx.Set<MapHiveUser>().AnyAsync(u => u.Uuid != Uuid && u.Slug == Slug)
                //user has an org but there is another org with given slug
                || (org != null && await dbCtx.Set<Organisation>().AnyAsync(o => o.Uuid != org.Uuid && o.Slug == Slug))
                //another org exists that has reserved the slug
                || (org == null && await dbCtx.Set<Organisation>().AnyAsync(o => o.Slug == Slug));

            if (slugTaken)
            {
                
                validationFailedException.ValidationErrors.Add(new ValidationError
                {
                    Message = "MapHiveUser slug already taken.",
                    Code = "slug_taken",
                    PropertyName = nameof(Slug)
                });

                throw validationFailedException;
            }
        }
    }
}
