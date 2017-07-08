using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using FTCCompiler.Parsing.Trees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FTCCompiler.Parsing
{
    class ActualParametersParser : ParserBase, IParseable
    {
        public ActualParametersParser(Action<string> logger) 
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;
            var firstParameterRead = false;

            var parameters = attributes["PARMS"] as ActualParameters;

            if (!token.Is(TokenType.RightParenthesis))
            {
                do
                {
                    if (firstParameterRead && token.Is(TokenType.ListSeparator))
                        token = lexer.GetNextToken();

                    successfullyParsed &= ParserFactory.GetExpressionParser().Parse(lexer, token, attributes);

                    if (attributes.ContainsAttribute("EXP"))
                    {
                        var parameter = attributes["EXP"] as Expression;

                        if (parameter != null)
                            parameters.AddParameter(parameter);

                        attributes.RemoveAttribute("EXP");
                    }

                    token = lexer.GetCurrentToken();
                    firstParameterRead = true;

                } while (token.Is(TokenType.ListSeparator));
            }

            return successfullyParsed;
        }

        public static bool VerifyIfArraysArePassedAsParameters(Environments environments, IList<Expression> actualParameters)
        {
            return actualParameters.OfType<IdentifierExpression>().Any(x =>
            {
                var identifier = environments.GetLocal(x.Reference);

                if (identifier != null && identifier.IsArray)
                    return true;

                return false;
            });
        }
    }
}
