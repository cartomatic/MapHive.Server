using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapHive.Server.Core.DataModel.Interface
{
    interface ILinkData
    {
        string Serialised { get; set; }
    }
}
