using System;
using System.Reflection;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel
{
    public partial class TranslationKey : Base, ILocalisation
    {
        static TranslationKey()
        {
            BaseObjectTypeIdentifierExtensions.RegisterTypeIdentifier(MethodInfo.GetCurrentMethod().DeclaringType, Guid.Parse("987ce604-4125-44e6-bd6d-8db0857756a4"));
        }
    }
}
