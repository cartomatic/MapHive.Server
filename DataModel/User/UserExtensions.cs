using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.DataModel
{
    public static class UserExtensions
    {
        /// <summary>
        /// Gets a full user name based on user data, so pretty much name && surname? name surname : surname || name || email
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static string GetFullUserName(this User u)
        {
            return !string.IsNullOrEmpty(u.Forename) && !string.IsNullOrEmpty(u.Surname)
                ? $"{u.Forename} {u.Surname}"
                : !string.IsNullOrEmpty(u.Forename)
                    ? u.Forename
                    : !string.IsNullOrEmpty(u.Surname)
                        ? u.Surname
                        : u.Email;
        }
    }
}
