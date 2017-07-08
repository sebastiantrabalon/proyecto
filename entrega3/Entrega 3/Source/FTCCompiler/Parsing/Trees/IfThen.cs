using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class IfThen : Statement
    {
        [DataMember]
        public Expression Condition { get; set; }
        [DataMember]
        public Statements Statements { get; set; }

        public override string GetCode()
        {
            throw new System.NotImplementedException();
        }
    }
}
