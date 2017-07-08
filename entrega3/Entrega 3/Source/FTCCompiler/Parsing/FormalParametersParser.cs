using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using FTCCompiler.Parsing.Trees;
using System;
using System.Collections.Generic;
using System.Linq;
using Environment = FTCCompiler.Parsing.Context.Environment;

namespace FTCCompiler.Parsing
{
    class FormalParametersParser : ParserBase, IParseable
    {
        public FormalParametersParser(Action<string> logger) 
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            ValidateToken(token, TokenType.LeftParenthesis);

            var environment = attributes["ENV"] as Environment;

            var successfullyParsed = ParseParameters(lexer, environment);

            ValidateToken(lexer.GetCurrentToken(), TokenType.RightParenthesis);

            return successfullyParsed;
        }

        private bool ParseParameters(Lexer lexer, Environment environment)
        {
            var successfullyParsed = true;

            do
            {
                var token = lexer.GetNextToken();

                if (token.Is(TokenType.RightParenthesis))
                    return true;

                var parameter = new FormalParameter();

                if (token.Is(TokenType.ByVal) || token.Is(TokenType.ByRef))
                {
                    parameter.ParameterType = token.Is(TokenType.ByVal) 
                        ? ParameterType.ByVal : ParameterType.ByRef;

                    token = lexer.GetNextToken();
                    ValidateToken(token, TokenType.Identifier);
                }
                else
                {
                    parameter.ParameterType = ParameterType.ByVal;

                    ValidateToken(token, TokenType.Identifier);
                }

                parameter.Identifier = token.Lexeme;

                // SEM: Validar que no haya parametros con nombres duplicados.
                if (environment.FormalParameters.Exists(parameter.Identifier))
                {
                    LogDuplicateIdentifierFound(parameter.Identifier, token);
                    successfullyParsed = false;
                }

                ValidateToken(lexer.GetNextToken(), TokenType.IdTypeSeparator);

                token = lexer.GetNextToken();
                ValidateToken(token, TokenType.IntegerDataType, TokenType.BooleanDataType);

                parameter.DataType = token.Type == TokenType.IntegerDataType
                    ? DataType.Integer : DataType.Boolean;

                environment.FormalParameters.AddParameter(parameter);

            } while (lexer.GetNextToken().Is(TokenType.ListSeparator));

            return successfullyParsed;
        }

        public static bool ValidateFormalParametersVsActualParameters(IList<FormalParameter> formalParameters,
            IList<Expression> actualParameters)
        {
            if (formalParameters.Count() == actualParameters.Count())
            {
                for (var i = 0; i < formalParameters.Count(); i++)
                {
                    if (formalParameters[i].DataType != actualParameters[i].Type)
                        return false;
                }

                return true;
            }

            return false;
        }
    }
}
