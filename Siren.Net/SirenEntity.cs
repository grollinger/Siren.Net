namespace WebApiContrib.Formatting.Siren.Client
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;    

    public class SirenEntity : ISirenEntity
    {
        public string Title { get; set; }
        public IDictionary<string, object> Properties { get; set; }
        public ICollection<Action> Actions { get; set; }
        public ICollection<IEmbeddedEntity> Entities { get; set; }
        public ICollection<string> Classes { get; set; }
        public ICollection<Link> Links { get; set; }


        public SirenEntity()
        {
            Properties = new Dictionary<string, object>();
            Classes = new List<string>();
            Links = new List<Link>();
            Actions = new List<Action>();
            Entities = new List<IEmbeddedEntity>();
        }
    }
}
