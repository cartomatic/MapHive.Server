using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DataModel
{
    public partial class MapHiveUser
    {
        /// <summary>
        /// makes sure the slug is present and valid
        /// </summary>
        protected void EnsureSlug()
        {
            if (!IsOrgUser && string.IsNullOrEmpty(Slug))
            {
                //assume the email is valid and just use it with @ replaced with _
                //this pretty much should ensure uniqueness as the emails must be unique anyway
                Slug = Email.Replace("@", "_");
            }
        }

        //todo - validate slug. it can only have a subset of chars, basically [A-Za-z0-9]
    }
}
