using System;
using System.Runtime.Serialization;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class FunctionCallExpression : UnaryExpression
    {
        public FunctionCallExpression()
        {
            ActualParameters = new ActualParameters();
        }

        [DataMember]
        public string Identifier { get; set; }
        [DataMember]
        public ActualParameters ActualParameters { get; set; }
        [DataMember]
        public Reference Reference { get; set; }

        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
