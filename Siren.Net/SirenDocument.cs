namespace WebApiContrib.Formatting.Siren.Client
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    public class SirenDocument : DynamicObject
    {
        public IDictionary<string, object> Properties { get; set; }
        public IEnumerable<int> Actions { get; set; }
        public IEnumerable<int> Entities { get; set; }
        public IEnumerable<string> Classes { get; set; }
        public IEnumerable<Link> Links { get; set; }

        public object this[string index]
        {
            get { return Properties[index]; }
            set { Properties[index] = value; }
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

        public override bool TrySetMember(SetMemberBinder binder, object result)
        {
            if (!base.TrySetMember(binder, result))
            {
                Properties[binder.Name] = result;
                return true;
            }
            else
            {
                return true;
            }
        }

        public SirenDocument()
        {
            Properties = new Dictionary<string, object>();
            Classes = Enumerable.Empty<string>();
            Links = Enumerable.Empty<Link>();
            Actions = Entities = Enumerable.Empty<int>();
        }
    }
}
