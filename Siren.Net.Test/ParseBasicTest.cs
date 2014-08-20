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
        public void Parses_specification_sample()
        {
            // Arrange
            var jsonString = TestHelper.SPEC_SAMPLE_JSON;

            // Act
            var doc = SirenJson.Parse(jsonString);

            // Assert
            TestHelper.AssertIsSpecSample(doc);
        }
    }
}
