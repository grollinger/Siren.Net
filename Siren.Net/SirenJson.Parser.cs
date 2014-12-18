namespace WebApiContrib.Formatting.Siren.Client
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public static partial class SirenJson
    {
        public static class Parser
        {
            internal static ISirenEntity ParseDocument(JToken tok)
            {
                if (!(tok is JObject))
                {
                    throw new FormatException("A Siren Document's top-level JSON Token must be an object");
                }

                var obj = tok as JObject;

                var document = new SirenEntity();

                document.Title = ParseStringOptional(obj, "title", "An Entity's 'title' must be a string if it exists");
                document.Classes = ParseClasses(obj);
                document.Links = ParseLinks(obj);
                document.Properties = ParseProperties(obj);
                document.Actions = ParseActions(obj);
                var embedded = ParseEntities(obj);
                document.EmbeddedLinks = embedded.Item1;
                document.EmbeddedRepresentations = embedded.Item2;

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

                if (classes != null)
                {
                    if (classes is JArray)
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

                IDictionary<string, object> result = new Dictionary<string, object>();
                var properties = obj["properties"];

                if (properties != null)
                {
                    if (properties is JObject)
                    {
                        var propObject = properties as JObject;

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

            public static Tuple<ICollection<IEmbeddedLink>, ICollection<IEmbeddedRepresentation>> ParseEntities(JObject obj)
            {
                // entities
                // A collection of related sub-entities.
                // If a sub-entity contains an href value, it should be treated as an embedded link.
                // Clients may choose to optimistically load embedded links.
                // If no href value exists, the sub-entity is an embedded entity representation that contains all the characteristics of a typical entity.
                // One difference is that a sub-entity MUST contain a rel attribute to describe its relationship to the parent entity.
                // In JSON Siren, this is represented as an array. Optional.

                ICollection<IEmbeddedLink> ELinks = new List<IEmbeddedLink>();
                ICollection<IEmbeddedRepresentation> EReps = new List<IEmbeddedRepresentation>();
                var entities = obj["entities"];

                if (entities != null)
                {
                    if (entities is JArray)
                    {
                        var entityArray = entities as JArray;

                        foreach (var tok in entityArray)
                        {
                            var tokObj = tok as JObject;

                            if (tokObj == null)
                            {
                                throw new FormatException("embedded entity is not a json object");
                            }

                            var href = tokObj["href"];
                            if (href != null)
                            {
                                // Is an embedded link
                                ELinks.Add(ParseEmbeddedLink(tokObj));
                            }
                            else
                            {
                                // Is an embedded representation
                                EReps.Add(ParseEmbeddedRepresentation(tokObj));
                            }
                        }
                    }
                    else
                    {
                        throw new FormatException("'entities' property is no array of object");
                    }
                }

                return Tuple.Create(ELinks, EReps);
            }

            private static IEmbeddedLink ParseEmbeddedLink(JObject obj)
            {
                var href = ParseHref(obj);

                var rels = ParseRel(obj);

                var classes = ParseClasses(obj);

                var title = ParseStringOptional(obj, "title", "An Embedded Links 'title' MUST be a string if it exists");

                var type = ParseMediaType(obj);

                return new EmbeddedLink(href, rels)
                {
                    Classes = classes,
                    Title = title,
                    Type = type
                };
            }

            private static IEmbeddedRepresentation ParseEmbeddedRepresentation(JObject obj)
            {
                var rels = ParseRel(obj);
                var actions = ParseActions(obj);
                var classes = ParseClasses(obj);
                var embedded = ParseEntities(obj);
                var links = ParseLinks(obj);
                var properties = ParseProperties(obj);
                var title = ParseStringOptional(obj, "title", "An Entities 'title' must be a string, if it exists");

                return new EmbeddedRepresentation(rels)
                {
                    Actions = actions,
                    Classes = classes,
                    EmbeddedLinks = embedded.Item1,
                    EmbeddedRepresentations = embedded.Item2,
                    Links = links,
                    Properties = properties,
                    Title = title,
                };
            }

            public static ICollection<Link> ParseLinks(JObject obj)
            {
                // links
                // A collection of items that describe navigational links, distinct from entity relationships.
                // Link items should contain a rel attribute to describe the relationship and an href attribute to point to the target URI.
                // Entities should include a link rel to self.
                // In JSON Siren, this is represented as "links": [{ "rel": ["self"], "href": "http://api.x.io/orders/1234" }] Optional.
                var result = new List<Link>();

                var links = obj["links"];
                if (links != null)
                {
                    var linksArray = links as JArray;

                    if (linksArray != null)
                    {
                        foreach (var tok in linksArray)
                        {
                            result.Add(ParseLink(tok));
                        }
                    }
                    else
                    {
                        throw new FormatException("'links' MUST be an array, if it exists");
                    }
                }

                return result;
            }

            public static ICollection<Action> ParseActions(JObject obj)
            {
                // fields
                // A collection of field objects, represented in JSON Siren as an array such as { "fields": [{ ... }] }.
                // See Actions. Optional
                var result = new List<Action>();

                var actions = obj["actions"];
                if (actions != null)
                {
                    var actionsArray = actions as JArray;

                    if (actionsArray != null)
                    {
                        foreach (var tok in actionsArray)
                        {
                            result.Add(ParseAction(tok));
                        }
                    }
                    else
                    {
                        throw new FormatException("'actions' MUST be an array, if it exists");
                    }
                }

                return result;
            }

            public static ICollection<Field> ParseFields(JObject action)
            {
                var result = new List<Field>();

                var fields = action["fields"];
                if (fields != null)
                {
                    var actionsArray = fields as JArray;

                    if (actionsArray != null)
                    {
                        foreach (var tok in actionsArray)
                        {
                            result.Add(ParseField(tok));
                        }
                    }
                    else
                    {
                        throw new FormatException("'fields' MUST be an array, if it exists");
                    }
                }

                return result;
            }

            public static Link ParseLink(JToken token)
            {
                var obj = token as JObject;

                if (obj == null)
                {
                    throw new FormatException("A Link must be a json object");
                }

                var rel = ParseRel(obj);

                var href = ParseHref(obj);

                var title = ParseStringOptional(obj, "title", "A Links 'title' must be a string if it exists");

                var type = ParseMediaType(obj);

                return new Link(href, rel)
                {
                    Type = type,
                    Title = title
                };
            }

            public static Field ParseField(JToken token)
            {
                // Fields
                // Fields represent controls inside of fields. They may contain these attributes:
                // name
                // A name describing the control. Required.
                // type
                // The input type of the field. This may include any of the following input types specified in HTML5:
                // hidden, text, search, tel, url, email, password, datetime, date, month, week, time, datetime-local, number, range, color, checkbox, radio, file, image, button
                // When missing, the default value is text.
                // Serialization of these fields will depend on the value of the field's type attribute.
                // See type under Actions, above. Optional.
                // value
                // A value assigned to the field. Optional.
                // title
                // Textual annotation of a field. Clients may use this as a label. Optional.

                var obj = token as JObject;

                if (obj == null)
                {
                    throw new FormatException("A Field must be a json object");
                }

                var name = ParseStringRequired(obj, "name", "A Field MUST have a 'name' string");

                var typeString = ParseStringOptional(obj, "type", "An Fields 'type' MUST be a string if it exists");

                FieldType type;

                if (!Enum.TryParse(typeString ?? "text", true, out type))
                {
                    throw new FormatException(string.Format("'{0}' is not a supported Field 'type' value", typeString));
                }

                var title = ParseStringOptional(obj, "title", "A Fields 'title' MUST be a string, if it exists");

                var value = ParseStringOptional(obj, "value", "An Fields 'value' MUST be a string, if it exists");

                return new Field(name)
                {
                    Type = type,
                    Value = value
                };
            }

            private static Action ParseAction(JToken token)
            {
                // Actions
                // show available behaviors an entity exposes.
                // name
                // A string that identifies the field to be performed. Required.
                // class
                // Describes the nature of an field based on the current representation.
                // Possible values are implementation-dependent and should be documented.
                // MUST be an array of strings. Optional.
                // method
                // An enumerated attribute mapping to a protocol method.
                // For HTTP, these values may be GET, PUT, POST, DELETE, or PATCH.
                // As new methods are introduced, this list can be extended.
                // If this attribute is omitted, GET should be assumed. Optional.
                // href
                // The URI of the field. Required.
                // title
                // Descriptive text about the field. Optional.
                // type
                // The encoding type for the request.
                // When omitted and the fields attribute exists, the default value is application/x-www-form-urlencoded.
                // Optional.
                // fields
                // A collection of fields, expressed as an array of objects in JSON Siren such as { "fields" : [{ ... }] }.
                // See Fields. Optional.
                var obj = token as JObject;

                if (obj == null)
                {
                    throw new FormatException("An Action must be a json object");
                }

                var name = ParseStringRequired(obj, "name", "An Action MUST have a 'name' string");

                var classes = ParseClasses(obj);

                var methodString = ParseStringOptional(obj, "method", "An Actions 'method' MUST be a string, if it exists");

                var href = ParseHref(obj);

                var title = ParseStringOptional(obj, "title", "An Actions 'title' MUST be a string, if it exists");

                var type = ParseMediaType(obj);

                var fields = ParseFields(obj);

                if (type == null && fields.Any())
                {
                    // Default type if we have any field definitions
                    type = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                }

                var method = new HttpMethod(methodString ?? "GET");

                return new Action(name, href)
                {
                    Title = title,
                    Type = type,
                    Method = method,
                    Fields = fields
                };
            }

            private static string ParseHref(JObject obj)
            {
                var href = ParseStringRequired(obj, "href", "'href' must be an URI string");

                return href;
            }

            private static MediaTypeHeaderValue ParseMediaType(JObject obj)
            {
                var typeString = ParseStringOptional(obj, "type", "A 'type' value MUST be a string if it exists");

                if (typeString != null)
                {
                    return new MediaTypeHeaderValue(typeString);
                }

                return null;
            }

            private static ICollection<string> ParseRel(JObject obj)
            {
                ICollection<string> rels = null;

                var relArray = obj["rel"] as JArray;
                if (relArray != null)
                {
                    rels = new List<string>();
                    foreach (var item in relArray)
                    {
                        try
                        {
                            rels.Add(item.Value<string>());
                        }
                        catch (InvalidCastException)
                        {
                            // Conversion to string failed
                            throw new FormatException("'rel' must be an array of string");
                        }
                    }
                }

                if (rels == null)
                {
                    throw new FormatException("rel is required and MUST be an array of string");
                }

                return rels;
            }

            private static string ParseStringRequired(JObject obj, string field, string failureMessage)
            {
                var token = obj[field];

                if (token == null)
                {
                    throw new FormatException(failureMessage);
                }

                try
                {
                    return token.Value<string>();
                }
                catch (InvalidCastException ex)
                {
                    throw new FormatException(failureMessage, ex);
                }
            }

            private static string ParseStringOptional(JObject obj, string field, string failureMessage)
            {
                var token = obj[field];

                if (token == null)
                {
                    return null;
                }

                try
                {
                    return token.Value<string>();
                }
                catch (InvalidCastException ex)
                {
                    throw new FormatException(failureMessage, ex);
                }
            }
        }
    }
}