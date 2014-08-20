namespace Siren.Net.Test
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebApiContrib.Formatting.Siren.Client;
    using Xunit;

    public class ParseClassesTest
    {
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
        public void Throws_for_invalid_classes()
        {
            // Arrange
            var anObject = JObject.Parse(
                @"{
                    class: { person: ""yes"" }
                }");

            var aString =  JObject.Parse(
                @"{
                    class: ""person""
                }");

            var aNumber =  JObject.Parse(
                @"{
                    class: 42
                }");

            // Act            

            // Assert
            Assert.Throws<FormatException>(() => SirenJson.Parser.ParseClasses(anObject));
            Assert.Throws<FormatException>(() => SirenJson.Parser.ParseClasses(aString));
            Assert.Throws<FormatException>(() => SirenJson.Parser.ParseClasses(aNumber));
        }

    }
}
