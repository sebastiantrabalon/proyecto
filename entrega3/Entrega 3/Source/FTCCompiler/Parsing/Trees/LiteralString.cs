using System.Runtime.Serialization;
using FTCCompiler.Parsing.Common;
using Attribute = System.Attribute;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class LiteralString : Attribute, IPrintable
    {
        public LiteralString(string value)
        {
            Value = value;
        }

        [DataMember]
        public string Value { get; set; }
    }
}
