using FTCCompiler.Common;
using System;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class EqualExpression : BinaryExpression
    {
        public EqualExpression()
        {
            Type = DataType.Boolean;
        }

        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
