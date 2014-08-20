namespace Siren.Net.Test
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using WebApiContrib.Formatting.Siren.Client;
    using Xunit;

    public class ParseEntitiesTest
    {
        [Fact]
        public void Parses_with_embedded_entities()
        {
            // Arrange
            var jsonString =
                @"{
                    class: [""person""],
                    properties: {
                        name: ""test"",
                        age: 42
                    },
                    links: [
                        { rel: [""self""], href: ""http://localhost:10/test""},
                        { rel: [""parent""], href: ""http://localhost:10/testdad""}
                    ],
                    entities: [
                        { 
                            class: [ ""items"", ""collection"" ], 
                            rel: [ ""http://x.io/rels/order-items"" ], 
                            href: ""http://api.x.io/orders/42/items""
                        },
                        {
                            class: [ ""info"", ""customer"" ],
                            rel: [ ""http://x.io/rels/customer"" ], 
                            properties: { 
                                customerId: ""pj123"",
                                name: ""Peter Joseph""
                            },
                            links: [
                                { rel: [ ""self"" ], href: ""http://api.x.io/customers/pj123"" }
                            ]
                        }
                    ],
                    actions: [
                        {
                            name: ""add-item"",
                            title: ""Add Item"",
                            method: ""POST"",
                            href: ""http://api.x.io/orders/42/items"",
                            type: ""application/x-www-form-urlencoded"",
                            fields: [
                                { name: ""orderNumber"", type: ""hidden"", value: ""42"" },
                                { name: ""productCode"", type: ""text"" },
                                { name: ""quantity"", type: ""number"" }
                            ]
                        }
                    ]
                }";

            // Act
            var doc = SirenJson.Parse(jsonString);

            // Assert
            Assert.NotNull(doc);
            Assert.Equal(2, doc.Entities.Count());
        }

        [Fact]
        public void Parses_with_embedded_links()
        {
            // Arrange
            var jsonString = 
                @"{                    
                    entities: [
                        { 
                            class: [ ""items"", ""collection"" ], 
                            rel: [ ""http://x.io/rels/order-items"" ], 
                            href: ""http://api.x.io/orders/42/items""
                        }
                    ]
                }";

            // Act
            var doc = SirenJson.Parse(jsonString);

            // Assert
            Assert.NotNull(doc);
            Assert.Equal(1, doc.Entities.Count);
            IEmbeddedLink link = doc.Entities
                .Select(x => x.AsLink())
                .Single(x => x.IsLink());
            Assert.Equal("http://api.x.io/orders/42/items", link.Href.ToString());
            Assert.Contains("http://x.io/rels/order-items", link.Rel);            
        }

        [Fact]
        public void Parses_with_embedded_representations()
        {
            // Arrange
            var jsonString =
                @"{                    
                    entities: [
                        {
                            class: [ ""info"", ""customer"" ],
                            rel: [ ""http://x.io/rels/customer"" ], 
                            properties: { 
                                customerId: ""pj123"",
                                name: ""Peter Joseph""
                            },
                            links: [
                                { rel: [ ""self"" ], href: ""http://api.x.io/customers/pj123"" }
                            ]
                        }
                    ]
                }";

            // Act
            var doc = SirenJson.Parse(jsonString);

            // Assert
            Assert.NotNull(doc);
            IEmbeddedRepresentation entity = doc.Entities
                .Select(x => x.AsRepresentation())
                .Single(x => x.IsRepresentation());
            Assert.Equal("http://x.io/rels/customer", entity.Rel.Single());
            Assert.Equal(2, entity.Classes.Count);
            Assert.Equal(1, entity.Links.Count);
        }

        [Fact]
        public void Throws_for_invalid_entities()
        {
            // Arrange
            var anObject = JObject.Parse(
                @"{
                    entities: { test: { href: ""http://api.x.io/orders/42/items"" } }
                }");

            var aString =  JObject.Parse(
                @"{
                    entities: ""person""
                }");

            var aNumber =  JObject.Parse(
                @"{
                    entities: 42
                }");            

            var noRel = JObject.Parse(
                @"{                    
                    entities: [
                        {
                            class: [ ""info"", ""customer"" ],
                            properties: { 
                                customerId: ""pj123"",
                                name: ""Peter Joseph""
                            },
                            links: [
                                { rel: [ ""self"" ], href: ""http://api.x.io/customers/pj123"" }
                            ]
                        }
                    ]
                }");

            // Act            

            // Assert
            Assert.Throws<FormatException>(() => SirenJson.Parser.ParseEntities(anObject));
            Assert.Throws<FormatException>(() => SirenJson.Parser.ParseEntities(aString));
            Assert.Throws<FormatException>(() => SirenJson.Parser.ParseEntities(aNumber));
            Assert.Throws<FormatException>(() => SirenJson.Parser.ParseEntities(noRel));
        }

    }
}
