namespace WebApiContrib.Formatting.Siren.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>required</remarks>
        public string Name { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>required</remarks>
        public string Href { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>optional</remarks>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>optional</remarks>
        public HttpMethod Method { get; set; }  
      
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>optional</remarks>
        public MediaTypeHeaderValue Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>optional</remarks>
        public ICollection<Field> Fields { get; set; }

        public Action(
            string name,
            string href
            )
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name), "name must be non-empty");
            ValidationHelper.ValidateHref(href);

            Name = name;
            Href = href;

            Fields = new List<Field>();
        }
    }
}
