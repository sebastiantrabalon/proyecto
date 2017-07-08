using FTCCompiler.Parsing.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Context
{
    [DataContract(Namespace = "")]
    public class Statements : Attribute
    {
        [DataMember(Name = "Declarations")]
        private readonly List<Statement> _statements = new List<Statement>();

        public int AddStatement(Statement statement)
        {
            _statements.Add(statement);

            return _statements.Count;
        }

        public Statement this[int index]
        {
            get { return _statements[index]; }
        }
    }
}
