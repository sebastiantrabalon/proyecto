using System.Runtime.Serialization;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Common;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(BinaryExpression))]
    [KnownType(typeof(UnaryExpression))]
    public abstract class Expression : Attribute, IGenerable, IPrintable
    {
        [DataMember]
        public DataType Type { get; set; }

        public abstract string GetCode();
    }
}
