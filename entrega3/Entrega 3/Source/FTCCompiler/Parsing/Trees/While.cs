using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class While : Statement
    {
        public While()
        {
            Statements = new Statements();
        }

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
