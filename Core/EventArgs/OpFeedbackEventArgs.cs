using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.EventArgs.Interface;

namespace MapHive.Server.Core.EventArgs
{
    /// <summary>
    /// Operation feedbac event arguments
    /// </summary>
    public class OpFeedbackEventArgs : System.EventArgs, IOpFeedbackEventArgs
    {
        /// <summary>
        /// Dict containing feedback on the operation performed.
        /// </summary>
        public IDictionary<string, object> OperationFeedback { get; set; } = new Dictionary<string, object>();
    }
}
