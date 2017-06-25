using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel
{
    public abstract partial class Base : IBase, IValidate
    {
        /// <summary>
        /// Type identifier - used to establish links between objects. not saved in a database;
        /// declared via class constructor.
        /// Important: when uuid is changed in code it will affect all the links in the database(s)
        /// </summary>
        public virtual Guid TypeUuid {
            get
            {
                try
                {
                    return this.GetTypeIdentifier();
                }
                catch
                {
                    return default(Guid);
                }
            }
        }

        protected Base()
        {
            if(TypeUuid == default (Guid))
                throw new Exception($"When deriving from MapHive.Server.Data.Base make sure to provide a unique type identifier! A model causing problems: {this.GetType()}");
        }
    }
}
