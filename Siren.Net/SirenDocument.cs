namespace WebApiContrib.Formatting.Siren.Client
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;    

    public class DynamicSirenEntity : DynamicObject, ISirenEntity
    {
        public string Title { get; set; }
        public IDictionary<string, object> Properties { get; set; }
        public ICollection<Action> Actions { get; set; }
        public ICollection<IEmbeddedEntity> Entities { get; set; }
        public ICollection<string> Classes { get; set; }
        public ICollection<Link> Links { get; set; }


        public DynamicSirenEntity()
        {
            Properties = new Dictionary<string, object>();
            Classes = new List<string>();
            Links = new List<Link>();
            Actions = new List<Action>();
            Entities = new List<IEmbeddedEntity>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!base.TryGetMember(binder, out result))
            {

                return Properties.TryGetValue(binder.Name, out result);
            }
            else
            {
                return true;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!base.TrySetMember(binder, value))
            {
                Properties[binder.Name] = value;
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
