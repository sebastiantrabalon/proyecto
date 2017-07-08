using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(XorExpression))]
    [KnownType(typeof(OrExpression))]
    [KnownType(typeof(AndExpression))]
    [KnownType(typeof(EqualExpression))]
    [KnownType(typeof(GreaterThanExpression))]
    [KnownType(typeof(GreaterThanOrEqualExpression))]
    [KnownType(typeof(LessThanExpression))]
    [KnownType(typeof(LessThanOrEqualExpression))]
    [KnownType(typeof(DistinctExpression))]
    [KnownType(typeof(AdditionExpression))]
    [KnownType(typeof(SubstractionExpression))]
    [KnownType(typeof(MultiplicationExpression))]
    [KnownType(typeof(DivisionExpression))]
    public abstract class BinaryExpression : Expression
    {
        [DataMember]
        public Expression LeftOperand { get; set; }
        [DataMember]
        public Expression RightOperand { get; set; }
    }
}
