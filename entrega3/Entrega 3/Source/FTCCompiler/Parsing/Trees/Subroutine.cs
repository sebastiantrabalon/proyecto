using FTCCompiler.Parsing.Context;
using System.Runtime.Serialization;
using Attribute = FTCCompiler.Parsing.Common.Attribute;
using Environment = FTCCompiler.Parsing.Context.Environment;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(Procedure))]
    [KnownType(typeof(Function))]
    public abstract class Subroutine : Attribute
    {
        private Environment _environment = new Environment();
        private Statements _statements = new Statements();

        protected Subroutine(string identifier)
        {
            Identifier = identifier;
        }

        [DataMember]
        public string Identifier { get; private set; }

        [DataMember]
        public Environment Environment
        {
            get { return _environment; }
            /*protected*/ set { _environment = value; }
        }

        [DataMember]
        public Statements Statements
        {
            get { return _statements; }
            protected set { _statements = value; }
        }
    }
}
