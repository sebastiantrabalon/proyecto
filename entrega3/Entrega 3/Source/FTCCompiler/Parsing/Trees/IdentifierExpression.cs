using FTCCompiler.Parsing.Common;
using System;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class IdentifierExpression : UnaryExpression
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Reference Reference { get; set; }

        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
