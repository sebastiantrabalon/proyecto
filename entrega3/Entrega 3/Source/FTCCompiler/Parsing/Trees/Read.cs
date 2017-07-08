using System.Runtime.Serialization;
using FTCCompiler.Parsing.Common;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class Read : Statement
    {
        [DataMember]
        public Expression Destination { get; set; }

        public override string GetCode()
        {
            throw new System.NotImplementedException();
        }
    }
}
