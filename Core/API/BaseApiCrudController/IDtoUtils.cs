using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiCrudController<T, TDbCtx>
    {
        public IDto<TDto> CrateIDtoInstance<TDto>()
            where TDto : class
        {
            var inst = Activator.CreateInstance(typeof(TDto)) as IDto<TDto>;

            if (inst == null)
                throw new ArgumentException(
                    "Could not create IDto<TDto> instance. When using the auto DTO feature, TDto object type must implement IDto<TDto>!");

            return inst;
        }
    }
}
