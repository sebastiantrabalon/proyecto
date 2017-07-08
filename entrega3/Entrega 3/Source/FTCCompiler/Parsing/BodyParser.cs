using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using System;

namespace FTCCompiler.Parsing
{
    class BodyParser : ParserBase, IParseable
    {
        public BodyParser(Action<string> logger)
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;
            var statements = attributes["STMS"] as Statements;

            while (token.Is(TokenType.Identifier) || token.Is(TokenType.Read) || token.Is(TokenType.Write) ||
                   token.Is(TokenType.WriteLine) || token.Is(TokenType.ConditionalIf) || token.Is(TokenType.IterationWhile))
            {
                try
                {
                    if (token.Is(TokenType.Identifier))
                    {
                        successfullyParsed &= ParserFactory.GetAssignmentOrCallParser().Parse(lexer, token, attributes);

                        if (attributes.ContainsAttribute("AOC"))
                        {
                            statements.AddStatement(attributes["AOC"] as Statement);
                            attributes.RemoveAttribute("AOC");
                        }
                    }
                    else if (token.Is(TokenType.Read))
                    {
                        successfullyParsed &= ParserFactory.GetReadParser().Parse(lexer, token, attributes);

                        if (attributes.ContainsAttribute("READ"))
                        {
                            statements.AddStatement(attributes["READ"] as Statement);
                            attributes.RemoveAttribute("READ");
                        }
                    }
                    else if (token.Is(TokenType.Write) || token.Is(TokenType.WriteLine))
                    {
                        successfullyParsed &= ParserFactory.GetShowParser().Parse(lexer, token, attributes);

                        if (attributes.ContainsAttribute("SHOW"))
                        {
                            statements.AddStatement(attributes["SHOW"] as Statement);
                            attributes.RemoveAttribute("SHOW");
                        }
                    }
                    else if (token.Is(TokenType.ConditionalIf))
                    {
                        successfullyParsed &= ParserFactory.GetIfParser().Parse(lexer, token, attributes);

                        if (attributes.ContainsAttribute("IF"))
                        {
                            statements.AddStatement(attributes["IF"] as Statement);
                            attributes.RemoveAttribute("IF");
                        }
                    }
                    else if (token.Is(TokenType.IterationWhile))
                    {
                        successfullyParsed &= ParserFactory.GetWhileParser().Parse(lexer, token, attributes);

                        if (attributes.ContainsAttribute("WHILE"))
                        {
                            statements.AddStatement(attributes["WHILE"] as Statement);
                            attributes.RemoveAttribute("WHILE");
                        }
                    }
                }
                catch (Exception ex)
                {
                    successfullyParsed = false;

                    Logger(ex.Message);
                    ErrorRecovery(lexer);
                }

                token = lexer.GetNextToken();
            }

            return successfullyParsed;
        }
    }
}
