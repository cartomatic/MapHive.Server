using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MapHive.Identity.MembershipReboot;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.Email;

namespace MapHive.Server.API.Utils
{
    public class User
    {
        /// <summary>
        /// Standardises user creation
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="user"></param>
        /// <param name="emailStuff"></param>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public static async Task<MapHiveUser> CreateUser(DbContext dbCtx, MapHiveUser user,
            Tuple<IEmailAccount, IEmailTemplate> emailStuff, string redirectUrl)
        {
            //Note: there are 2 options to send emails when creation a user account:
            //1. listen to UserCreated evt on the User object and then process email manually
            //2. grab the appropriate email template and email account, potentially adjust some email template tokens prior to creating a user and pass both sender account and email template to a user creation procedure

            //In this scenario a second approach is used

            //initial email template customisation:
            //{UserName}
            //{Email}
            //{RedirectUrl}
            var replacementData = new Dictionary<string, object>
                {
                    {"UserName", $"{user.GetFullUserName()} ({user.Email})"},
                    {"Email", user.Email},
                    {"RedirectUrl", redirectUrl}
                };

            emailStuff?.Item2.Prepare(replacementData);

            return await user.CreateAsync(dbCtx, CustomUserAccountService.GetInstance("MapHiveMbr"), emailStuff?.Item1, emailStuff?.Item2);
        }
    }
}