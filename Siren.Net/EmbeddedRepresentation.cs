namespace WebApiContrib.Formatting.Siren.Client
{
    using System.Collections.Generic;

    public class EmbeddedRepresentation : SirenEntity, IEmbeddedRepresentation
    {
        /// <inheritdoc cref="IEmbeddedEntity.Rel"/>
        public ICollection<string> Rel { get; private set; }

        public EmbeddedRepresentation(
            ICollection<string> rels
            )
        {
            ValidationHelper.ValidateRel(rels);

            Rel = rels;
        }
    }
}