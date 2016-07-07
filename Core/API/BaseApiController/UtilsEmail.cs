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
    public abstract partial class BaseApiController<T, TDbCtx>
    {
        /// <summary>
        /// Extracts an email template and email account. Tries to work out the lang to extract the translation for email dynamically
        /// </summary>
        /// <param name="emailIdentifier"></param>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        protected async Task<Tuple<IEmailAccount, IEmailTemplate>> GetEmailStuff(string emailIdentifier, ILocalised dbCtx)
        {
            EmailAccount ea = null;
            EmailTemplate et = null;


            //try to locate an email template with the specified identifier
            //Note: this is a default webservice, so just lookig for an email with user_created identifier and no app
            //obviously this is a generic 'platform wide' email that can be customised on per app basis
            var emailTemplateLocalisation =
                await
                    dbCtx.EmailTemplates.Where(t => string.IsNullOrEmpty(t.ApplicationName) && t.Identifier == emailIdentifier).FirstOrDefaultAsync();

            //request or default lang
            var langCode = await GetRequestLangCode();
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
