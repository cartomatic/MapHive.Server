using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel
{
    // <summary>
    /// When in a need to transfer some modified objects, but still closely mapped to the Core.DataModel.Base, this class provides the required properties;
    /// It should be kept intact with the Core.DataModel.Base, or just IBase
    /// </summary>
    public class Dto<TDto> : IBase, IDto<TDto>
        where TDto : class
    {
        //public int InsOr { get; set; }
        public Guid TypeUuid { get; set; }
        public Guid Uuid { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? CreateDateUtc { get; set; }
        public DateTime? ModifyDateUtc { get; set; }
        public DateTime? EndDateUtc { get; set; }

        //hide links behind explicit implementation
        ILinksDiff IBase.Links { get; set; }

        ILinkData IBase.LinkData { get; set; }

        /// <summary>
        /// Creates an instance of object from a DTO class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public T FromDto<T>(TDto o) where T : class
        {
            var core = o.CopyPublicPropertiesToNew<T>();

            FromDto(o, core);

            return core;
        }

        /// <summary>
        /// Converts instance of class to its DTO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public TDto ToDto<T>(T o) where T : class
        {
            var dto = o.CopyPublicPropertiesToNew<TDto>();

            ToDto(o, dto);

            return dto;
        }

        /// <summary>
        /// Customised data operations when transferring from DTO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        protected virtual void FromDto<T>(TDto src, T dest)
        {
            //do some hocus pocus data transfer in a derived class!
        }

        /// <summary>
        /// Customised data operations when transferring to DTO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        protected virtual void ToDto<T>(T src, TDto dest)
        {
            //do some hocus pocus data transfer in a derived class!
        }

        protected void CheckTypes<T1, T2>()
        {
            if (typeof(T1) != typeof(T2))
                throw new ArgumentException($"Invalid DataModel<->DTO types; Expected {typeof(T1).Name}, received {typeof(T2).Name}");
        }
    }
}
