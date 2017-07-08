using System;

namespace FTCCompiler.Parsing.Common
{
    static class ParserFactory
    {
        private static Action<string> _logger;

        public static void SetLogger(Action<string> logger)
        {
            _logger = logger;
        }

        public static IParseable GetProgramParser()
        {
            return new ProgramParser(_logger);
        }

        public static IParseable GetEnvironmentParser()
        {
            return new EnvironmentParser(_logger);
        }

        public static IParseable GetAssignmentOrCallParser()
        {
            return new AssignmentOrCallParser(_logger);
        }

        public static IParseable GetDeclarationsParser()
        {
            return new DeclarationsParser(_logger);
        }

        public static IParseable GetFunctionParser()
        {
            return new FunctionParser(_logger);
        }

        public static IParseable GetProcedureParser()
        {
            return new ProcedureParser(_logger);
        }

        public static IParseable GetFormalParametersParser()
        {
            return new FormalParametersParser(_logger);
        }

        public static IParseable GetActualParametersParser()
        {
            return new ActualParametersParser(_logger);
        }

        public static IParseable GetBodyParser()
        {
            return new BodyParser(_logger);
        }

        public static IParseable GetIfParser()
        {
            return new IfParser(_logger);
        }

        public static IParseable GetWhileParser()
        {
            return new WhileParser(_logger);
        }

        public static IParseable GetReadParser()
        {
            return new ReadParser(_logger);
        }
        
        public static IParseable GetShowParser()
        {
            return new ShowParser(_logger);
        }

        public static IParseable GetExpressionParser()
        {
            return new ExpressionParser(_logger);
        }
    }
}
