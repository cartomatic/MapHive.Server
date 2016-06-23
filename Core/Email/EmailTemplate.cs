using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Email
{
    /// <summary>
    /// Email template as fed to email sender
    /// </summary>
    public class EmailTemplate : IEmailTemplate
    {
        /// <summary>
        /// Email title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Email body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Whether or not email body is html
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// Prepares template by applying a set of values that replace tokens
        /// </summary>
        /// <param name="tokens">disctionary keys should be tokens, but without the curly braces. If the token is "{some_token}", the dictionary key is "some_token"</param>
        /// <returns></returns>
        public IEmailTemplate Prepare(IDictionary<string, object> tokens)
        {
            foreach (var token in tokens.Keys)
            {
                if (!string.IsNullOrEmpty(Title))
                {
                    Title = Title.Replace(GetReplacementToken(token), Convert.ToString(tokens[token], CultureInfo.InvariantCulture));
                }
                if (!string.IsNullOrEmpty(Body))
                {
                    Body = Body.Replace(GetReplacementToken(token), Convert.ToString(tokens[token], CultureInfo.InvariantCulture));
                }
            }
            return this;
        }

        /// <summary>
        /// Prepares a replacement string for a token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static string GetReplacementToken(string token)
        {
            return $"{("{")}{token}{("}")}";
        }
    }
}
