using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapHive.Server.Core.Data
{
    public partial class ReadFilter
    {
        public string Operator { get; set; }

        public dynamic Value { get; set; }

        public string Property { get; set; }
    }
}
