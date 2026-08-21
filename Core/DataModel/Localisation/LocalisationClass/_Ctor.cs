using System;
using System.Reflection;

namespace MapHive.Server.Core.DataModel
{
    public partial class LocalisationClass : Base
    {
        static LocalisationClass()
        {
            BaseObjectTypeIdentifierExtensions.RegisterTypeIdentifier(MethodInfo.GetCurrentMethod().DeclaringType, Guid.Parse("03ad4b67-7801-4cf1-90dd-fe65674fc1e6"));
        }
    }
}
