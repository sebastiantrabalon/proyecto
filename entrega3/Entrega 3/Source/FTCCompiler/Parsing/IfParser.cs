using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using FTCCompiler.Parsing.Trees;
using System;
using System.Linq;

namespace FTCCompiler.Parsing
{
    class IfParser : ParserBase, IParseable
    {
        public IfParser(Action<string> logger) : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;
            Expression condition = null;

            try
            {
                ValidateToken(token, TokenType.ConditionalIf);

                var expressionAttributes = Attributes.Create(attributes.ToArray());

                successfullyParsed &= ParserFactory.GetExpressionParser().Parse(lexer, lexer.GetNextToken(), expressionAttributes);

                // SEM: La condición en un IF no puede ser una expresión nula.
                if (expressionAttributes.ContainsAttribute("EXP"))
                {
                    condition = expressionAttributes["EXP"] as Expression;

                    // SEM: La condición en un IF no puede ser una expresión nula.
                    if (condition != null)
                    {
                        // SEM: La condición en un IF sólo puede ser una expresión booleana
                        if (condition.Type != DataType.Boolean)
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

                ValidateToken(lexer.GetCurrentToken(), TokenType.ConditionalThen);

                var thenBodyAttributes = Attributes.Create(attributes["ENVS"], "ENVS");
                var thenStatements = new Statements();

                thenBodyAttributes.AddAttribute(thenStatements, "STMS");

                successfullyParsed &= ParserFactory.GetBodyParser().Parse(lexer, lexer.GetNextToken(), thenBodyAttributes);

                token = lexer.GetCurrentToken();

                if (token.Is(TokenType.ConditionalEnd))
                {
                    ValidateToken(lexer.GetNextToken(), TokenType.EndOfInstruction);

                    var tree = new IfThen();

                    tree.Condition = condition;
                    tree.Statements = thenStatements;

                    attributes.AddAttribute(tree, "IF");
                }
                else if (token.Is(TokenType.ConditionalElse))
                {
                    var elseBodyAttributes = Attributes.Create(attributes["ENVS"], "ENVS");
                    var elseStatements = new Statements();

                    elseBodyAttributes.AddAttribute(elseStatements, "STMS");

                    successfullyParsed &= ParserFactory.GetBodyParser().Parse(lexer, lexer.GetNextToken(), elseBodyAttributes);

                    ValidateToken(lexer.GetCurrentToken(), TokenType.ConditionalEnd);
                    ValidateToken(lexer.GetNextToken(), TokenType.EndOfInstruction);

                    var tree = new IfThenElse();

                    tree.Condition = condition;
                    tree.Then = thenStatements;
                    tree.Else = elseStatements;

                    attributes.AddAttribute(tree, "IF");
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
    }
}
