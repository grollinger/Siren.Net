namespace WebApiContrib.Formatting.Siren.Client
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;

    public static partial class SirenJson
    {
        internal class ContractResolver : DefaultContractResolver
        {
            public ContractResolver()
            {

            }

            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var properties = base.CreateProperties(type, memberSerialization);

                if (IsSirenEntity(type))
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

        private static bool IsSirenEntity(Type type)
        {
            var sirenInterfaceType = typeof(ISirenEntity).GetTypeInfo();

            var info = type.GetTypeInfo();

            return sirenInterfaceType.IsAssignableFrom(info);
        }

        public static ISirenEntity Parse(string jsonString)
        {
            var jobj = JObject.Parse(jsonString);
            return Parser.ParseDocument(jobj);
        }

        public static ISirenEntity Parse(JObject jobj)
        {   
            return Parser.ParseDocument(jobj);
        }

        public static JObject Unparse(ISirenEntity entity)
        {
            return null;
        }
    }
}
