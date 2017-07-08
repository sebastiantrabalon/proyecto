using System.IO;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class SyntaxTree
    {
        public SyntaxTree(Subroutine rootNode, bool hasErrors)
        {
            Root = rootNode;
            HasErrors = hasErrors;
        }

        [DataMember]
        public bool HasErrors { get; set; }

        [DataMember(Name = "Main")]
        public Subroutine Root { get; set; }

        public void Serialize(Stream outputStream)
        {
            var serializer = new DataContractSerializer(GetType());
            serializer.WriteObject(outputStream, this);
        }
    }
}
