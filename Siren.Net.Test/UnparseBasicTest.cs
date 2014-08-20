namespace Siren.Net.Test
{
    using WebApiContrib.Formatting.Siren.Client;
    using Xunit;

    public class UnparseBasicTest
    {
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
    }
}
