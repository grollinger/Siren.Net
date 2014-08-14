namespace WebApiContrib.Formatting.Siren.Client
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class EmbeddedRepresentation : SirenEntity, IEmbeddedRepresentation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>required</remarks>
        public ICollection<string> Rel { get; private set; }

        
        public EmbeddedRepresentation(
            ICollection<string> rels
            )
        {
            ValidationHelper.ValidateRel(rels);
        }
    }
}
