﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using Cartomatic.Utils.Data;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.DataModel.Validation;

namespace MapHive.Server.Core.DataModel
{
    public static partial class MapHiveUserCrudExtensions
    {
        /// <summary>
        /// Updates an object; returns an updated object or null if the object does not exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAccount"></typeparam>
        /// <param name="obj"></param>
        /// <param name="dbCtx"></param>
        /// <param name="userAccountService"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static async Task<T> Update<T, TAccount>(this T obj, DbContext dbCtx, UserAccountService<TAccount> userAccountService, Guid uuid)
            where T : MapHiveUser
            where TAccount : RelationalUserAccount
        {
            return await obj.Update<T, TAccount>(dbCtx, userAccountService, uuid);
        }
    }

    public abstract partial class MapHiveUser
    {
        /// <summary>
        /// Overrides the default Update method of Base and throws an exception. The default method cannot be used for a MapHive user object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected internal override Task<T> Update<T>(DbContext dbCtx, Guid uuid)
        {
            throw new InvalidOperationException(WrongCrudMethodErrorInfo);
        }

        /// <summary>
        /// Updates an object; returns an updated object or null if the object does not exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAccount"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="userAccountService"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected internal virtual async Task<T> Update<T, TAccount>(DbContext dbCtx, UserAccountService<TAccount> userAccountService, Guid uuid)
            where T : MapHiveUser
            where TAccount : RelationalUserAccount
        {
            T output;

            //need to validate the model first
            this.Validate();


            //make sure the email is ALWAYS lower case
            Email = Email.ToLower();


            //Note: user account resides in two places - MembershipReboot and the MapHive metadata database.
            //therefore need t manage it in tow places and obviously make sure the ops are properly wrapped into transactions

       
            //first get the user as saved in the db
            var mbrUser = userAccountService.GetByID(uuid);
            if (mbrUser == null)
                throw new BadRequestException(string.Empty);


            //in order to check if some mbr ops are needed need to compare the incoming data with the db equivalent
            var currentStateOfUser = await Read<T>(dbCtx, uuid);


            //work out if email is being updated and make sure to throw if it is not possible!
            var updateEmail = false;
            if (currentStateOfUser.Email != Email)
            {
                //looks like email is about to be changed, so need to check if it is possible to proceed
                var mbrUserWithSameEmail = userAccountService.GetByEmail(Email);
                if (mbrUserWithSameEmail != null && mbrUserWithSameEmail.ID != uuid)
                    throw Validation.Utils.GenerateValidationFailedException(nameof(Email), ValidationErrors.EmailInUse);

                //looks like we're good to go.
                updateEmail = true;
            }


            DbContext mbrDbCtx = GetMembershipRebootDbCtx(userAccountService);
            System.Data.Common.DbTransaction mbrTrans = null;

            System.Data.Common.DbTransaction mhTransaction = null;

            try
            {
                //open the connections as otherwise will not be able to begin transaction
                await mbrDbCtx.Database.Connection.OpenAsync();
                await dbCtx.Database.Connection.OpenAsync();

                //begin the transaction and set the transaction object back on the db context so it uses it
                //do so for both contexts - mbr and mh
                mbrTrans = mbrDbCtx.Database.Connection.BeginTransaction();
                mbrDbCtx.Database.UseTransaction(mbrTrans);

                mhTransaction = dbCtx.Database.Connection.BeginTransaction();
                dbCtx.Database.UseTransaction(mhTransaction);

                //check if mbr email related work is needed at all...
                if (updateEmail)
                {
                    //Note:
                    //since the change comes from the user edit, can assume this is an authorised operation...
                    userAccountService.SetConfirmedEmail(uuid, Email);
                }

                //also check the IsAccountClosed, as this may be modified via update too, not only via Destroy
                //btw. destroy just adjust the model's property and delegates the work to update
                if (currentStateOfUser.IsAccountClosed != IsAccountClosed)
                {
                    if (IsAccountClosed)
                    {
                        userAccountService.CloseAccount(uuid);
                    }
                    else
                    {
                        userAccountService.ReopenAccount(uuid);
                    }
                }

                //check the account verification status
                if (!mbrUser.IsAccountVerified)
                {
                    if (IsAccountVerified)
                    {
                        userAccountService.SetConfirmedEmail(uuid, Email);
                    }
                }
                else
                {
                    //force one way changes only
                    IsAccountVerified = true;
                }

                //mbr work done, so can update the user within the mh metadata db
                output = await base.Update<T>(dbCtx, uuid);


                //looks like we're good to go, so can commit
                mbrTrans.Commit();
                mhTransaction.Commit();
            }
            catch (Exception ex)
            {
                mbrTrans?.Rollback();
                mhTransaction?.Rollback();

                throw Validation.Utils.GenerateValidationFailedException(ex);
            }
            finally
            {
                //try to close the connections as they were opened manually and therefore may not have been closed!
                dbCtx.Database.Connection.CloseConnection(dispose: true);
                mbrDbCtx.Database.Connection.CloseConnection(dispose: true);

                mbrTrans?.Dispose();
                mhTransaction?.Dispose();
            }

            return output;
        }
    }
}
