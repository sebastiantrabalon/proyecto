using FTCCompiler.Common;
using System;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class AdditionExpression : BinaryExpression
    {
        public AdditionExpression()
        {
            Type = DataType.Integer;
        }
        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
