using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Validation
{
    public class Utils
    {
        /// <summary>
        /// Generates a ValidationFailedException with one validation error
        /// </summary>
        /// <param name="property"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ValidationFailedException GenerateValidationFailedException(string property, string code, string msg)
        {
            var ex = new ValidationFailedException();
            ex.ValidationErrors.Add(new ValidationError
            {
                Code = code,
                Message = msg,
                PropertyName = property
            });

            return ex;
        }

        /// <summary>
        /// Generates a ValidationFailedException with one validation error
        /// </summary>
        /// <param name="property"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static ValidationFailedException GenerateValidationFailedException(string property, ValidationError e)
        {
            return GenerateValidationFailedException(property, e.Code, e.Message);
        }
    }
}
