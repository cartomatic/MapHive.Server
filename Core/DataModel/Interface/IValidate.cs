using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MapHive.Server.Core.DataModel.Interface
{
    /// <summary>
    /// Whether or not a class exposes functionality to validate its data model
    /// </summary>
    public interface IValidate
    {
        /// <summary>
        /// Gets the validator
        /// </summary>
        /// <returns></returns>
        IValidator GetValidator();

        /// <summary>
        /// Validates data model. Should throw when model is not valid
        /// </summary>
        void Validate();
    }
}
