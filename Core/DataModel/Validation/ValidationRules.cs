using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MapHive.Server.Core.DataModel.Validation
{
    /// <summary>
    /// Helper class for generating IValidationError configurations
    /// </summary>
    public static class ValidationRules
    {

        public static IRuleBuilderOptions<TEntity, string> WithValueRequired<TEntity>(this IRuleBuilder<TEntity, string> builder)
           where TEntity : class
        {
            var item = ValidationErrors.ValueRequired;

           return builder.NotNull()
                .WithMessage(item.Message)
                .WithErrorCode(item.Code);
        }
        
        public static IRuleBuilderOptions<TEntity, string> WithLength<TEntity>(this IRuleBuilder<TEntity, string> builder, int minLength = 0, int maxLength = 10000)
           where TEntity : class
        {
            var item = ValidationErrors.InvalidLength;

         return  builder.Length(minLength, maxLength)
               .WithMessage(item.Message)
               .WithErrorCode(item.Code);
        }

        public static IRuleBuilderOptions<TEntity, string> WithMinLength<TEntity>(this IRuleBuilder<TEntity, string> builder, int minLength)
           where TEntity : class
        {
            var item = ValidationErrors.InvalidLength;

            return WithLength(builder, minLength);
        }

        public static IRuleBuilderOptions<TEntity, string> WithMaxLength<TEntity>(this IRuleBuilder<TEntity, string> builder, int maxLength)
           where TEntity : class
        {
            var item = ValidationErrors.InvalidLength;

            return WithLength(builder, maxLength);
        }

        public static IRuleBuilderOptions<TEntity, string> WithExactLength<TEntity>(this IRuleBuilder<TEntity, string> builder, int exactLength)
           where TEntity : class
        {
            var item = ValidationErrors.InvalidLength;

            return builder.Length(exactLength)
                  .WithMessage(item.Message)
                  .WithErrorCode(item.Code);
        }

        public static IRuleBuilderOptions<TEntity, string> WithValidEmailAddress<TEntity>(this IRuleBuilder<TEntity, string> builder)
           where TEntity : class
        {
            var item = ValidationErrors.InvalidEmail;

            return builder.EmailAddress()
                .WithMessage(item.Message)
                .WithErrorCode(item.Code);
        }
    }
}
