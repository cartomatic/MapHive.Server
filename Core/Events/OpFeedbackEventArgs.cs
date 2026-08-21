using System.Collections.Generic;

namespace MapHive.Server.Core.Events
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
