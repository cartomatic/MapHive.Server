﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel
{
    public partial class EmailTemplateLocalisation : Base, ILocalisation
    {
        static EmailTemplateLocalisation()
        {
            BaseObjectTypeIdentifierExtensions.RegisterTypeIdentifier(MethodInfo.GetCurrentMethod().DeclaringType, Guid.Parse("8226738b-8d7b-42e7-bfd6-30cf4f55d57e"));
        }
    }
}
