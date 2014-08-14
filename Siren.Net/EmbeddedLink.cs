namespace WebApiContrib.Formatting.Siren.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http.Headers;


    public class EmbeddedLink : Link, IEmbeddedLink
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>optional</remarks>
        public ICollection<string> Classes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EmbeddedLink(
            string href,
            ICollection<string> rels
            ) : base(href, rels)
        {            
        }
    }
}
