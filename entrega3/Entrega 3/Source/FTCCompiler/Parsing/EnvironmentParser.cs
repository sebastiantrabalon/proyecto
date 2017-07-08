using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using System;

namespace FTCCompiler.Parsing
{
    class EnvironmentParser : ParserBase, IParseable
    {
        public EnvironmentParser(Action<string> logger)
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            successfullyParsed &= ParseDeclarations(lexer, token, attributes);
            successfullyParsed &= ParseFunctionsAndProcedures(lexer, lexer.GetCurrentToken(), attributes);

            return successfullyParsed;
        }

        private bool ParseDeclarations(Lexer lexer, Token token, Attributes attributes)
        {
            if (token.Is(TokenType.VariableDefinition) || token.Is(TokenType.ConstDefinition))
                return ParserFactory.GetDeclarationsParser().Parse(lexer, token, attributes);

            return true;
        }

        private bool ParseFunctionsAndProcedures(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            while (lexer.GetCurrentToken().Is(TokenType.FunctionDefinition) || lexer.GetCurrentToken().Is(TokenType.ProcedureDefinition))
            {
                if (token.Is(TokenType.FunctionDefinition))
                {
                    successfullyParsed &= ParserFactory.GetFunctionParser().Parse(lexer, token, attributes);
                }
                if (token.Is(TokenType.ProcedureDefinition))
                {
                    successfullyParsed &= ParserFactory.GetProcedureParser().Parse(lexer, token, attributes);
                }

                token = lexer.GetNextToken();
            }

            return successfullyParsed;
        }
    }
}
