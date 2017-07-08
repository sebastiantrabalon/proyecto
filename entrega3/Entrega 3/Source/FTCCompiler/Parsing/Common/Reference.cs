using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Common
{
    [DataContract(Namespace = "")]
    public class Reference
    {
        [DataMember]
        public int EnvironmentReference { get; set; }
        [DataMember]
        public int ListReference { get; set; }
        [DataMember]
        public bool IsParameter { get; set; }
    }
}
