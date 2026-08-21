using System;
using System.Reflection;
using MapHive.Server.Core.DataModel.SerialisableDict;

namespace MapHive.Server.Core.DataModel
{
    public partial class Organisation : Base
    {
        static Organisation()
        {
            BaseObjectTypeIdentifierExtensions.RegisterTypeIdentifier(MethodInfo.GetCurrentMethod().DeclaringType, Guid.Parse("0bc1402a-ec54-4e50-8e04-eb22a7625b91"));
        }

        public Organisation()
        {
            BillingExtraInfo = new StringPropertyCollection();
        }
    }
}
