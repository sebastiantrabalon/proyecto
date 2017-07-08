using FTCCompiler.Common;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class XorExpression : BinaryExpression
    {
        public XorExpression()
        {
            Type = DataType.Boolean;
        }
        public override string GetCode()
        {
            throw new System.NotImplementedException();
        }
    }
}
