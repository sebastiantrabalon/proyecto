using FTCCompiler.Parsing.Common;
using System;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class Assignment : Statement
    {
        [DataMember]
        public Expression Destination { get; set; }
        [DataMember]
        public Expression Expression { get; set; }

        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
