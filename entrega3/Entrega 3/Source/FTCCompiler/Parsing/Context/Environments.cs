using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Trees;
using Attribute = FTCCompiler.Parsing.Common.Attribute;

namespace FTCCompiler.Parsing.Context
{
    public class Environments : Attribute
    {
        private readonly List<Environment> _environments = new List<Environment>();

        public void Push(Environment environment)
        {
            _environments.Insert(0, environment);
        }
        
        public Environment Pop()
        {
            var environment = _environments[0];
            
            _environments.RemoveAt(0);

            return environment;
        }

        public Environment this[int index]
        {
            get { return _environments[index]; }
        }

        public Reference FindVariable(string identifier) 
        {
            var result = new Reference();

            result.EnvironmentReference = _environments.FindIndex(x => {

                result.ListReference = x.Locals.FindVariable(identifier);

                return result.ListReference >= 0;
            });

            return result;
        }

        public Reference FindConstant(string identifier)
        {
            var result = new Reference();

            result.EnvironmentReference = _environments.FindIndex(x =>
            {

                result.ListReference = x.Locals.FindConstant(identifier);

                return result.ListReference >= 0;
            });

            return result;
        }

        public Reference FindArray(string identifier)
        {
            var result = new Reference();

            result.EnvironmentReference = _environments.FindIndex(x =>
            {

                result.ListReference = x.Locals.FindArray(identifier);

                return result.ListReference >= 0;
            });

            return result;
        }

        public Reference FindLocal(string identifier)
        {
            var result = new Reference();

            result.EnvironmentReference = _environments.FindIndex(x =>
            {

                result.ListReference = x.Locals.FindLocal(identifier);

                return result.ListReference >= 0;
            });

            return result;
        }

        public Reference FindFunction(string identifier)
        {
            var result = new Reference();

            result.EnvironmentReference = _environments.Take(2).ToList().FindIndex(x =>
            {

                result.ListReference = x.Subroutines.FindFunction(identifier);

                return result.ListReference >= 0;
            });

            return result;
        }

        public Reference FindProcedure(string identifier)
        {
            var result = new Reference();

            result.EnvironmentReference = _environments.Take(2).ToList().FindIndex(x =>
            {

                result.ListReference = x.Subroutines.FindProcedure(identifier);

                return result.ListReference >= 0;
            });

            return result;
        }
        
        public bool ExistsLocal(string identifier)
        {
            return _environments.Any(x => x.Locals.Exists(identifier));
        }

        public bool ExistsVariable(string identifier)
        {
            return _environments.Any(x => x.Locals.ExistsVariable(identifier));
        }

        public bool ExistsArray(string identifier)
        {
            return _environments.Any(x => x.Locals.ExistsArray(identifier));
        }

        public bool ExistsConstant(string identifier)
        {
            return _environments.Any(x => x.Locals.ExistsConstant(identifier));
        }

        public bool ExistsFunction(string identifier)
        {
            return _environments.Take(2).Any(x => x.Subroutines.ExistsFunction(identifier));
        }

        public bool ExistsProcedure(string identifier)
        {
            return _environments.Take(2).Any(x => x.Subroutines.ExistsProcedure(identifier));
        }

        public Local GetLocal(Reference reference)
        {
            if (reference != null && reference.EnvironmentReference >= 0 && reference.ListReference >= 0)
                return _environments[reference.EnvironmentReference].Locals[reference.ListReference];

            return null;
        }

        public Subroutine GetSubroutine(Reference reference)
        {
            if (reference != null && reference.EnvironmentReference >= 0 && reference.ListReference >= 0)
                return _environments[reference.EnvironmentReference].Subroutines[reference.ListReference];

            return null;
        }
    }

    [DataContract(Namespace = "")]
    public class Environment : Attribute
    {
        private Locals _locals = new Locals();
        private FormalParameters _formalParameters = new FormalParameters();
        private Subroutines _subroutines = new Subroutines();

        [DataMember]
        public Locals Locals
        {
            get { return _locals; }
            protected set { _locals = value; }
        }

        [DataMember]
        public FormalParameters FormalParameters
        {
            get { return _formalParameters; }
            protected set { _formalParameters = value; }
        }

        [DataMember]
        public Subroutines Subroutines
        {
            get { return _subroutines; }
            protected set { _subroutines = value; }
        }

        [DataMember]
        public bool IsGlobal { get; set; }

        public bool ExistsParameter(string identifier)
        {
            return _formalParameters.Exists(identifier);
        }

        public Reference FindParameter(string identifier)
        {
            var result = new Reference();

            result.EnvironmentReference = 0;
            result.ListReference = _formalParameters.Find(identifier);
            result.IsParameter = true;

            return result;
        }

        public FormalParameter GetParameter(Reference reference)
        {
            if (reference.EnvironmentReference == 0 && reference.IsParameter && reference.ListReference >= 0)
                return _formalParameters[reference.ListReference];

            return null;
        }
    }
}
