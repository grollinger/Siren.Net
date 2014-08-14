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
                .Single(x => x.IsLink())
                .Select(x => x.AsLink());
            Assert.Equal("http://api.x.io/orders/42/items", link.Href.ToString());
            Assert.Equal("http://x.io/rels/order-items", link.Rel.ToString());            
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
                .Single(x => x.IsRepresentation())
                .Select(x => x.AsRepresentation());
            Assert.Equal("http://x.io/rels/customer", entity.Rel);
            Assert.False(true, "Add more asserts");
        }

        [Fact]
        public void Throws_for_invalid_entities()
        {
            // Arrange
            var anObject = JObject.Parse(
                @"{
                    entities: { { href: ""http://api.x.io/orders/42/items"" } }
                }");

            var aString =  JObject.Parse(
                @"{
                    entities: ""person""
                }");

            var aNumber =  JObject.Parse(
                @"{
                    entities: 42
                }");

            var noLinkHref = JObject.Parse(
                @"{                    
                    entities: [
                        {
                            class: [ ""items"", ""collection"" ], 
                            rel: [ ""http://x.io/rels/order-items"" ]                            
                        }
                    ]
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
            Assert.Throws<FormatException>(() => SirenJson.ParseEntities(anObject));
            Assert.Throws<FormatException>(() => SirenJson.ParseEntities(aString));
            Assert.Throws<FormatException>(() => SirenJson.ParseEntities(aNumber));
            Assert.Throws<FormatException>(() => SirenJson.ParseEntities(noLinkHref));
            Assert.Throws<FormatException>(() => SirenJson.ParseEntities(noRel));
        }

    }
}
