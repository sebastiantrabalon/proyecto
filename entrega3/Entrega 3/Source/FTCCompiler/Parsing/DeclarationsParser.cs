using System.Linq;
using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using System;
using System.Collections.Generic;
using Environment = FTCCompiler.Parsing.Context.Environment;

namespace FTCCompiler.Parsing
{
    class DeclarationsParser : ParserBase, IParseable
    {
        public DeclarationsParser(Action<string> logger)
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            try
            {
                var environment = ((Environments) attributes["ENVS"])[0];

                if (token.Is(TokenType.ConstDefinition))
                {
                    successfullyParsed &= ParseConstants(lexer, environment);
                    ValidateToken(lexer.GetCurrentToken(), TokenType.EndOfInstruction);

                    successfullyParsed &= Parse(lexer, lexer.GetNextToken(), attributes);
                }
                if (token.Is(TokenType.VariableDefinition))
                {
                    var variables = new List<Local>();

                    successfullyParsed &= ParseVariables(lexer, environment, variables);

                    ValidateToken(lexer.GetCurrentToken(), TokenType.IdTypeSeparator);

                    var dataTypeToken = lexer.GetNextToken();
                    ValidateToken(dataTypeToken, TokenType.IntegerDataType, TokenType.BooleanDataType);

                    ValidateToken(lexer.GetNextToken(), TokenType.EndOfInstruction);

                    foreach (var variable in variables)
                    {
                        variable.Type = dataTypeToken.Type == TokenType.IntegerDataType
                            ? DataType.Integer
                            : DataType.Boolean;

                        environment.Locals.AddLocal(variable);
                    }

                    successfullyParsed &= Parse(lexer, lexer.GetNextToken(), attributes);
                }
            }
            catch (Exception ex)
            {
                successfullyParsed = false;
                Logger(ex.Message);
                ErrorRecovery(lexer);

                Parse(lexer, lexer.GetNextToken(), attributes);
            }

            return successfullyParsed;
        }

        private bool ParseConstants(Lexer lexer, Environment currentEnvironment)
        {
            var successfullyParsed = true;

            do
            {
                var identifierToken = lexer.GetNextToken();
                ValidateToken(identifierToken, TokenType.Identifier);

                // SEM: Verifico que no exista un identificador con el mismo nombre en el entorno local.
                if (currentEnvironment.Locals.Exists(identifierToken.Lexeme) && 
                    currentEnvironment.FormalParameters.Exists(identifierToken.Lexeme))
                {
                    LogDuplicateIdentifierFound(identifierToken.Lexeme, identifierToken);
                    successfullyParsed = false;
                }

                ValidateToken(lexer.GetNextToken(), TokenType.IdTypeSeparator);

                var dataTypeToken = lexer.GetNextToken();
                ValidateToken(dataTypeToken, TokenType.IntegerDataType, TokenType.BooleanDataType);

                ValidateToken(lexer.GetNextToken(), TokenType.EqualOperator);

                var valueToken = lexer.GetNextToken();
                ValidateToken(valueToken, TokenType.LiteralNumber, TokenType.LiteralBoolean);

                var dataType = dataTypeToken.Type == TokenType.IntegerDataType 
                        ? DataType.Integer : DataType.Boolean;

                currentEnvironment.Locals.AddLocal(new Local() { 
                            Identifier = identifierToken.Lexeme,
                            IsConstant = true,
                            Type = dataType,
                            Value = valueToken.Lexeme
                        });

            } while (lexer.GetNextToken().Is(TokenType.ListSeparator)) ;

            return successfullyParsed;
        }

        private bool ParseVariables(Lexer lexer, Environment currentEnvironment, List<Local> localVariables)
        {
            var successfullyParsed = true;
            Token token;

            do
            {
                var identifierToken = lexer.GetNextToken();
                ValidateToken(identifierToken, TokenType.Identifier);

                // SEM: Verifico que no exista un identificador con el mismo nombre en el entorno local.
                if (currentEnvironment.Locals.Exists(identifierToken.Lexeme) ||
                    currentEnvironment.FormalParameters.Exists(identifierToken.Lexeme) ||
                    localVariables.Any(x => x.Identifier == identifierToken.Lexeme))
                {
                    LogDuplicateIdentifierFound(identifierToken.Lexeme, identifierToken);
                    successfullyParsed = false;
                }

                token = lexer.GetNextToken();

                if (token.Is(TokenType.LeftSquareBracket))
                {
                    var arraySizeToken = lexer.GetNextToken();

                    ValidateToken(arraySizeToken, TokenType.LiteralNumber);
                    ValidateToken(lexer.GetNextToken(), TokenType.RightSquareBracket);

                    if (!currentEnvironment.IsGlobal)
                    {
                        LogArrayCantBeDeclaredOutsideGlobalEnvironment(identifierToken);
                        successfullyParsed = false;
                    }

                    var arraySize = Convert.ToInt32(arraySizeToken.Lexeme);

                    localVariables.Add(new Local() {
                        Identifier = identifierToken.Lexeme, 
                        ArraySize = arraySize,
                        IsArray = true
                    });

                    token = lexer.GetNextToken();
                }
                else
                {
                    localVariables.Add(new Local() {
                        Identifier = identifierToken.Lexeme
                    });
                }

            } while (token.Is(TokenType.ListSeparator));

            return successfullyParsed;
        }
    }
}
