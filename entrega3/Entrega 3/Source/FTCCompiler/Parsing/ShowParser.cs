using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Trees;
using System;
using System.Collections.Generic;

namespace FTCCompiler.Parsing
{
    class ShowParser : ParserBase, IParseable
    {
        public ShowParser(Action<string> logger) 
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;
            var printables = new List<IPrintable>();

            try
            {
                ValidateToken(token, TokenType.Write, TokenType.WriteLine);

                successfullyParsed &= ParsePrintables(lexer, attributes, printables);

                if (token.Type == TokenType.WriteLine)
                {
                    var tree = new ShowLn();

                    tree.Values = printables;

                    attributes.AddAttribute(tree, "SHOW");
                }
                else
                {
                    var tree = new Show();

                    tree.Values = printables;

                    attributes.AddAttribute(tree, "SHOW");
                }
            }
            catch (Exception ex)
            {
                successfullyParsed = false;

                Logger(ex.Message);
                ErrorRecovery(lexer);
            }

            return successfullyParsed;
        }

        private bool ParsePrintables(Lexer lexer, Attributes attributes, List<IPrintable> printables)
        {
            var successfullyParsed = true;

            try
            {
                do
                {
                    var token = lexer.GetNextToken();

                    if (!token.Is(TokenType.LiteralString))
                    {
                        successfullyParsed &= ParserFactory.GetExpressionParser().Parse(lexer, token, attributes);

                        // SEM: Verifico que la expresion no sea nula
                        if (attributes.ContainsAttribute("EXP"))
                        {
                            var exp = attributes["EXP"];

                            // SEM: Verifico que la expresion no sea nula
                            if (exp == null)
                            {
                                LogNullExpression(token);
                                successfullyParsed = false;
                            }

                            printables.Add(exp as IPrintable);

                            attributes.RemoveAttribute("EXP");
                        }
                        else
                        {
                            LogNullExpression(token);
                            successfullyParsed = false;
                        }
                    }
                    else
                    {
                        ValidateToken(lexer.GetCurrentToken(), TokenType.LiteralString);

                        printables.Add(new LiteralString(lexer.GetCurrentToken().Lexeme));

                        lexer.GetNextToken();
                    }

                } while (lexer.GetCurrentToken().Is(TokenType.ListSeparator));

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
