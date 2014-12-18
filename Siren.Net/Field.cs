namespace WebApiContrib.Formatting.Siren.Client
{
    using System;
    using System.Diagnostics.Contracts;

    public class Field
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks>required</remarks>
        public string Name { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        public FieldType Type { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        public object Value { get; set; }

        public Field(
            string name
            )
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name), "'name' must be non-empty");

            Name = name;
        }
    }
}