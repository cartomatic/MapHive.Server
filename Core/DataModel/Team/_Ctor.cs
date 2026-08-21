using System;
using System.Reflection;

namespace MapHive.Server.Core.DataModel
{
    public partial class Team : Base
    {
        static Team()
        {
            BaseObjectTypeIdentifierExtensions.RegisterTypeIdentifier(MethodInfo.GetCurrentMethod().DeclaringType, Guid.Parse("907ef53f-9c2e-4463-bb52-3b6e97bc21ab"));
        }
    }
}
