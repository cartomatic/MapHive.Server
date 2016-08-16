using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Cartomatic.Utils.Web;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiController
    {
        //TODO - grab a referrer, url from param, and webconfig url if nothing else worked; needed to send a msg where a user can validate account, finalise pass reset, etc. Or maybe there should be a separate app tp dpo just this???

    }
}
