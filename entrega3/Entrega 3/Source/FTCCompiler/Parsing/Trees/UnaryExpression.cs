using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(LiteralNumberExpression))]
    [KnownType(typeof(LiteralBooleanExpression))]
    [KnownType(typeof(IdentifierExpression))]
    [KnownType(typeof(PositionInArrayExpression))]
    [KnownType(typeof(FunctionCallExpression))]
    public abstract class UnaryExpression : Expression
    {
    }
}
