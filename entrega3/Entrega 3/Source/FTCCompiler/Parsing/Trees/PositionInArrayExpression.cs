using System;
using System.Runtime.Serialization;
using FTCCompiler.Parsing.Common;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class PositionInArrayExpression : UnaryExpression
    {
        [DataMember]
        public string Identifier { get; set; }
        [DataMember]
        public Reference Reference { get; set; }
        [DataMember]
        public Expression Position { get; set; }

        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
