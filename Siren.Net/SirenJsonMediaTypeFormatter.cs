using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace WebApiContrib.Formatting.Siren.Client
{
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
