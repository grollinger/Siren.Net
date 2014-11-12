namespace WebApiContrib.Formatting.Siren.Client
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class SirenJsonMediaTypeFormatter : MediaTypeFormatter
    {
        /// <inheritdoc cref="MediaTypeFormatter.CanReadType(Type)"/>
        public override bool CanReadType(Type type)
        {
            return IsSirenDocumentType(type);
        }

        /// <inheritdoc cref="MediaTypeFormatter.CanWriteType(Type)"/>
        public override bool CanWriteType(Type type)
        {
            return IsSirenDocumentType(type);
        }

        /// <inheritdoc cref="MediaTypeFormatter.ReadFromStreamAsync(Type, Stream, HttpContent, IFormatterLogger)"/>
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return this.ReadFromStreamAsync(type, readStream, content, formatterLogger, CancellationToken.None);
        }

        /// <inheritdoc cref="MediaTypeFormatter.ReadFromStreamAsync(Type, Stream, HttpContent, IFormatterLogger, CancellationToken)"/>
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger, CancellationToken cancellationToken)
        {
            Contract.Requires<ArgumentException>(IsSirenDocumentType(type), "type");

            using (var txtReader = new StreamReader(readStream))
            using (var jsonReader = new JsonTextReader(txtReader))
            {
                var sirenJson = JObject.ReadFrom(jsonReader);

                var sirenDoc = SirenJson.Parse(sirenJson);

                return Task.FromResult<object>(sirenDoc);
            }

            return Task.FromResult<object>(null);
        }

        /// <inheritdoc cref="MediaTypeFormatter.WriteToStreamAsync(Type, object, Stream, HttpContent, TransportContext)"/>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return this.WriteToStreamAsync(type, value, writeStream, content, transportContext, CancellationToken.None);
        }

        /// <inheritdoc cref="MediaTypeFormatter.WriteToStreamAsync(Type, object, Stream, HttpContent, TransportContext, CancellationToken)"/>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext, CancellationToken cancellationToken)
        {
            Contract.Requires<ArgumentException>(IsSirenDocumentType(type), "type");
            Contract.Requires<ArgumentException>(value is ISirenEntity);

            var sirenDocument = value as ISirenEntity;

            var jsonDocument = SirenJson.Unparse(sirenDocument);

            using(var writer = new StreamWriter(writeStream, this.SelectCharacterEncoding(content.Headers)))
            using(var jsonWriter = new JsonTextWriter(writer))
            {
                jsonDocument.WriteTo(jsonWriter);

                jsonWriter.Flush();
                writer.Flush();
            }

            return Task.FromResult<object>(null);
        }

        public bool IsSirenDocumentType(Type type)
        {
            return type == typeof(ISirenEntity) ||
                type == typeof(SirenEntity);
        }

        public SirenJsonMediaTypeFormatter()
        {
            var sirenMediaType = new MediaTypeHeaderValue("application/vnd.siren+json");
            this.SupportedMediaTypes.Add(sirenMediaType);
            this.SupportedEncodings.Add(Encoding.UTF8);
        }
    }
}
