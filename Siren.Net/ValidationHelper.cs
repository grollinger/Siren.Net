namespace WebApiContrib.Formatting.Siren.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;


    internal static class ValidationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rels"></param>
        public static void ValidateRel(ICollection<string> rels)
        {
            Contract.Requires<ArgumentNullException>(rels != null, "rels");
            Contract.Requires<ArgumentException>(rels.Count > 0, "rels may not be empty");
        }

        public static void ValidateHref(string href)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(href), "href must be non-empty");
        }
    }
}
