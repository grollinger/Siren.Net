namespace WebApiContrib.Formatting.Siren.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using System.Net.Http.Headers;

    /// <summary>
    /// Actions show available behaviors an entity exposes.
    /// </summary>
    public class Action
    {
        /// <summary>
        /// Gets a string that identifies the field to be performed.
        /// </summary>
        /// <remarks>Required Property.</remarks>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the URI of the field.
        /// </summary>
        /// <remarks>Required Property.</remarks>
        public string Href { get; private set; }

        /// <summary>
        /// Gets a descriptive text about the field.
        /// </summary>
        /// <remarks>Optional Property.</remarks>
        public string Title { get; set; }

        /// <summary>
        /// Gets an enumerated attribute mapping to a protocol method.
        /// For HTTP, these values may be GET, PUT, POST, DELETE, or PATCH.
        /// As new methods are introduced, this list can be extended.
        /// If this attribute is omitted, GET should be assumed.
        /// </summary>
        /// <remarks>Optional Property.</remarks>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Gets the encoding type for the request.
        /// When omitted and the fields attribute exists, the default value is <c>"application/x-www-form-urlencoded"</c>.
        /// </summary>
        /// <remarks>Optional Property.</remarks>
        public MediaTypeHeaderValue Type { get; set; }

        /// <summary>
        /// Gets a collection of values that describe the nature of an field based on the current representation.
        /// Possible values are implementation-dependent and should be documented.
        /// </summary>
        /// <remarks>Optional Property.</remarks>
        public ICollection<string> Classes { get; set; }

        /// <summary>
        /// Gets a collection of fields, expressed as an array of objects in JSON Siren such as { "fields" : [{ ... }] }.
        /// See <see cref="Field"/>.
        /// </summary>
        /// <remarks>Optional Property.</remarks>
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