using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using Cartomatic.Utils.Data;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.DataModel.Validation;
using MapHive.Server.Core.Email;
using MapHive.Server.Core.Events;

namespace MapHive.Server.Core.DataModel
{

    public static partial class MapHiveUserCrudExtensions
    {
        /// <summary>
        /// Creates an object; returns a created object or null if it was not possible to create it due to the fact a uuid is already reserved
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAccount"></typeparam>
        /// <param name="obj"></param>
        /// <param name="dbCtx"></param>
        /// <param name="userAccountService"></param>
        /// <returns></returns>
        public static async Task<T> Create<T, TAccount>(this T obj, DbContext dbCtx, UserAccountService<TAccount> userAccountService)
            where T : MapHiveUser
            where TAccount : RelationalUserAccount
        {
            return await obj.Create<T, TAccount>(dbCtx, userAccountService);
        }
    }

    public abstract partial class MapHiveUser
    {
        /// <summary>
        /// Fired when user creation completes successfully
        /// </summary>
        /// <remarks>
        /// EvtHandler is a property so it is serializable by default. It contains some self refs, so serializers would go nuts. Need NonSerialized attribute. Not to mentions this property is not needeed on the user object anyway!
        /// </remarks>
        [NonSerialized]
        public EventHandler<IOpFeedbackEventArgs> UserCreated;

        /// <summary>
        /// Overrides the default Create method of Base and throws an exception. The default method cannot be used for a MapHive user object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        protected internal override Task<T> Create<T>(DbContext dbCtx)
        {
            throw new InvalidOperationException(WrongCrudMethodErrorInfo);
        }

        /// <summary>
        /// Creates a new user account in both MembershipReboot database and in the MapHive meta database;
        /// sends out a confirmation email if email account and template are provided
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAccount"></typeparam>
        /// <param name="userAccountService"></param>
        /// <param name="dbCtx"></param>
        /// <param name="emailAccount"></param>
        /// <param name="emailTemplate"></param>
        /// <returns></returns>
        protected internal virtual async Task<T> Create<T, TAccount>(DbContext dbCtx, UserAccountService<TAccount> userAccountService, EmailAccount emailAccount = null, IEmailTemplate emailTemplate = null)
            where T : MapHiveUser
            where TAccount : RelationalUserAccount
        {
            T output;

            //need to validate the model first
            this.Validate();

            //make sure the email is ALWAYS lower case
            Email = Email.ToLower();

            //check if the email is already used or not; throw validation feedback exception if so
            //Note - could do it in the mh meta, but both dbs must be in sync anyway
            var emailInUse = userAccountService.GetByEmail(Email) != null;
            if (emailInUse)
            {
                throw Validation.Utils.GenerateValidationFailedException(nameof(Email), ValidationErrors.EmailInUse);
            }

            //user account exists in two places - mbr and mh databases. Therefore need to handle them both properly wrapped into transactions

            DbContext mbrDbCtx = GetMembershipRebootDbCtx(userAccountService);
            System.Data.Common.DbTransaction mbrTrans = null;

            System.Data.Common.DbTransaction mhTrans = null;

            try
            {
                //open the connections as otherwise will not be able to begin transaction
                await mbrDbCtx.Database.Connection.OpenAsync();
                await dbCtx.Database.Connection.OpenAsync();

                //begin the transaction and set the transaction object back on the db context so it uses it
                mbrTrans = mbrDbCtx.Database.Connection.BeginTransaction();
                mbrDbCtx.Database.UseTransaction(mbrTrans);

                mhTrans = dbCtx.Database.Connection.BeginTransaction();
                dbCtx.Database.UseTransaction(mhTrans);


                //first create a membership reboot account
                //wire up evt too, to intercept what mbr is trying to say...
                AccountCreatedEvent<TAccount> e = null;

                userAccountService.Configuration.AddEventHandler(new MembershipRebootEventHandlers.AccountCreatedEventHandler<TAccount>(
                    (evt) =>
                    {
                        e = evt;
                    }));
                var newMbrAccount = userAccountService.CreateAccount(this.Email, Cartomatic.Utils.Crypto.Generator.GenerateRandomString(10), this.Email);

                //so can next pass some data to the mh meta user object
                this.Uuid = newMbrAccount.ID;

                //mbr work done, so can create the user within the mh metadata db
                output = await base.Create<T>(dbCtx);

                //looks like we're good to go, so can commit
                mbrTrans.Commit();
                mhTrans.Commit();


                var opFeedback = new Dictionary<string, object>
                {
                    {nameof(e.InitialPassword), e.InitialPassword},
                    {nameof(e.VerificationKey), e.VerificationKey}
                };

                //if email related objects have been provided, send the account created email
                if (emailAccount != null && emailTemplate != null)
                {
                    EmailSender.Send(
                        emailAccount, emailTemplate.Prepare(opFeedback), Email
                    );
                }

                //finally the user created event
                UserCreated?.Invoke(
                    this,
                    new Events.OpFeedbackEventArgs
                    {
                        OperationFeedback = opFeedback
                    }
                );
            }
            catch (Exception ex)
            {
                mbrTrans?.Rollback();
                mhTrans?.Rollback();

                throw Validation.Utils.GenerateValidationFailedException(ex);
            }
            finally
            {
                //try to close the connections as they were opened manually and therefore may not have been closed!
                dbCtx.Database.Connection.CloseConnection(dispose: true);
                mbrDbCtx.Database.Connection.CloseConnection(dispose: true);

                mbrTrans?.Dispose();
                mhTrans?.Dispose();
            }

            return output;
        }
    }
}
