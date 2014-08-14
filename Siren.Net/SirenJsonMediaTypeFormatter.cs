namespace WebApiContrib.Formatting.Siren.Client
{
    using System;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;

    public class SirenJsonMediaTypeFormatter : MediaTypeFormatter
    {
        public override bool CanReadType(Type type)
        {
            throw new NotImplementedException();
        }

        public override bool CanWriteType(Type type)
        {
            throw new NotImplementedException();
        }

        public SirenJsonMediaTypeFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.siren+json"));
        }
    }
}
