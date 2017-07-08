using System;
using System.Runtime.Serialization;
using FTCCompiler.Common;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class LiteralBooleanExpression : UnaryExpression
    {
        public LiteralBooleanExpression(bool value)
        {
            Value = value;
            Type = DataType.Boolean;
        }

        [DataMember]
        public bool Value { get; set; }

        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
