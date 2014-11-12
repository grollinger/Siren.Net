namespace Siren.Net.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using WebApiContrib.Formatting.Siren.Client;
    using Xunit;

    public class FormatterTest
    {
        private readonly SirenJsonMediaTypeFormatter Formatter;

        public FormatterTest()
        {
            Formatter = new SirenJsonMediaTypeFormatter();
        }

        [Fact]
        public void Supports_Siren_Types()
        {
            // Arrange

            // Act

            // Assert
            Assert.True(Formatter.CanReadType(typeof(SirenEntity)));
            Assert.True(Formatter.CanWriteType(typeof(SirenEntity)));
            Assert.True(Formatter.CanReadType(typeof(ISirenEntity)));
            Assert.True(Formatter.CanWriteType(typeof(ISirenEntity)));
        }

        [Fact]
        public void Supports_Siren_MediaTypes()
        {
            // Arrange

            // Act

            // Assert
            Assert.True(Formatter.SupportedMediaTypes.Any(x => x.MediaType == "application/vnd.siren+json"));
        }

        [Fact]
        public async Task Can_Read_the_spec_sample()
        {
            // Arrange
            var sample = TestHelper.SPEC_SAMPLE_JSON;
            var sampleContent = new StringContent(sample);

            // Act
            var doc = await Formatter.ReadFromStreamAsync(typeof(ISirenEntity), await sampleContent.ReadAsStreamAsync(), sampleContent, null);

            // Assert
            var sirenDoc = Assert.IsAssignableFrom<ISirenEntity>(doc);
            TestHelper.AssertIsSpecSample(sirenDoc);
        }

        [Fact]
        public async Task Can_Write_the_spec_sample()
        {
            // Arrange
            var sample = TestHelper.SPEC_SAMPLE_JSON;
            var sampleDoc = SirenJson.Parse(sample);
            var content = new StringContent("42");          
            var contentStream = new MemoryStream();
            var nonDisposeContentStream = new NonDisposableStream(contentStream);

            // Act
            await Formatter.WriteToStreamAsync(typeof(ISirenEntity), sampleDoc, nonDisposeContentStream, content, null);
            contentStream.Seek(0, SeekOrigin.Begin);
            string contentString;
            using (var rdr = new StreamReader(contentStream))
            {
                contentString = rdr.ReadToEnd();
            }
            var restoredSample = SirenJson.Parse(contentString);

            // Assert
            TestHelper.AssertIsSpecSample(restoredSample);
        }
    }
}
