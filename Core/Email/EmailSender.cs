using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Email
{
    /// <summary>
    /// Email sending functionality
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// Sends an email to the recipient. Email is sent in a fire'n'forget manner.
        /// Note: in some scenarios fire'n'forget means the email may not eventually be sent at all.
        /// </summary>
        /// <param name="emailAccount">EmailAccount deails</param>
        /// <param name="emailTemplate">Email data to be sent out</param>
        /// <param name="recipient">Email of a recipient</param>
        public static void Send(IEmailAccount emailAccount, IEmailTemplate emailTemplate, string recipient)
        {
            Task.Run(() =>
            {
                var mail = new MailMessage();

                mail.To.Add(recipient);

                mail.From = new MailAddress(emailAccount.Sender);

                mail.Subject = emailTemplate.Title;
                mail.SubjectEncoding = Encoding.UTF8;

                mail.Body = emailTemplate.Body;
                mail.IsBodyHtml = emailTemplate.IsBodyHtml;
                mail.BodyEncoding = Encoding.UTF8;

                var smtp = new SmtpClient
                {
                    Host = emailAccount.SmtpHost,
                    Port = emailAccount.SmtpPort ?? 587,
                    Credentials = new System.Net.NetworkCredential(emailAccount.User, emailAccount.Pass),
                    EnableSsl = emailAccount.Ssl ?? false
                };

                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    //ignore
                }
            });
        }
    }
}
