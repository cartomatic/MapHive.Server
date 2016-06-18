using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Interfaces
{

    /// <summary>
    /// A minimum required Base class model / functionality necessary to perform some standardised ops
    /// </summary>
    public interface IBase
    {
        Guid Uuid { get; set; }
        Guid? CreatedBy { get; set; }
        Guid? LastModifiedBy { get; set; }
        DateTime? CreateDate { get; set; }
        DateTime? ModifyDate { get; set; }
        DateTime? EndDate { get; set; }
    }
}
