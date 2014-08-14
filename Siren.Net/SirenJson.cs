namespace WebApiContrib.Formatting.Siren.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using Newtonsoft.Json.Linq;

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
                
                if(IsSirenEntity(type))
                {
                    // Map differing property names
                    var classes = properties.Single(x => x.PropertyName == "Classes");
                    classes.PropertyName = "class";                    
                }

                return properties;
            }
        }

        private class HttpMethodJsonConverter : JsonConverter
        {                
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(HttpMethod);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new HttpMethod(reader.Value.ToString());
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var method = (HttpMethod)value;

                writer.WriteValue(method);
            }
        }

        private class UriJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Uri);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new Uri(reader.Value.ToString());
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }

        private class MediaTypeJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(MediaTypeHeaderValue);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new MediaTypeHeaderValue(reader.Value.ToString());
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(((MediaTypeHeaderValue)value).MediaType);
            }
        }

        private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings()
        {
            ContractResolver = new ContractResolver(),
            Converters = new List<JsonConverter>()
            {
                new HttpMethodJsonConverter(),
                new UriJsonConverter(),
                new MediaTypeJsonConverter()
            }
        };

        private static bool IsSirenEntity(Type type)
        {
            var sirenInterfaceType = typeof(ISirenEntity).GetTypeInfo();

            var info = type.GetTypeInfo();

            return sirenInterfaceType.IsAssignableFrom(info);
        }

        public static ISirenEntity ParseDocument(JObject obj)
        {
            var document = new DynamicSirenEntity();

            document.Title = ParseTitle(obj);
            document.Classes = ParseClasses(obj);
            document.Links = ParseLinks(obj);
            document.Properties = ParseProperties(obj);
            document.Actions = ParseActions(obj);
            document.Entities = ParseEntities(obj);

            return document;
        }

        public static ICollection<string> ParseClasses(JObject obj)
        {
            // class
            // Describes the nature of an entity's content based on the current representation. 
            // Possible values are implementation-dependent and should be documented. 
            // MUST be an array of strings. 
            // Optional.

            ICollection<string> result = null;
            var classes = obj["class"];

 	        if(classes != null)
            {
                if(classes is JArray)
                {
                    var classArray = classes as JArray;

                    result = (from tok in classArray
                              select tok.Value<string>()).ToList();
                }
                else
                {
                    throw new FormatException("'class' property is no array of string");
                }
            }

            return result ?? new List<string>();
        }

        public static IDictionary<string, object> ParseProperties(JObject obj)
        {
            // properties
            // A set of key-value pairs that describe the state of an entity. 
            // In JSON Siren, this is an object such as { "name": "Kevin", "age": 30 }. 
            // Optional.

            IDictionary<string, object> result = null;
            var properties = obj["properties"];

 	        if(properties != null)
            {
                if(properties is JObject)
                {
                    var propObject = properties as JObject;

                    result = new Dictionary<string, object>();

                    foreach (var keyVal in propObject)
                    {
                        result.Add(keyVal.Key, keyVal.Value);
                    }
                }
                else
                {
                    throw new FormatException("'properties' property is no json object");
                }
            }

            return result;
        }

        public static ICollection<IEmbeddedEntity> ParseEntities(JObject obj)
        {
            // entities 
            // A collection of related sub-entities. 
            // If a sub-entity contains an href value, it should be treated as an embedded link. 
            // Clients may choose to optimistically load embedded links. 
            // If no href value exists, the sub-entity is an embedded entity representation that contains all the characteristics of a typical entity. 
            // One difference is that a sub-entity MUST contain a rel attribute to describe its relationship to the parent entity.
            // In JSON Siren, this is represented as an array. Optional.
            return null;
        }


        public static ICollection<Link> ParseLinks(JObject obj)
        {
            // links
            // A collection of items that describe navigational links, distinct from entity relationships. 
            // Link items should contain a rel attribute to describe the relationship and an href attribute to point to the target URI. 
            // Entities should include a link rel to self. 
            // In JSON Siren, this is represented as "links": [{ "rel": ["self"], "href": "http://api.x.io/orders/1234" }] Optional.
            return null;
        }

        public static ICollection<Action> ParseActions(JObject obj)
        {
            // actions
            // A collection of action objects, represented in JSON Siren as an array such as { "actions": [{ ... }] }. 
            // See Actions. Optional

            // Actions 
            // show available behaviors an entity exposes.
            // name
            // A string that identifies the action to be performed. Required.
            // class
            // Describes the nature of an action based on the current representation. 
            // Possible values are implementation-dependent and should be documented. 
            // MUST be an array of strings. Optional.
            // method
            // An enumerated attribute mapping to a protocol method. 
            // For HTTP, these values may be GET, PUT, POST, DELETE, or PATCH. 
            // As new methods are introduced, this list can be extended. 
            // If this attribute is omitted, GET should be assumed. Optional.
            // href
            // The URI of the action. Required.
            // title
            // Descriptive text about the action. Optional.
            // type
            // The encoding type for the request. 
            // When omitted and the fields attribute exists, the default value is application/x-www-form-urlencoded. 
            // Optional.
            // fields
            // A collection of fields, expressed as an array of objects in JSON Siren such as { "fields" : [{ ... }] }. 
            // See Fields. Optional.
            return null;
        }

        public static ICollection<Field> ParseFields(JObject action)
        {
            // Fields
            // Fields represent controls inside of actions. They may contain these attributes:
            // name
            // A name describing the control. Required.
            // type
            // The input type of the field. This may include any of the following input types specified in HTML5:
            // hidden, text, search, tel, url, email, password, datetime, date, month, week, time, datetime-local, number, range, color, checkbox, radio, file, image, button
            // When missing, the default value is text. 
            // Serialization of these fields will depend on the value of the action's type attribute. 
            // See type under Actions, above. Optional.
            // value
            // A value assigned to the field. Optional.
            // title
            // Textual annotation of a field. Clients may use this as a label. Optional.
            return null;
        }

        private static string ParseTitle(JObject obj)
        {
            // title
            // Descriptive text about the entity. Optional.
            return null;
        }

        public static ISirenEntity Parse(string jsonString)
        {
            var jobj = JObject.Parse(jsonString);
            return ParseDocument(jobj);
        }
    }
}
