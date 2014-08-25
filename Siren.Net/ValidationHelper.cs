namespace WebApiContrib.Formatting.Siren.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Contains Helper Methods that validate Arguments passed to Object Constructors or Library Methods
    /// and throw the appropriate Exceptions if necessary.
    /// </summary>
    internal static class ValidationHelper
    {
        /// <summary>
        /// Validates a proposed Value for the <c>Rel</c> Property of a <see cref="Link"/>, <see cref="EmbeddedLink"/> or <see cref="EmbeddedRepresentation"/>.
        /// <exception cref="System.ArgumentNullException">If <paramref name="rels"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">If <paramref name="rels"/> is empty.</exception>
        /// </summary>
        /// <param name="rels">The proposed value.</param>
        public static void ValidateRel(ICollection<string> rels)
        {
            Contract.Requires<ArgumentNullException>(rels != null, "rels");
            Contract.Requires<ArgumentException>(rels.Count > 0, "rels may not be empty");
        }
        
        /// <summary>
        /// Validates a proposed Value for the <c>Href</c> Property of a <see cref="Link"/> or <see cref="EmbeddedLink"/>.
        /// <exception cref="System.ArgumentException">If <paramref name="href"/> is null or whitespace</exception>
        /// </summary>
        /// <param name="href">The proposed value.</param>
        public static void ValidateHref(string href)
        {
            Contract.Requires<ArgumentException>(Uri.IsWellFormedUriString(href, UriKind.RelativeOrAbsolute), "href must be a valid URI");
        }
    }
}
