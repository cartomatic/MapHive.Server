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
            var slugTaken = await dbCtx.Set<MapHiveUser>().FirstOrDefaultAsync(u => u.Uuid != Uuid && u.Slug == Slug);

            if (slugTaken != null)
            {
                var validationFailedException = new ValidationFailedException();
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
