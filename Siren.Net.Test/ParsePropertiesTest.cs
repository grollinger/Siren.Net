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

    public class ParsePropertiesTest
    {
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
            Assert.Equal("test", doc.Properties["name"].ToString());
            Assert.Equal("42", doc.Properties["age"].ToString());
        }

        [Fact]
        public void Throws_for_invalid_properties()
        {
            // Arrange
            var anArray = JObject.Parse(
                @"{
                    properties: [""yes""]
                }");

            var aString = JObject.Parse(
                @"{
                    properties: ""person""
                }");

            var aNumber = JObject.Parse(
                @"{
                    properties: 42
                }");

            // Act   
           

            // Assert
            Assert.Throws<FormatException>(() => SirenJson.Parser.ParseProperties(anArray));
            Assert.Throws<FormatException>(() => SirenJson.Parser.ParseProperties(aString));
            Assert.Throws<FormatException>(() => SirenJson.Parser.ParseProperties(aNumber));
        }

    }
}
