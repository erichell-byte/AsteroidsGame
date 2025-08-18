using System;
using System.Collections.Generic;

namespace SaveLoad
{
    public class MapKeysProvider : IKeysProvider
    {
        private readonly IReadOnlyDictionary<Type, string> map = 
            new Dictionary<Type, string>
            {
                { typeof(PurchasedData), "PurchasedData" },
                { typeof(SpaceshipData), "SpaceshipData" }
            };
        
        public string Provide<TType>()
        {
            return map[typeof(TType)];
        }

        public IEnumerable<string> ProvideAll()
        {
            return map.Values;
        }
    }
}