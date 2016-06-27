using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MapHive.Server.Core.API.Serialisation
{
    /// <summary>
    /// Use to prevent changing dictionary keys changing
    /// </summary>
    public class UnmodifiedDictKeyCasingOutputMethodAttribute : CamelCasedOutputMethodAttribute
    {
        static UnmodifiedDictKeyCasingOutputMethodAttribute()
        {
            CamelCasingFormatter.SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCaseExceptDictionaryKeysResolver()
            };
        }
    }

    /// <summary>
    /// Custom dict resolver that does not change the dict keys
    /// </summary>
    internal class CamelCaseExceptDictionaryKeysResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            var contract = base.CreateDictionaryContract(objectType);

            //make sure to not change dict keys!
            contract.DictionaryKeyResolver = propertyName => propertyName;

            return contract;
        }
    }
}
