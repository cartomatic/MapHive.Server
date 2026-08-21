using System;
using System.Reflection;

namespace MapHive.Server.Core.DataModel
{
    public partial class Application : Base
    {
        static Application()
        {
            BaseObjectTypeIdentifierExtensions.RegisterTypeIdentifier(MethodInfo.GetCurrentMethod().DeclaringType, Guid.Parse("a980c990-656f-47ca-8969-100853866d7b"));
        }
    }
}
