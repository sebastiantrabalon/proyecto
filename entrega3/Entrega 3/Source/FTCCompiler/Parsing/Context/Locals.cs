using System.Runtime.Serialization;
using FTCCompiler.Common;
using System.Collections.Generic;
using System.Linq;

namespace FTCCompiler.Parsing.Context
{
    [DataContract(Namespace = "")]
    public class Locals
    {
        [DataMember(Name = "Declarations")]
        private readonly List<Local> _locals = new List<Local>();

        public int AddLocal(Local local)
        {
            _locals.Add(local);

            return _locals.Count;
        }

        public bool Exists(string identifier) 
        {
            return _locals.Any(x => x.Identifier == identifier);
        }

        public bool ExistsVariable(string identifier)
        {
            return _locals.Any(x => x.Identifier == identifier && !x.IsArray && !x.IsConstant);
        }

        public bool ExistsConstant(string identifier)
        {
            return _locals.Any(x => x.Identifier == identifier && x.IsConstant);
        }

        public bool ExistsArray(string identifier)
        {
            return _locals.Any(x => x.Identifier == identifier && x.IsArray);
        }

        public int FindVariable(string identifier)
        {
            return _locals.FindIndex(x => x.Identifier == identifier && !x.IsArray && !x.IsConstant);
        }

        public int FindConstant(string identifier)
        {
            return _locals.FindIndex(x => x.Identifier == identifier && x.IsConstant);
        }

        public int FindArray(string identifier)
        {
            return _locals.FindIndex(x => x.Identifier == identifier && x.IsArray);
        }

        public int FindLocal(string identifier)
        {
            return _locals.FindIndex(x => x.Identifier == identifier);
        }

        public Local this[string identifier]
        {
            get { return _locals.First(x => x.Identifier == identifier); }
        }

        public Local this[int index]
        {
            get { return _locals[index]; }
        }

        public bool HasLocals()
        {
            return _locals.Count > 0;
        }

        public int Count()
        {
            return _locals.Count();
        }
    }

    [DataContract(Namespace = "")]
    public class Local
    {
        [DataMember]
        public string Identifier { get; set; }
        [DataMember]
        public DataType Type { get; set; }
        [DataMember]
        public bool IsConstant { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public bool IsArray { get; set; }
        [DataMember]
        public int ArraySize { get; set; }
    }
}