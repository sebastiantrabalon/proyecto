using System;
using System.Runtime.Serialization;
using FTCCompiler.Common;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class LiteralNumberExpression : UnaryExpression
    {
        public LiteralNumberExpression(short value)
        {
            Value = value;
            Type = DataType.Integer;
        }

        [DataMember]
        public short Value { get; set; }

        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
