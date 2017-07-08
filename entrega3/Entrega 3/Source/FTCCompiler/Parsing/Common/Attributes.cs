using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Common
{
    public class Attributes : IEnumerable<Attribute>
    {
        private readonly Dictionary<string, Attribute> _attributes = new Dictionary<string, Attribute>();

        public static Attributes Create(Attribute attribute, string name)
        {
            var instance = new Attributes();

            instance.AddAttribute(attribute, name);

            return instance;
        }

        public static Attributes Create(params Attribute[] attributes)
        {
            var instance = new Attributes();

            attributes.ToList().ForEach(x => instance.AddAttribute(x, x.AttributeName));

            return instance;
        }

        public void AddAttribute(Attribute attribute, string name)
        {
            if (name != null && attribute != null)
                attribute.AttributeName = name;

            _attributes.Add(name, attribute);
        }

        public void RemoveAttribute(string name)
        {
            _attributes.Remove(name);
        }

        public bool ContainsAttribute(string name)
        {
            return _attributes.ContainsKey(name);
        }

        public Attribute this[string name]
        {
            get { return _attributes[name]; }
            set { _attributes[name] = value; }
        }

        public Attribute this[int index]
        {
            get { return _attributes.Values.ToList()[index]; }
        }

        public IEnumerator<Attribute> GetEnumerator()
        {
            return _attributes.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int GetCount()
        {
            return _attributes.Values.Count;
        }

        public List<KeyValuePair<string,Attribute>> GetAttributes()
        {
            return _attributes.ToList();
        }
    }
    
    [DataContract]
    public abstract class Attribute
    {
        public string AttributeName { get; set; }
    }
}
