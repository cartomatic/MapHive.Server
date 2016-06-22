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
        /// <param name="a">EmailAccount deails</param>
        /// <param name="t">Email data to be sent out</param>
        /// <param name="recipient">Email of a recipient</param>
        public static void Send(EmailAccount a, EmailTemplate t, string recipient)
        {
            Task.Run(() =>
            {
                var mail = new MailMessage();

                mail.To.Add(recipient);

                mail.From = new MailAddress(a.Sender);

                mail.Subject = t.Title;
                mail.SubjectEncoding = Encoding.UTF8;

                mail.Body = t.Body;
                mail.IsBodyHtml = t.IsBodyHtml;
                mail.BodyEncoding = Encoding.UTF8;

                var smtp = new SmtpClient
                {
                    Host = a.SmtpHost,
                    Port = a.SmtpPort ?? 587,
                    Credentials = new System.Net.NetworkCredential(a.User, a.Pass),
                    EnableSsl = a.Ssl ?? false
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
