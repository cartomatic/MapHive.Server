using System;
using System.Reflection;

namespace MapHive.Server.Core.DataModel
{
    public partial class Lang : Base
    {
        static Lang()
        {
            BaseObjectTypeIdentifierExtensions.RegisterTypeIdentifier(MethodInfo.GetCurrentMethod().DeclaringType, Guid.Parse("f532f2c9-48ca-4f8f-aff2-0bb8ef9789a8"));
        }
    }
}
