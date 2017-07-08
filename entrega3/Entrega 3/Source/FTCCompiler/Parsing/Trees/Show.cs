using System.Runtime.Serialization;
using FTCCompiler.Parsing.Common;
using System.Collections.Generic;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(LiteralString))]
    public class Show : Statement
    {
        public Show()
        {
            Values = new List<IPrintable>();
        }

        [DataMember]
        public List<IPrintable> Values { get; set; }

        public override string GetCode()
        {
            throw new System.NotImplementedException();
        }
    }
}
