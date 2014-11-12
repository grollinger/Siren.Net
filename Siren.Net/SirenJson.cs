namespace WebApiContrib.Formatting.Siren.Client
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static partial class SirenJson
    {
        internal const string CLASSES = "class";
        internal const string TITLE = "title";
        internal const string ENTITIES = "entities";
        internal const string LINKS = "links";
        internal const string HREF = "href";
        internal const string RELS = "rel";
        internal const string TYPE = "type";
        internal const string PROPERTIES = "properties";
        internal const string ACTIONS = "actions";
        internal const string FIELDS = "fields";
        internal const string METHOD = "method";
        internal const string NAME = "name";
        internal const string VALUE = "value";

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
                    classes.PropertyName = CLASSES;
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

        public static ISirenEntity Parse(JToken jobj)
        {   
            return Parser.ParseDocument(jobj);
        }

        public static JObject Unparse(ISirenEntity entity)
        {
            return Unparser.UnparseDocument(entity);
        }
    }
}
