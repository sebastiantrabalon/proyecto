using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FTCCompiler.Parsing.Trees;

namespace FTCCompiler.Parsing.Context
{
    [DataContract(Namespace = "")]
    public class Subroutines
    {
        [DataMember(Name = "Declarations")]
        private readonly List<Subroutine> _subroutines = new List<Subroutine>();

        public void AddSubroutine(Subroutine subroutine)
        {
            _subroutines.Add(subroutine);
        }

        public void RemoveSubroutine(string identifier)
        {
            _subroutines.Remove(_subroutines.FirstOrDefault(x => x.Identifier == identifier));
        }

        public bool ExistsProcedure(string identifier)
        {
            return _subroutines.OfType<Procedure>().Any(x => x.Identifier == identifier);
        }

        public bool ExistsFunction(string identifier)
        {
            return _subroutines.OfType<Function>().Any(x => x.Identifier == identifier);
        }

        public bool Exists(string identifier)
        {
            return ExistsFunction(identifier) || ExistsProcedure(identifier);
        }

        public int FindFunction(string identifier)
        {
            return _subroutines.FindIndex(x => x.Identifier == identifier && x is Function);
        }

        public int FindProcedure(string identifier)
        {
            return _subroutines.FindIndex(x => x.Identifier == identifier && x is Procedure);
        }

        public Subroutine this[string identifier]
        {
            get { return _subroutines.First(x => x.Identifier == identifier); }
        }

        public Subroutine this[int index]
        {
            get { return _subroutines[index]; }
        }

        public bool HasSubroutines()
        {
            return _subroutines.Count > 0;
        }

        public int Count()
        {
            return _subroutines.Count();
        }
    }
}
