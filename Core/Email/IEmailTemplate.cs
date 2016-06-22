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
    }
}