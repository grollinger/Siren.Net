namespace WebApiContrib.Formatting.Siren.Client
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a sub-entity that should only be linked to.
    /// That is, its full representation should not be included in the representation of its parent.
    /// </summary>
    public class EmbeddedLink : Link, IEmbeddedLink
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        public ICollection<string> Classes { get; set; }

        public EmbeddedLink(
            string href,
            ICollection<string> rels
            )
            : base(href, rels)
        {
        }
    }
}