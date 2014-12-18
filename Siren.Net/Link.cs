namespace WebApiContrib.Formatting.Siren.Client
{
    using System.Collections.Generic;
    using System.Net.Http.Headers;

    /// <summary>
    ///
    /// </summary>
    public class Link
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks>required</remarks>
        public string Href { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>required</remarks>
        public ICollection<string> Rel { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        public string Title { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        public MediaTypeHeaderValue Type { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="href"></param>
        /// <param name="rels"></param>
        public Link(
            string href,
            IEnumerable<string> rels
            )
        {
            ValidationHelper.ValidateHref(href);
            ValidationHelper.ValidateRel(rels);

            Href = href;
            Rel = new List<string>(rels);
        }
    }
}