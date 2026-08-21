using System;
using System.Reflection;

namespace MapHive.Server.Core.DataModel
{
    /// <summary>
    /// Customised user
    /// </summary>
    public partial class MapHiveUser : MapHiveUserBase
    {
        static MapHiveUser()
        {
            BaseObjectTypeIdentifierExtensions.RegisterTypeIdentifier(MethodInfo.GetCurrentMethod().DeclaringType, Guid.Parse("d4f2f07c-8712-456c-9ad0-8ac5adc67178"));
        }
    }
}
