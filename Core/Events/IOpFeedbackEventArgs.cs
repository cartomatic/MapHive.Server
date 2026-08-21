using System.Collections.Generic;

namespace MapHive.Server.Core.Events
{
    public interface IOpFeedbackEventArgs
    {
        /// <summary>
        /// Dict containing feedback on the operation performed.
        /// </summary>
        IDictionary<string, object> OperationFeedback { get; set; }
    }
}
