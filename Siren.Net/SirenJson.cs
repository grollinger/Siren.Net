namespace WebApiContrib.Formatting.Siren.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

    public static class SirenJson
    {
        private class ContractResolver : DefaultContractResolver
        {
            public ContractResolver()
            {

            }

            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var properties = base.CreateProperties(type, memberSerialization);

                if(type == typeof(SirenDocument))
                {
                    // Map differing property names
                    var classes = properties.Single(x => x.PropertyName == "Classes");
                    classes.PropertyName = "class";
                }

                return properties;
            }
        }

        private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings()
        {
            ContractResolver = new ContractResolver()
        };


        public static dynamic Parse(string jsonString)
        {
            return JsonConvert.DeserializeObject<SirenDocument>(jsonString, DefaultSettings);
        }

        public static T Parse<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString, DefaultSettings);
        }
    }
}
