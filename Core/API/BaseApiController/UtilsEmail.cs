using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DAL.Interface;
using MapHive.Server.Core.Email;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiController
    {
        /// <summary>
        /// Extracts an email template and a confogured email account based on the email identifier. Since application name is not provided it only matches email identifiers for templates that do not have the app name set. Tries to work out the lang to extract the translation for email dynamically
        /// </summary>
        /// <param name="emailIdentifier"></param>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        protected async Task<Tuple<IEmailAccount, IEmailTemplate>> GetEmailStuff(string emailIdentifier,
            ILocalised dbCtx)
        {
            return await this.GetEmailStuff(emailIdentifier, string.Empty, dbCtx);
        }

        /// <summary>
        /// Extracts an email template and a configured email account based on the email template identifier and app identifier; Tries to work out the lang to extract the translation for email dynamically
        /// </summary>
        /// <param name="emailIdentifier"></param>
        /// <param name="appName"></param>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        protected async Task<Tuple<IEmailAccount, IEmailTemplate>> GetEmailStuff(string emailIdentifier, string appName, ILocalised dbCtx)
        {
            EmailAccount ea = null;
            EmailTemplate et = null;


            //try to locate an email template with the specified identifier
            //Note: this is a default webservice, so just lookig for an email with user_created identifier and no app
            //obviously this is a generic 'platform wide' email that can be customised on per app basis
            var emailTemplateLocalisation = string.IsNullOrEmpty(appName)
                ? await
                    dbCtx.EmailTemplates.Where(
                        t => string.IsNullOrEmpty(t.ApplicationName) && t.Identifier == emailIdentifier)
                        .FirstOrDefaultAsync()
                : await
                    dbCtx.EmailTemplates.Where(
                        t => t.ApplicationName == appName && t.Identifier == emailIdentifier)
                        .FirstOrDefaultAsync();

            //request or default lang
            var langCode = GetRequestLangCode();
            var defaultLangCode = await GetDefaultLang(dbCtx);

            //finaly 
            var localisedEmail = 
                emailTemplateLocalisation?.Translations?.FirstOrDefault(t => t.Key == langCode).Value 
                ?? emailTemplateLocalisation?.Translations?.FirstOrDefault(t => t.Key == defaultLangCode).Value 
                ?? emailTemplateLocalisation?.Translations?.FirstOrDefault().Value;

            //silently read cfg
            try
            {
                ea = EmailAccount.FromJson(ConfigurationManager.AppSettings["ServiceEmail"]);
            }
            catch
            {
                //ignore
            }

            //sooo, if there is a localised email and such can prepare the actual template
            if (localisedEmail != null && ea != null)
            {
                et = new Core.Email.EmailTemplate
                {
                    Title = localisedEmail?.Title,
                    Body = localisedEmail?.Body,
                    IsBodyHtml = emailTemplateLocalisation.IsBodyHtml
                };
            }


            return new Tuple<IEmailAccount, IEmailTemplate>(ea, et);
        }
    }
}
