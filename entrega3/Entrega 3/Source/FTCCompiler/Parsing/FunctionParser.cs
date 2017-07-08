using System.Linq;
using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using FTCCompiler.Parsing.Trees;
using System;
using Environment = FTCCompiler.Parsing.Context.Environment;

namespace FTCCompiler.Parsing
{
    class FunctionParser : ParserBase, IParseable
    {
        public FunctionParser(Action<string> logger) 
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            var environments = ((Environments)attributes["ENVS"]);

            var function = ParseFunctionDeclaration(lexer, token, environments[0], out successfullyParsed);

            environments[0].Subroutines.AddSubroutine(function);

            environments.Push(function.Environment);

            successfullyParsed &= ParserFactory.GetEnvironmentParser().Parse(lexer, lexer.GetNextToken(), attributes);

            successfullyParsed &= ParseFunctionBody(lexer, lexer.GetCurrentToken(), function, attributes);
            successfullyParsed &= ParseFunctionEnd(lexer, lexer.GetCurrentToken(), function, attributes);

            environments.Pop();

            return successfullyParsed;
        }

        private Function ParseFunctionDeclaration(Lexer lexer, Token token,
                    Environment parentEnvironment, out bool successfullyParsed)
        {
            Function function = null;
            successfullyParsed = true;

            try
            {
                ValidateToken(token, TokenType.FunctionDefinition);

                var identifierToken = lexer.GetNextToken();
                ValidateToken(identifierToken, TokenType.Identifier);

                // SEM: Verificar que no exista una subrutina con el mismo nombre.
                if (parentEnvironment.Subroutines.Exists(identifierToken.Lexeme))
                {
                    LogDuplicateIdentifierFound(identifierToken.Lexeme, identifierToken);
                    successfullyParsed = false;
                }

                function = new Function(identifierToken.Lexeme);

                successfullyParsed &= ParserFactory.GetFormalParametersParser().Parse(
                            lexer, lexer.GetNextToken(), Attributes.Create(function.Environment, "ENV"));

                ValidateToken(lexer.GetNextToken(), TokenType.IdTypeSeparator);

                var returnTypeToken = lexer.GetNextToken();
                ValidateToken(returnTypeToken, TokenType.IntegerDataType, TokenType.BooleanDataType);

                function.ReturnType = returnTypeToken.Type == TokenType.BooleanDataType
                    ? DataType.Boolean
                    : DataType.Integer;

                ValidateToken(lexer.GetNextToken(), TokenType.EndOfInstruction);
            }
            catch (Exception ex)
            {
                successfullyParsed = false;
                Logger(ex.Message);
                ErrorRecovery(lexer);
            }

            return function;
        }

        private bool ParseFunctionBody(Lexer lexer, Token token, Function function, Attributes attributes)
        {
            var successfullyParsed = true;

            try
            {
                ValidateToken(token, TokenType.ScopeStart);
            }
            catch (Exception ex)
            {
                successfullyParsed = false;
                Logger(ex.Message);
            }

            var bodyAttributes = Attributes.Create(attributes["ENVS"], "ENVS");

            bodyAttributes.AddAttribute(function.Statements, "STMS");

            successfullyParsed &= ParserFactory.GetBodyParser().Parse(lexer, lexer.GetNextToken(), bodyAttributes);

            return successfullyParsed;
        }

        private bool ParseFunctionEnd(Lexer lexer, Token token, Function function, Attributes attributes)
        {
            var successfullyParsed = true;

            try
            {
                ValidateToken(token, TokenType.FunctionEnd);

                var expressionAttributes = Attributes.Create(attributes.ToArray());

                successfullyParsed &= ParserFactory.GetExpressionParser().Parse(lexer, lexer.GetNextToken(), expressionAttributes);

                if (expressionAttributes.ContainsAttribute("EXP"))
                {
                    function.ReturnExpression = expressionAttributes["EXP"] as Expression;

                    // SEM: La expresion de retorno de una funcion no puede ser nula.
                    if (function.ReturnExpression == null)
                    {
                        LogNullExpression(token, function.ReturnType);
                        successfullyParsed = false;
                    }
                    // SEM: El tipo de la expresion de retorno de una funcion debe coincidir con el tipo declarado.
                    else if (function.ReturnExpression.Type != function.ReturnType)
                    {
                        LogTypeExpressionInvalid(token, function.ReturnType, function.ReturnExpression.Type);
                        successfullyParsed = false;
                    }
                }
                // SEM: La expresion de retorno de una funcion no puede ser nula.
                else
                {
                    LogNullExpression(token, function.ReturnType);
                    successfullyParsed = false;
                }

                ValidateToken(lexer.GetCurrentToken(), TokenType.EndOfInstruction);
            }
            catch (Exception ex)
            {
                successfullyParsed = false;

                Logger(ex.Message);
                ErrorRecovery(lexer);
            }

            return successfullyParsed;
        }
    }
}
