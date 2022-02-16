using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    public static class DtoObjectExtensions
    {
        /// <summary>
        /// Copies public properties from a source object to a destination object
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        public static void CopyPublicPropertiesFrom(this object dest, object src)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public;

            var srcProps = src.GetType().GetProperties(flags);
            var dstProps = dest.GetType().GetProperties(flags);

            foreach (var srcFld in srcProps)
            {
                var dstFld = dstProps.FirstOrDefault(field => field.Name == srcFld.Name);

                if (dstFld != null && srcFld.PropertyType == dstFld.PropertyType && dstFld.GetSetMethod() != null)
                {
                    dstFld.SetValue(dest, srcFld.GetValue(src));
                }
            }
        }

        /// <summary>
        /// Copies public properties from a src object to a destination object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static T CopyPublicPropertiesToNew<T>(this object src) where T : class
        {
            var dest = (T)Activator.CreateInstance(typeof(T));
            dest.CopyPublicPropertiesFrom(src);
            return dest;
        }
    }
}
