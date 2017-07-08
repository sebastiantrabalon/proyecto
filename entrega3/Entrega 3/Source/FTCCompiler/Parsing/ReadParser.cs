using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Trees;
using System;

namespace FTCCompiler.Parsing
{
    class ReadParser : ParserBase, IParseable
    {
        public ReadParser(Action<string> logger)
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;
            var tree = new Read();

            try
            {
                ValidateToken(token, TokenType.Read);

                token = lexer.GetNextToken();
                successfullyParsed &= ParserFactory.GetExpressionParser().Parse(lexer, token, attributes);

                // SEM: Verifico que la expresion no sea nula
                if (attributes.ContainsAttribute("EXP"))
                {
                    var exp = attributes["EXP"] as Expression;

                    // SEM: Verifico que la expresion no sea nula
                    if (exp != null)
                    {
                        // SEM: Verifico que el tipo de la expresion sea un asignable.
                        if (!(exp is IdentifierExpression) && !(exp is PositionInArrayExpression))
                        {
                            LogInvalidAssignableType(token);
                            successfullyParsed = false;
                        }
                        else
                        {
                            tree.Destination = exp;
                            attributes.AddAttribute(tree, "READ");
                        }
                    }
                }
                else
                {
                    LogNullExpression(token);
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
