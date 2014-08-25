namespace Siren.Net.Test
{
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using WebApiContrib.Formatting.Siren.Client;
    using Xunit;
    using System.Net.Http.Headers;
    using System.Net.Http;
    using System.Collections.Generic;

    public class UnparseBasicTest
    {
        [Fact]
        public void Unparsing_the_empty_entity()
        {
            // Arrange
            var emptyDoc = new SirenEntity();

            // Act
            var jobj = SirenJson.Unparse(emptyDoc);          

            // Assert
            Assert.NotNull(jobj);
            Assert.Empty(jobj);
        }

        [Fact]
        public void Unparsing_an_entity_title()
        {
            // Arrange
            var doc = new SirenEntity();
            doc.Title = "testtitle";

            // Act
            var jobj = SirenJson.Unparse(doc);

            // Assert
            Assert.NotNull(jobj);
            Assert.Equal("testtitle", jobj[SirenJson.TITLE]);
        }

        [Fact]
        public void Unparse_only_classes()
        {
            // Arrange
            var doc = new SirenEntity();
            doc.Classes.Add("person");
            doc.Classes.Add("programmer");

            // Act
            var jobj = SirenJson.Unparse(doc);

            // Assert
            Assert.NotNull(jobj);
            var classes = Assert.IsAssignableFrom<JArray>(jobj[SirenJson.CLASSES]);
            Assert.Equal(2, classes.Count);
            Assert.Contains("person", classes);
            Assert.Contains("programmer", classes);
        }

        [Fact]
        public void Unparsing_only_properties()
        {
            // Arrange
            var doc = new SirenEntity();
            doc.Properties.Add("prop1", "value1");
            doc.Properties.Add("prop_arr", new[] { 42 });                

            // Act
            var jobj = SirenJson.Unparse(doc);            

            // Assert
            Assert.NotNull(jobj);
            var properties = jobj[SirenJson.PROPERTIES];
            Assert.NotNull(properties);
            Assert.Equal("value1", properties["prop1"]);
            Assert.IsAssignableFrom<JArray>(properties["prop_arr"]);
            Assert.Equal(42, properties["prop_arr"][0]);
            Assert.Null(jobj[SirenJson.LINKS]);
            Assert.Null(jobj[SirenJson.ENTITIES]);
            Assert.Null(jobj[SirenJson.CLASSES]);
            Assert.Null(jobj[SirenJson.TITLE]);
        }

        [Fact]
        public void Unparsing_only_links()
        {
            // Arrange
            var doc = new SirenEntity();
            doc.Links.Add(new Link("https://localhost/test", new[] { "self", "previous" }));
            doc.Links.Add(new Link("https://localhost/test2", new[] { "next" }) { Title = "testtitle", Type = new MediaTypeHeaderValue("application/json") });

            // Act
            var jobj = SirenJson.Unparse(doc);

            // Assert
            Assert.NotNull(jobj);
            var links = Assert.IsAssignableFrom<JArray>(jobj[SirenJson.LINKS]);

            var link0 = Assert.IsAssignableFrom<JObject>(links.Single(x => x[SirenJson.HREF].ToString() == "https://localhost/test"));
            var link0rels = Assert.IsAssignableFrom<JArray>(link0[SirenJson.RELS]);
            Assert.Contains("self", link0rels);
            Assert.Contains("previous", link0rels);

            var link1 = Assert.IsAssignableFrom<JObject>(links.Single(x => x[SirenJson.HREF].ToString() == "https://localhost/test2"));
            var link1rels = Assert.IsAssignableFrom<JArray>(link1[SirenJson.RELS]);
            Assert.Equal("next", link1rels.Single());
            Assert.Equal("testtitle", link1[SirenJson.TITLE]);
            Assert.Equal("application/json", link1[SirenJson.TYPE]);
        }

        [Fact]
        public void Unparsing_only_entities()
        {
            // Arrange
            var doc = new SirenEntity();
            doc.EmbeddedLinks.Add(new EmbeddedLink("https://localhost/testchild", new[] { "child" }));
            doc.EmbeddedRepresentations.Add(
                new EmbeddedRepresentation(new[] { "child2" })
                {
                    EmbeddedRepresentations = new[]
                    {
                        new EmbeddedRepresentation(new []{ "grandchild" })
                    }
                }
            );

            // Act
            var jobj = SirenJson.Unparse(doc);

            // Assert
            Assert.NotNull(jobj);
            var entities = Assert.IsAssignableFrom<JArray>(jobj[SirenJson.ENTITIES]);

            var link0 = Assert.IsAssignableFrom<JObject>(entities.Single(x => (x[SirenJson.HREF] ?? "").ToString() == "https://localhost/testchild"));
            var link0rels = Assert.IsAssignableFrom<JArray>(link0[SirenJson.RELS]);
            Assert.Equal("child", link0rels.Single());

            var rep1 = Assert.IsAssignableFrom<JObject>(entities.Single(x => x[SirenJson.HREF] == null));
            var rep1rels = Assert.IsAssignableFrom<JArray>(rep1[SirenJson.RELS]);
            Assert.Equal("child2", rep1rels.Single());

            var rep2 = Assert.IsAssignableFrom<JObject>(rep1[SirenJson.ENTITIES][0]);
            var rep2rels = Assert.IsAssignableFrom<JArray>(rep2[SirenJson.RELS]);
            Assert.Equal("grandchild", rep2rels.Single());
        }

        [Fact]
        public void Unparsing_only_actions()
        {
            // Arrange
            var doc = new SirenEntity();
            doc.Actions.Add(
                new Action("action1", "https://localhost/testaction")
                {
                    Classes = new[] { "actionclass" },
                    Fields = new[]{
                        new Field("field1")
                        {
                            Type = FieldType.Button,
                            Value = "value"
                        }
                    },
                    Method = new HttpMethod("TEST"),
                    Title = "actiontitle",
                    Type = new MediaTypeHeaderValue("application/json")
                }
            );
            doc.Actions.Add(
                new Action("action2", "https://localhost/testaction2")
            );

            // Act
            var jobj = SirenJson.Unparse(doc);

            // Assert
            Assert.NotNull(jobj);
            var actions = Assert.IsAssignableFrom<JArray>(jobj[SirenJson.ACTIONS]);

            var action1 = Assert.IsAssignableFrom<JObject>(actions.Single(x => x[SirenJson.NAME].ToString() == "action1"));
            Assert.Equal("https://localhost/testaction", action1[SirenJson.HREF]);
            var action1classes = Assert.IsAssignableFrom<JArray>(action1[SirenJson.CLASSES]);
            Assert.Equal("actionclass", action1classes.Single());
            Assert.Equal("TEST", action1[SirenJson.METHOD]);
            Assert.Equal("actiontitle", action1[SirenJson.TITLE]);
            Assert.Equal("application/json", action1[SirenJson.TYPE]);
            var fields = Assert.IsAssignableFrom<JArray>(action1[SirenJson.FIELDS]);
            var field = Assert.IsAssignableFrom<JObject>(fields[0]);
            Assert.Equal("field1", field[SirenJson.NAME]);
            Assert.Equal("button", field[SirenJson.TYPE]);
            Assert.Equal("value", field[SirenJson.VALUE]);

            var action2 = Assert.IsAssignableFrom<JObject>(actions.Single(x => x[SirenJson.NAME].ToString() == "action2"));
            Assert.Equal("https://localhost/testaction2", action2[SirenJson.HREF]);            
            Assert.Null(action2[SirenJson.METHOD]);
            Assert.Null(action2[SirenJson.CLASSES]);
            Assert.Null(action2[SirenJson.TITLE]);
            Assert.Null(action2[SirenJson.TYPE]);
            Assert.Null(action2[SirenJson.FIELDS]);            
        }

        [Fact]
        public void Roundtripping_the_spec_sample_works()
        {
            // Arrange
            var jsonString = TestHelper.SPEC_SAMPLE_JSON;

            // Act
            var doc = SirenJson.Parse(jsonString);
            var docString = SirenJson.Unparse(doc);
            var doc2 = SirenJson.Parse(docString);

            // Assert
            TestHelper.AssertIsSpecSample(doc2);
        }

        class InvalidEmbedded : IEmbeddedEntity
        {
            public ICollection<string> Rel
            {
                get;
                set;
            }
        }
    }
}
