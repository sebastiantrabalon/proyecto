using FTCCompiler.Common;
using FTCCompiler.Parsing.Context;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class Function : Subroutine
    {
        public Function(string identifier) 
            : base(identifier)
        {
        }

        [DataMember]
        public DataType ReturnType { get; set; }

        [DataMember]
        public Expression ReturnExpression { get; set; }

        public Function(string identifier, Environment environment, Statements statements)
            : base(identifier)
        {
            Environment = environment;
            Statements = statements;
        }
    }
}
