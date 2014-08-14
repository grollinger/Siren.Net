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

    public class ParseActionsTest
    {
        [Fact]
        public void Parses_with_actions()
        {
            // Arrange
            var jsonString =
                @"{                    
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
            var action = doc.Actions.Single();
            Assert.Equal(HttpMethod.Post, action.Method);
        }

        [Fact]
        public void Parses_minimal_actions()
        {
            // Arrange
            var jsonString =
                @"{                    
                    actions: [
                        {  
                            name: ""get-items"",                          
                            href: ""http://api.x.io/orders/42/items""                            
                        }
                    ]
                }";

            // Act
            var doc = SirenJson.Parse(jsonString);

            // Assert
            Assert.NotNull(doc);
            var action = doc.Actions.Single();
            Assert.Equal(HttpMethod.Get, action.Method);
            Assert.NotNull(action.Fields);
        }

        [Fact]
        public void Honors_default_media_type_with_fields()
        {
            // Arrange
            var jsonString =
                @"{                    
                    actions: [
                        {  
                            name: ""default-test"",                          
                            href: ""http://api.x.io/orders/42/items"",
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
            var action = doc.Actions.Single();
            Assert.Equal("application/x-www-form-urlencoded", action.Type.MediaType);
        }

        [Fact]
        public void Throws_for_invalid_actions()
        {
            // Arrange
            var anObject = JObject.Parse(
                @"{
                    actions: { test: { href: ""http://api.x.io/orders/42/items"" } }
                }");

            var aString = JObject.Parse(
                @"{
                    actions: ""person""
                }");

            var aNumber = JObject.Parse(
                @"{
                    actions: 42
                }");

            var noHref = JObject.Parse(
                @"{                    
                    actions: [
                        {
                            name: ""add-item"",
                            title: ""Add Item"",
                            method: ""POST"",
                            type: ""application/x-www-form-urlencoded"",
                            fields: [
                                { name: ""orderNumber"", type: ""hidden"", value: ""42"" },
                                { name: ""productCode"", type: ""text"" },
                                { name: ""quantity"", type: ""number"" }
                            ]
                        }
                    ]
                }");

            // Act            

            // Assert
            Assert.Throws<FormatException>(() => SirenJson.ParseActions(anObject));
            Assert.Throws<FormatException>(() => SirenJson.ParseActions(aString));
            Assert.Throws<FormatException>(() => SirenJson.ParseActions(aNumber));
            Assert.Throws<FormatException>(() => SirenJson.ParseActions(noHref));
        }

        [Fact]
        public void Throws_for_invalid_Fields()
        {
            // Arrange
            var invalidFieldType = JObject.Parse(
                @"{ name: ""orderNumber"", type: ""hidden"", value: ""42"" }"
                );

            var anotherObject = JObject.Parse(
                @"{ 0:{ name: ""orderNumber"", type: ""hidden"", value: ""42"" }}"
                );

            var aString = JToken.Parse(
                @"""test"""
                );

            // Act

            // Assert
            Assert.Throws<FormatException>(() => SirenJson.ParseField(invalidFieldType));
            Assert.Throws<FormatException>(() => SirenJson.ParseField(anotherObject));
            Assert.Throws<FormatException>(() => SirenJson.ParseField(aString));
        }

    }
}
