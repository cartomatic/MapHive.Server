using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel.Interface
{
    public interface ILocalisation
    {
        ITranslations Translations { get; set; }
    }
}
