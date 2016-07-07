namespace MapHive.Server.Core.Email
{
    public interface IEmailAccount
    {
        /// <summary>
        /// Sender email - from 
        /// </summary>
        string Sender { get; set; }

        /// <summary>
        /// Smtp host to be used to connect to the smtp server
        /// </summary>
        string SmtpHost { get; set; }

        /// <summary>
        /// Smtp port
        /// </summary>
        int? SmtpPort { get; set; }

        /// <summary>
        /// Email account user used to login to the smtp server
        /// </summary>
        string User { get; set; }

        /// <summary>
        /// Password used to login to the smtp server
        /// </summary>
        string Pass { get; set; }

        /// <summary>
        /// Whether or not the communication should be encrypted
        /// </summary>
        bool? Ssl { get; set; }
    }
}