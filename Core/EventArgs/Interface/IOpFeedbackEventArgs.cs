using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.EventArgs.Interface
{
    public interface IOpFeedbackEventArgs
    {
        /// <summary>
        /// Dict containing feedback on the operation performed.
        /// </summary>
        IDictionary<string, object> OperationFeedback { get; set; }
    }
}
