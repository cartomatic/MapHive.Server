using System.Collections.Generic;

namespace MapHive.Server.Core.Email
{
    public interface IEmailTemplate
    {
        /// <summary>
        /// Email template as fed to email sender
        /// </summary>
        string Title { get; set; }

        string Body { get; set; }
        bool IsBodyHtml { get; set; }

        /// <summary>
        /// Prepares the template based on a collection of token/value to be applied
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        IEmailTemplate Prepare(IDictionary<string, object> tokens);
    }
}