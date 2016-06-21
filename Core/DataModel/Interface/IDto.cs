using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel.Interface
{
    /// <summary>
    /// To be used on DTO clases. Enables automated to / from DTO ops
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public interface IDto<TDto> where TDto: class
    {
        /// <summary>
        /// Creates an instance of object from a DTO class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        T FromDto<T>(TDto o) where T : class;

        /// <summary>
        /// Converts instance of class to its DTO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        TDto ToDto<T>(T o) where T : class;
    }
}
