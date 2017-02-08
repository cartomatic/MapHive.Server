using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MapHive.Server.Core.DAL.Extensions
{
    public static class DbContextExtensions
    {

        /// <summary>
        /// clones db context; requires the cloned ctx to implement a ctor that takes in 2 params (DbConnection conn and bool contextOwnsConnection);
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="contextOwnsConnection"></param>
        /// <returns></returns>
        public static System.Data.Entity.DbContext Clone(this System.Data.Entity.DbContext ctx, bool contextOwnsConnection = true)
        {
            //need to clone the connection too as have no idea how the connection has been provided to the ctx - directly, via conn str name as conn str, etc.
            var clonedConn = (DbConnection)Activator.CreateInstance(ctx.Database.Connection.GetType());
            clonedConn.ConnectionString = ctx.Database.Connection.ConnectionString;

            //this is where the ctx type is expected to provide a constructor that takes in DbConnection conn and bool contextOwnsConnection
            System.Data.Entity.DbContext clonedCtx = null;
            try
            {
                clonedCtx =
                    (System.Data.Entity.DbContext) Activator.CreateInstance(ctx.GetType(), new object[] {clonedConn, contextOwnsConnection});
            }
            catch
            {
                //ignore
            }

            if (clonedCtx == null)
            {
                try
                {
                    //ok looks, like ctx does not implement a (DbConnection conn and bool contextOwnsConnection) ctor
                    //need to mess a bit more.
                    //this time need to dig a bit deeper and get a conn str name

                    //set a read only conn prop now...
                    //looks like the 
                    var internalCtx = ctx.GetType().GetProperty("InternalContext", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ctx);
                    var connStrName = (string)internalCtx.GetType()
                        .GetProperty("ConnectionStringName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(internalCtx);

                    //try to create a ctx with a conn str name
                    clonedCtx = (System.Data.Entity.DbContext)Activator.CreateInstance(ctx.GetType(), new object[] { connStrName });

                    //Note: should set the ctx owns connection, but totally not sure where to find it...
                }
                catch
                {
                    //ignore
                }
            }

            if (clonedCtx == null)
            {
                throw new InvalidOperationException("It was not possible to clone the db context.");
            }

            return clonedCtx;
        }
    }
}
