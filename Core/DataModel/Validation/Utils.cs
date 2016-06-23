using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel.Validation
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
        public static ValidationFailedException GenerateValidationFailedException(string property, IValidationError e)
        {
            return GenerateValidationFailedException(property, e.Code, e.Message);
        }

        /// <summary>
        /// Generates a ValidationFailedException with one validation error
        /// </summary>
        /// <param name="property"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static ValidationFailedException GenerateValidationFailedException(string msg)
        {
            return GenerateValidationFailedException(ValidationErrors.UnknownError.PropertyName, ValidationErrors.UnknownError.Code, msg);
        }

        /// <summary>
        /// Generates a ValidationFailedException with one validation error; tries to collect an error message combined with the inner exception messages
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ValidationFailedException GenerateValidationFailedException(Exception ex)
        {
            var msgs = new List<string> {ex.Message};
            var localEx = ex;
            while (localEx.InnerException != null)
            {
                msgs.Add(localEx.InnerException.Message);
                localEx = localEx.InnerException;
            }

            return GenerateValidationFailedException(string.Join(Environment.NewLine, msgs));
        }
    }
}
