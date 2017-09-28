using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.Email;

using MapHive.Identity.MembershipReboot;
using MapHive.Server.Core.Utils;

using Cartomatic.Utils.EF;
using MapHive.Server.Core.Events;

namespace MapHive.Server.Core
{
    public partial class Auth
    {
        /// <summary>
        /// Resends an activation link for a user; expects a user_created email template and a valid user identifier
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userId"></param>
        /// <param name="ea"></param>
        /// <param name="emailTpl"></param>
        /// <returns></returns>
        public static async Task ResendActivationLink(DbContext context, Guid userId, IEmailAccount ea = null, IEmailTemplate emailTpl = null)
        {
            //Note: MBR seems to not like resending an activation link with a new password... It simply does not generate a new one but rather regenerates the verification key
            //because of that need to fake an account creation with some random email and then grab some auto generated data out of it
            //
            //basically a new account is created in order to get a new token and a new pass and verification key with its creation date
            //such user account is then destroyed but the necessary details are set on the account that we try to resend a link for.



            if (userId == default(Guid))
                throw new InvalidOperationException("You cannot resend an activation link - this user has not yet been created...");


            var mbrCtx = new CustomDbContext("MapHiveMbr"); 

            //get the mbr user object
            var mbrUser = await mbrCtx.Users.FirstOrDefaultAsync(u => u.ID == userId);

            if (mbrUser == null)
                throw new InvalidOperationException("User does not exist in MBR.");


            //need user account service to properly create a MBR user
            var userAccountService = CustomUserAccountService.GetInstance("MapHiveMbr");

            //wire up an evt listener - this is the way mbr talks
            AccountCreatedEvent<CustomUserAccount> e = null;
            userAccountService.Configuration.AddEventHandler(
                new MembershipRebootEventHandlers.AccountCreatedEventHandler<CustomUserAccount>(evt => e = evt));

            //rrnd email - after all need to avoid scenarios when two folks try the same resend activation procedure at once
            var rndEmail = $"{DateTime.Now.Ticks}@somedomain.com";

            //finally a new rnd user, so we can get a properly recreated verification key and a new pass...
            var newMbrAccount = userAccountService.CreateAccount(rndEmail, Cartomatic.Utils.Crypto.Generator.GenerateRandomString(10), rndEmail);

            //update the account in question with 
            //mbrUser.VerificationKey = newMbrAccount.VerificationKey;
            //mbrUser.VerificationPurpose = newMbrAccount.VerificationPurpose;
            //mbrUser.HashedPassword = newMbrAccount.HashedPassword;
            //mbrUser.VerificationKeySent = newMbrAccount.VerificationKeySent
            //because the properties are read only, we need to do some crazy hocus-pocus again

            //note: looks like the type returned via mbrCtx.Users.FirstOrDefaultAsync is somewhat more dynamic and does not
            //map properly. therefore need to use a 'barebone' object instance
            var obj = new CustomUserAccount();


            //Warning - this sql is postgresql specific!
            var updateSql = $@"UPDATE
    {mbrCtx.GetTableSchema(obj)}.""{mbrCtx.GetTableName(obj)}""
SET
    ""{mbrCtx.GetTableColumnName(obj, nameof(mbrUser.VerificationKey))}"" = '{newMbrAccount.VerificationKey}',
    ""{mbrCtx.GetTableColumnName(obj, nameof(mbrUser.VerificationPurpose))}"" = {(int)newMbrAccount.VerificationPurpose},
    ""{mbrCtx.GetTableColumnName(obj, nameof(mbrUser.HashedPassword))}"" = '{newMbrAccount.HashedPassword}',
    ""{mbrCtx.GetTableColumnName(obj, nameof(mbrUser.VerificationKeySent))}"" = '{newMbrAccount.VerificationKeySent}'
WHERE
    ""{mbrCtx.GetTableColumnName(obj, nameof(mbrUser.ID))}"" = '{mbrUser.ID}';";

            //get rid of the new account
            mbrCtx.Users.Remove(await mbrCtx.Users.FirstAsync(u => u.ID == newMbrAccount.ID));

            //and save da mess
            await mbrCtx.SaveChangesAsync();
            await mbrCtx.Database.ExecuteSqlCommandAsync(updateSql);

            //send out the email
            if (emailTpl != null && ea != null)
            {
                MapHive.Server.Core.Email.EmailSender.Send(
                    ea,
                    emailTpl.Prepare(new Dictionary<string, object>
                    {
                        {nameof(e.VerificationKey), e.VerificationKey},
                        {nameof(e.InitialPassword), e.InitialPassword}
                    }),
                    mbrUser.Email
                );
            }
        }
    }
}
