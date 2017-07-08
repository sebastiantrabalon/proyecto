using System.Runtime.Serialization;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class IfThenElse : Statement
    {
        [DataMember]
        public Expression Condition { get; set; }
        [DataMember]
        public Statements Then { get; set; }
        [DataMember]
        public Statements Else { get; set; }

        public override string GetCode()
        {
            throw new System.NotImplementedException();
        }
    }
}
