using FTCCompiler.Common;
using System;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class GreaterThanExpression : BinaryExpression
    {
        public GreaterThanExpression()
        {
            Type = DataType.Boolean;
        }
        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
