namespace WebApiContrib.Formatting.Siren.Client
{
    using System.Collections.Generic;
    using System.Net.Http.Headers;

    public interface ISirenEntity
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        string Title { get; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        IDictionary<string, object> Properties { get; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        ICollection<Action> Actions { get; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        ICollection<IEmbeddedRepresentation> EmbeddedRepresentations { get; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        ICollection<IEmbeddedLink> EmbeddedLinks { get; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        ICollection<string> Classes { get; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        ICollection<Link> Links { get; }
    }

    public interface IEmbeddedEntity
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks>required</remarks>
        ICollection<string> Rel { get; }
    }

    public interface IEmbeddedLink : IEmbeddedEntity
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks>required</remarks>
        string Href { get; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        string Title { get; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        MediaTypeHeaderValue Type { get; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>optional</remarks>
        ICollection<string> Classes { get; }
    }

    public interface IEmbeddedRepresentation : IEmbeddedEntity, ISirenEntity
    {
    }
}