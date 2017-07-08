using System.Runtime.Serialization;
using FTCCompiler.Parsing.Trees;

namespace FTCCompiler.Parsing.Common
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(Assignment))]
    [KnownType(typeof(IfThen))]
    [KnownType(typeof(IfThenElse))]
    [KnownType(typeof(ProcedureCall))]
    [KnownType(typeof(Read))]
    [KnownType(typeof(Show))]
    [KnownType(typeof(ShowLn))]
    [KnownType(typeof(While))]
    public abstract class Statement : Attribute, IGenerable
    {
        public abstract string GetCode();
    }
}
