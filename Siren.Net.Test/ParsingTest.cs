namespace Siren.Net.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebApiContrib.Formatting.Siren.Client;
    using Xunit;

    public class ParsingTest
    {
        public ParsingTest()
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
        public void Parses_a_naked_entity()
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
            var doc = SirenJson.Parse(jsonString);

            // Assert
            Assert.NotNull(doc);
            Assert.Equal(doc.name, "test");
            Assert.Equal(doc.age, 42);
        }

        [Fact]
        public void Parses_with_classes()
        {
            // Arrange
            var jsonString =
                @"{
                    class: [""person""],
                    properties: {
                        name: ""test"",
                        age: 42
                    }  
                }";

            // Act
            var doc = SirenJson.Parse(jsonString);

            // Assert
            Assert.NotNull(doc);
            Assert.Contains<string>("person", doc.Classes);
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
            Assert.Equal(2, doc.Links.Length);
            Assert.True(Enumerable.Any(doc.Links, (Func<Link, bool>)((Link x) => x.Rel == "self")));
            Assert.True(Enumerable.Any(doc.Links, (Func<Link, bool>)((Link x) => x.Rel == "parent")));            
        }

        [Fact]
        public void Parses_a_derived_type()
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
            var doc = SirenJson.Parse<TestPerson>(jsonString);

            // Assert
            Assert.NotNull(doc);
            Assert.Equal(doc.Name, "test");
            Assert.Equal(doc.Age, 42);
            Assert.Equal(2, doc.Links.Count());
            Assert.True(Enumerable.Any(doc.Links, (Func<Link, bool>)((Link x) => x.Rel == "self")));
            Assert.True(Enumerable.Any(doc.Links, (Func<Link, bool>)((Link x) => x.Rel == "parent")));
        }

        [Fact]
        public void Parses_with_actions()
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
            var doc = SirenJson.Parse<TestPerson>(jsonString);

            // Assert
            Assert.NotNull(doc);
        }
    }
}
