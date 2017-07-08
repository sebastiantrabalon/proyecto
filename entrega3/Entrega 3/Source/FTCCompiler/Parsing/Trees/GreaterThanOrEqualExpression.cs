using FTCCompiler.Common;
using System;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class GreaterThanOrEqualExpression : BinaryExpression
    {
        public GreaterThanOrEqualExpression()
        {
            Type = DataType.Boolean;
        }
        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
