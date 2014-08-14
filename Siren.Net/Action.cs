namespace WebApiContrib.Formatting.Siren.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class Action
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public HttpMethod Method { get; set; }
        public Uri Href { get; set; }
        public MediaTypeHeaderValue Type { get; set; }
        public ICollection<Field> Fields { get; set; }
    }
}
