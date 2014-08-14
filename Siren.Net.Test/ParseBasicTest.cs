namespace Siren.Net.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebApiContrib.Formatting.Siren.Client;
    using Xunit;

    public class ParseBasicTest
    {
        public ParseBasicTest()
        {

        }


        [Fact]
        public void Parses_the_empty_document()
        {
            // Arrange
            var jsonString = "{}";

            // Act
            var doc = SirenJson.Parse(jsonString);

            // Assert
            Assert.NotNull(doc);
            Assert.Empty(doc.Classes);
            Assert.Empty(doc.Actions);
            Assert.Empty(doc.Links);
            Assert.Empty(doc.Entities);
            Assert.Empty(doc.Properties);
        }        

        [Fact]
        public void Parses_into_an_object_supporting_dynamic()
        {
            // Arrange
            var jsonString =
                @"{
                    properties: {
                        name: ""test"",
                        age: 42
                    }  
                }";

            // Act
            dynamic doc = SirenJson.Parse(jsonString);

            // Assert
            Assert.NotNull(doc);
            Assert.Equal(doc.name, "test");
            Assert.Equal(doc.age, 42);
        }       

        [Fact]
        public void Parses_with_links()
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
                        { rel: ""self"", href: ""http://localhost:10/test""},
                        { rel: ""parent"", href: ""http://localhost:10/testdad""}
                    ] 
                }";

            // Act
            var doc = SirenJson.Parse(jsonString);

            // Assert
            Assert.NotNull(doc);
            Assert.Equal(2, Enumerable.Count(doc.Links));
            Assert.True(Enumerable.Any(doc.Links, (Func<Link, bool>)((Link x) => x.Rel.Contains("self"))));
            Assert.True(Enumerable.Any(doc.Links, (Func<Link, bool>)((Link x) => x.Rel.Contains("parent"))));
        }

        [Fact]
        public void Parses_specification_sample()
        {
            // Arrange
            var jsonString =
            @"{
                ""class"": [ ""order"" ],
                ""properties"": { 
                    ""orderNumber"": 42, 
                    ""itemCount"": 3,
                    ""status"": ""pending""
                },
                ""entities"": [
                    { 
                        ""class"": [ ""items"", ""collection"" ], 
                        ""rel"": [ ""http://x.io/rels/order-items"" ], 
                        ""href"": ""http://api.x.io/orders/42/items""
                    },
                    {
                        ""class"": [ ""info"", ""customer"" ],
                        ""rel"": [ ""http://x.io/rels/customer"" ], 
                        ""properties"": { 
                        ""customerId"": ""pj123"",
                        ""name"": ""Peter Joseph""
                        },
                        ""links"": [
                        { ""rel"": [ ""self"" ], ""href"": ""http://api.x.io/customers/pj123"" }
                        ]
                    }
                ],
                ""actions"": [
                    {
                        ""name"": ""add-item"",
                        ""title"": ""Add Item"",
                        ""method"": ""POST"",
                        ""href"": ""http://api.x.io/orders/42/items"",
                        ""type"": ""application/x-www-form-urlencoded"",
                        ""fields"": [
                            { ""name"": ""orderNumber"", ""type"": ""hidden"", ""value"": ""42"" },
                            { ""name"": ""productCode"", ""type"": ""text"" },
                            { ""name"": ""quantity"", ""type"": ""number"" }
                        ]
                    }
                ],
                ""links"": [
                    { ""rel"": [ ""self"" ], ""href"": ""http://api.x.io/orders/42"" },
                    { ""rel"": [ ""previous"" ], ""href"": ""http://api.x.io/orders/41"" },
                    { ""rel"": [ ""next"" ], ""href"": ""http://api.x.io/orders/43"" }
                ]
                }";

            // Act
            var doc = SirenJson.Parse(jsonString);

            // Assert
            Assert.NotNull(doc);
            Assert.Equal(3, doc.Properties.Count);
            Assert.Equal(2, doc.Entities.Count);
            Assert.Equal(1, doc.Classes.Count);
            Assert.Equal(1, doc.Actions.Count);
            Assert.Equal(3, doc.Links.Count);
        }
    }
}
