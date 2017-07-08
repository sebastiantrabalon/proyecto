using FTCCompiler.Common;
using FTCCompiler.Parsing.Common.Exceptions;
using FTCCompiler.Resources;
using System;

namespace FTCCompiler.Parsing.Common
{
    class ParserBase
    {
        protected readonly Action<string> Logger;

        public ParserBase(Action<string> logger)
        {
            Logger = logger;
        }

        protected void ErrorRecovery(Lexer lexer)
        {
            var token = lexer.GetNextToken();
            
            while (!token.Is(TokenType.EndOfInstruction) && !token.Is(TokenType.EndOfFile))
                token = lexer.GetNextToken();
        }

        protected void ValidateToken(Token token, params TokenType[] expectedTokens)
        {
            foreach (var expectedToken in expectedTokens)
            {
                if (token.Is(expectedToken))
                    return;
            }

            throw new UnexpectedTokenException(token, expectedTokens);
        }

        protected void LogIdentifierNotFound(string identifier, Token closestToken)
        {
            Logger(string.Format(Messages.IdentifierNotFoundMessage, closestToken.Line, closestToken.Column, identifier));
        }
        
        protected void LogInvalidParametersCountOrType(string identifier, Token closestToken)
        {
            Logger(string.Format(Messages.InvalidParametersCountOrTypeMessage, closestToken.Line, closestToken.Column, identifier));
        }

        protected void LogDuplicateIdentifierFound(string identifier, Token closestToken)
        {
            Logger(string.Format(Messages.DuplicateIdentifierFoundMessage, closestToken.Line, closestToken.Column, identifier));
        }

        protected void LogInvalidArrayIndex(Token closestToken)
        {
            Logger(string.Format(Messages.InvalidArrayIndexMessage, closestToken.Line, closestToken.Column));
        }

        protected void LogTypeExpressionInvalid(Token closestToken, DataType expected, DataType received)
        {
            Logger(string.Format(Messages.InvalidExpressionTypeMessage, closestToken.Line, closestToken.Column, expected, received));
        }

        protected void LogNullExpression(Token closestToken, DataType expected)
        {
            Logger(string.Format(Messages.InvalidExpressionTypeMessage, closestToken.Line, closestToken.Column, expected, "NULL"));
        }

        protected void LogNullExpression(Token closestToken)
        {
            Logger(string.Format(Messages.ExpressionIsNullMessage, closestToken.Line, closestToken.Column));
        }

        protected void LogTypePrintableInvalid(Token closestToken, string received)
        {
            Logger(string.Format(Messages.InvalidPrintableTypeMessage, closestToken.Line, closestToken.Column, received));
        }

        protected void LogTypeMismatch(Token closestToken, DataType type1, DataType type2)
        {
            Logger(string.Format(Messages.TypeMismatchMessage, closestToken.Line, closestToken.Column, type1, type2));
        }

        protected void LogConstantIsNotAssignable(Token closestToken)
        {
            Logger(string.Format(Messages.ConstantIsNotAssignableMessage, closestToken.Line, closestToken.Column));
        }

        protected void LogArrayCantBeSubroutineParameter(Token closestToken, string subroutineName)
        {
            Logger(string.Format(Messages.ArrayCantBeSubroutineParameterMessage, closestToken.Line, closestToken.Column, subroutineName));
        }

        protected void LogInvalidAssignableType(Token closestToken)
        {
            Logger(string.Format(Messages.InvalidAssignableTypeMessage, closestToken.Line, closestToken.Column));
        }

        protected void LogArrayCantBeDeclaredOutsideGlobalEnvironment(Token closestToken)
        {
            Logger(string.Format(Messages.ArrayCantBeDeclaredOutsideGlobalEnvironmentMessage, closestToken.Line, closestToken.Column));
        }

        protected void LogFunctionNotFound(Token closestToken, string functionName)
        {
            Logger(string.Format(Messages.FunctionNotFoundMessage, closestToken.Line, closestToken.Column, functionName));
        }

        protected void LogProcedureNotFound(Token closestToken, string procedureName)
        {
            Logger(string.Format(Messages.ProcedureNotFoundMessage, closestToken.Line, closestToken.Column, procedureName));
        }

        protected void LogMainHasParameters()
        {
            Logger(Messages.MainHasParametersMessage);
        }

        protected void LogMainHasLocalEnvironment()
        {
            Logger(Messages.MainHasLocalEnvironmentMessage);
        }
    }
}
