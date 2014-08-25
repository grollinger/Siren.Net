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
        public static class Unparser
        {
            internal static JObject UnparseDocument(ISirenEntity entity)
            {
                var result = new JObject();

                UnparseClasses(result, entity.Classes);
                UnparseTitle(result, entity.Title);
                UnparseProperties(result, entity.Properties);
                UnparseActions(result, entity.Actions);
                UnparseLinks(result, entity.Links);
                UnparseEntities(result, entity.EmbeddedLinks, entity.EmbeddedRepresentations);

                return result;
            }

            private static void UnparseTitle(JObject obj, string title)
            {
                if (!string.IsNullOrEmpty(title))
                {
                    obj[TITLE] = title;
                }
            }

            public static void UnparseClasses(JObject obj, IEnumerable<string> Classes)
            {
                if (Classes != null && Classes.Any())
                {
                    obj[CLASSES] = JArray.FromObject(Classes);
                }
            }

            public static void UnparseProperties(JObject obj, IDictionary<string, object> Properties)
            {
                if (Properties != null && Properties.Any())
                {
                    var properties = new JObject();
                    foreach (var prop in Properties)
                    {
                        properties.Add(prop.Key, JToken.FromObject(prop.Value));
                    }

                    obj[PROPERTIES] = properties;
                }
            }

            public static void UnparseEntities(JObject obj, IEnumerable<IEmbeddedLink> ELinks, IEnumerable<IEmbeddedRepresentation> EReps)
            {
                var anyLinks = ELinks != null && ELinks.Any();
                var anyReps = EReps != null && EReps.Any();

                if (anyLinks || anyReps)
                {
                    var entities = new JArray();

                    if (anyReps)
                    {
                        foreach (var rep in EReps)
                        {
                            entities.Add(UnparseEmbeddedRepresentation(rep));
                        }
                    }

                    if (anyLinks)
                    {
                        foreach (var link in ELinks)
                        {
                            entities.Add(UnparseEmbeddedLink(link));
                        }
                    }

                    obj[ENTITIES] = entities;
                }
            }

            private static JObject UnparseEmbeddedLink(IEmbeddedLink Link)
            {
                var link = new JObject();

                link[HREF] = Link.Href;

                UnparseClasses(link, Link.Classes);

                UnparseRels(link, Link.Rel);

                UnparseTitle(link, Link.Title);

                if (Link.Type != null)
                {
                    link[TYPE] = Link.Type.ToString();
                }

                return link;
            }



            private static void UnparseRels(JObject link, IEnumerable<string> Rels)
            {
                if (Rels != null && Rels.Any())
                {
                    link[RELS] = JArray.FromObject(Rels);
                }
                else
                {
                    throw new ArgumentException(string.Format("An Entry for '{0}' is required but none was specified.", RELS));
                }
            }

            private static JObject UnparseEmbeddedRepresentation(IEmbeddedRepresentation Entity)
            {
                var entity = new JObject();

                UnparseClasses(entity, Entity.Classes);
                UnparseRels(entity, Entity.Rel);
                UnparseTitle(entity, Entity.Title);
                UnparseProperties(entity, Entity.Properties);
                UnparseActions(entity, Entity.Actions);
                UnparseLinks(entity, Entity.Links);
                UnparseEntities(entity, Entity.EmbeddedLinks, Entity.EmbeddedRepresentations);

                return entity;
            }


            public static void UnparseLinks(JObject obj, IEnumerable<Link> Links)
            {
                if (Links != null && Links.Any())
                {
                    var links = new JArray();
                    foreach (var l in Links)
                    {
                        var link = new JObject();

                        link[HREF] = l.Href;

                        UnparseRels(link, l.Rel);

                        UnparseTitle(link, l.Title);

                        UnparseType(link, l.Type);

                        links.Add(link);
                    }

                    obj[LINKS] = links;
                }
            }

            public static void UnparseActions(JObject obj, IEnumerable<Action> Actions)
            {
                if (Actions != null && Actions.Any())
                {
                    var actions = new JArray();

                    foreach (var action in Actions)
                    {
                        actions.Add(UnparseAction(action));
                    }

                    obj[ACTIONS] = actions;
                }
            }

            public static void UnparseFields(JObject action, IEnumerable<Field> Fields)
            {
                if (Fields != null && Fields.Any())
                {
                    var fields = new JArray();

                    foreach (var field in Fields)
                    {
                        fields.Add(UnparseField(field));
                    }

                    action[FIELDS] = fields;
                }
            }

            private static JToken UnparseField(Field f)
            {
                var field = new JObject();

                field[NAME] = f.Name;

                if (f.Type != FieldType.Text)
                {
                    field[TYPE] = f.Type.ToString().ToLowerInvariant();
                }

                if (f.Value != null)
                {
                    field[VALUE] = JValue.FromObject(f.Value);
                }

                return field;
            }


            private static JObject UnparseAction(Action a)
            {
                var action = new JObject();

                action[NAME] = a.Name;

                action[HREF] = a.Href;

                UnparseClasses(action, a.Classes);
                UnparseTitle(action, a.Title);
                UnparseType(action, a.Type);

                if (a.Method != null)
                {
                    action[METHOD] = a.Method.Method;
                }

                UnparseFields(action, a.Fields);

                return action;
            }

            private static void UnparseType(JObject obj, MediaTypeHeaderValue Type)
            {
                if (Type != null)
                {
                    obj[TYPE] = Type.ToString();
                }
            }
        }
    }
}
