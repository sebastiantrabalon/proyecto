using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Common.Exceptions;
using FTCCompiler.Parsing.Context;
using FTCCompiler.Parsing.Trees;
using System;

namespace FTCCompiler
{
    class Parser
    {
        private readonly Lexer _lexer;
        private readonly Action<string> _logger;

        public Parser(Lexer lexer, Action<string> logger)
        {
            _logger = logger;
            _lexer = lexer;
        }

        public SyntaxTree Parse()
        {
            try
            {
                ParserFactory.SetLogger(_logger);

                var attributes = new Attributes();

                var successfullyParsed = ParserFactory.GetProgramParser().Parse(_lexer, _lexer.GetNextToken(), attributes);

                var globalEnvironment = ((Environments)attributes["ENVS"])[0];

                // SEM: Debe existir un procedimiento MAIN.
                if (!globalEnvironment.Subroutines.ExistsProcedure("MAIN"))
                    throw new MainNotFoundException();

                var mainProcedure = globalEnvironment.Subroutines["MAIN"];

                globalEnvironment.Subroutines.RemoveSubroutine("MAIN");

                var main = new Procedure("MAIN", globalEnvironment, mainProcedure.Statements);

                return new SyntaxTree(main, !successfullyParsed);
            }
            catch (Exception ex)
            {
                _logger(ex.Message);
            }

            return new SyntaxTree(null, true);
        }
    }
}
