using System;
using System.Runtime.Serialization;

namespace FTCCompiler.Parsing.Trees
{
    [DataContract(Namespace = "")]
    public class ShowLn : Show
    {
        public override string GetCode()
        {
            throw new NotImplementedException();
        }
    }
}
