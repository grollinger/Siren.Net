namespace Siren.Net.Test
{
    using Xunit;
    using System;
    using WebApiContrib.Formatting.Siren.Client;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class ParseLinksTest
    {
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
                        { rel: [""self""], href: ""http://localhost:10/test""},
                        { rel: [""parent""], href: ""http://localhost:10/testdad""}
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
        public void Throws_for_invalid_Links()
        {
            // Arrange
            var anObject = JObject.Parse(
                @"{
                    links: { test: { href: ""http://api.x.io/orders/42/items"" } }
                }");

            var aString = JObject.Parse(
                @"{
                    links: ""person""
                }");

            var aNumber = JObject.Parse(
                @"{
                    links: 42
                }");

            var noRel = JObject.Parse(
                @"{                    
                    links: [
                        { href: ""http://api.x.io/customers/pj123"" }
                    ]
                }");

            var noHref = JObject.Parse(
                @"{                    
                    links: [
                        { rel: [ ""self"" ] }
                    ]
                }");

            // Act            

            // Assert
            Assert.Throws<FormatException>(() => SirenJson.ParseLinks(anObject));
            Assert.Throws<FormatException>(() => SirenJson.ParseLinks(aString));
            Assert.Throws<FormatException>(() => SirenJson.ParseLinks(aNumber));
            Assert.Throws<FormatException>(() => SirenJson.ParseLinks(noRel));
            Assert.Throws<FormatException>(() => SirenJson.ParseLinks(noHref));
        }
    }
}
