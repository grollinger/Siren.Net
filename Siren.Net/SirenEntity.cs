namespace WebApiContrib.Formatting.Siren.Client
{
    using System.Collections.Generic;

    public class SirenEntity : ISirenEntity
    {
        public string Title { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        public ICollection<Action> Actions { get; set; }

        public ICollection<IEmbeddedRepresentation> EmbeddedRepresentations { get; set; }

        public ICollection<IEmbeddedLink> EmbeddedLinks { get; set; }

        public ICollection<string> Classes { get; set; }

        public ICollection<Link> Links { get; set; }

        public SirenEntity()
        {
            Properties = new Dictionary<string, object>();
            Classes = new List<string>();
            Links = new List<Link>();
            Actions = new List<Action>();
            EmbeddedLinks = new List<IEmbeddedLink>();
            EmbeddedRepresentations = new List<IEmbeddedRepresentation>();
        }
    }
}