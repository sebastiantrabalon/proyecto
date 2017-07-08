using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Trees;

namespace FTCCompiler.Parsing.Context
{
    [DataContract(Namespace = "")]
    public class FormalParameters : IEnumerable<FormalParameter>
    {
        [DataMember(Name = "Declarations")]
        private readonly List<FormalParameter> _parameters = new List<FormalParameter>();

        public int AddParameter(FormalParameter formalParameter)
        {
            _parameters.Add(formalParameter);

            return _parameters.Count;
        }

        public bool HasParameters()
        {
            return _parameters.Count > 0;
        }

        public bool Exists(string identifier)
        {
            return _parameters.Any(x => x.Identifier == identifier);
        }

        public int Find(string identifier)
        {
            return _parameters.FindIndex(x => x.Identifier == identifier);
        }

        public FormalParameter this[string identifier]
        {
            get { return _parameters.First(x => x.Identifier == identifier); }
        }

        public FormalParameter this[int index]
        {
            get { return _parameters[index]; }
        }

        IEnumerator<FormalParameter> IEnumerable<FormalParameter>.GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }
    }

    [DataContract(Namespace = "")]
    public class FormalParameter
    {
        [DataMember]
        public string Identifier { get; set; }
        [DataMember]
        public DataType DataType { get; set; }
        [DataMember]
        public ParameterType ParameterType { get; set; }
    }

    [DataContract(Namespace = "")]
    public class ActualParameters : Attribute
    {
        public ActualParameters()
        {
            Parameters = new List<Expression>();
        }

        public void AddParameter(Expression parameter)
        {
            Parameters.Add(parameter);
        }

        [DataMember]
        public List<Expression> Parameters { get; set; }
    }
}
