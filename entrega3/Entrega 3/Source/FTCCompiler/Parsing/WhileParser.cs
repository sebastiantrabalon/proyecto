using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Trees;
using System;
using System.Linq;

namespace FTCCompiler.Parsing
{
    class WhileParser : ParserBase, IParseable
    {
        public WhileParser(Action<string> logger) 
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            try
            {
                var tree = new While();

                ValidateToken(token, TokenType.IterationWhile);

                var expressionAttributes = Attributes.Create(attributes.ToArray());

                successfullyParsed &= ParserFactory.GetExpressionParser().Parse(lexer, lexer.GetNextToken(), expressionAttributes);

                // SEM: La condición en un WHILE no puede ser una expresión nula.
                if (expressionAttributes.ContainsAttribute("EXP"))
                {
                    tree.Condition = expressionAttributes["EXP"] as Expression;

                    // SEM: La condición en un WHILE no puede ser una expresión nula.
                    if (tree.Condition != null)
                    {
                        // SEM: La condición en un WHILE sólo puede ser una expresión booleana
                        if (tree.Condition.Type != DataType.Boolean)
                        {
                            LogTypeExpressionInvalid(token, DataType.Boolean, DataType.Integer);
                            successfullyParsed = false;
                        }
                    }
                    else
                    {
                        LogNullExpression(token, DataType.Boolean);
                        successfullyParsed = false;
                    }
                }
                else
                {
                    LogNullExpression(token, DataType.Boolean);
                    successfullyParsed = false;
                }
                
                ValidateToken(lexer.GetCurrentToken(), TokenType.IterationDo);

                var bodyAttributes = Attributes.Create(attributes["ENVS"], "ENVS");

                bodyAttributes.AddAttribute(tree.Statements, "STMS");

                successfullyParsed &= ParserFactory.GetBodyParser().Parse(lexer, lexer.GetNextToken(), bodyAttributes);

                ValidateToken(lexer.GetCurrentToken(), TokenType.IterationEnd);
                ValidateToken(lexer.GetNextToken(), TokenType.EndOfInstruction);

                attributes.AddAttribute(tree, "WHILE");
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
