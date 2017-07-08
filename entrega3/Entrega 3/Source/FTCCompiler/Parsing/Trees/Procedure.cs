using System.Runtime.Serialization;
using FTCCompiler.Parsing.Context;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class Procedure : Subroutine
    {
        public Procedure(string identifier) 
            : base(identifier)
        {
        }

        public Procedure(string identifier, Environment environment, Statements statements)
            : base(identifier)
        {
            Environment = environment;
            Statements = statements;
        }
    }
}
