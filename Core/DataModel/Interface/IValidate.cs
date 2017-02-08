using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        /// Gets the validators
        /// </summary>
        /// <returns></returns>
        IEnumerable<IValidator> GetValidators();

        /// <summary>
        /// Validates data model. Should throw when model is not valid; expected to be async!
        /// </summary>
        Task ValidateAsync(DbContext dbCtx);
    }
}
