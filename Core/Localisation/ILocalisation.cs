﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Localisation
{
    public interface ILocalisation
    {
        ITranslations Translations { get; set; }
    }
}