using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using FTCCompiler.Parsing.Trees;
using System;
using System.Linq;

namespace FTCCompiler.Parsing
{
    class ExpressionParser : ParserBase, IParseable
    {
        public ExpressionParser(Action<string> logger)
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            return ParseXorIfExists(lexer, token, attributes);
        }

        private bool ParseXorIfExists(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            successfullyParsed &= ParseOrIfExists(lexer, token, attributes);

            if (lexer.GetCurrentToken().Is(TokenType.XorOperator))
            {
                var retTree = attributes["EXP"] as Expression;

                // SEM: Sólo puede aplicarse un XOR entre booleanos
                if (retTree.Type != DataType.Boolean)
                {
                    LogTypeExpressionInvalid(lexer.GetCurrentToken(), DataType.Boolean, retTree.Type);
                    successfullyParsed = false;
                }

                do
                {
                    var tree = new XorExpression();

                    tree.LeftOperand = retTree;
                    successfullyParsed &= ParseOrIfExists(lexer, lexer.GetNextToken(), attributes);
                    tree.RightOperand = attributes["EXP"] as Expression;

                    // SEM: Sólo puede aplicarse un XOR entre booleanos
                    if (tree.RightOperand.Type != DataType.Boolean)
                    {
                        LogTypeExpressionInvalid(lexer.GetCurrentToken(), DataType.Boolean, tree.RightOperand.Type);
                        successfullyParsed = false;
                    }

                    retTree = tree;

                } while (lexer.GetCurrentToken().Is(TokenType.XorOperator));

                attributes["EXP"] = retTree;
            }

            return successfullyParsed;
        }

        private bool ParseOrIfExists(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            successfullyParsed &= ParseAndIfExists(lexer, token, attributes);

            if (lexer.GetCurrentToken().Is(TokenType.OrOperator))
            {
                var retTree = attributes["EXP"] as Expression;

                // SEM: Sólo puede aplicarse un OR entre booleanos
                if (retTree.Type != DataType.Boolean)
                {
                    LogTypeExpressionInvalid(lexer.GetCurrentToken(), DataType.Boolean, retTree.Type);
                    successfullyParsed = false;
                }

                do
                {
                    var tree = new OrExpression();

                    tree.LeftOperand = retTree;
                    successfullyParsed &= ParseAndIfExists(lexer, lexer.GetNextToken(), attributes);
                    tree.RightOperand = attributes["EXP"] as Expression;

                    // SEM: Sólo puede aplicarse un AND entre booleanos
                    if (tree.RightOperand.Type != DataType.Boolean)
                    {
                        LogTypeExpressionInvalid(lexer.GetCurrentToken(), DataType.Boolean, tree.RightOperand.Type);
                        successfullyParsed = false;
                    }

                    retTree = tree;

                } while (lexer.GetCurrentToken().Is(TokenType.OrOperator));

                attributes["EXP"] = retTree;
            }

            return successfullyParsed;
        }

        private bool ParseAndIfExists(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            successfullyParsed &= ParseRelationalOperatorIfExists(lexer, token, attributes);

            if (lexer.GetCurrentToken().Is(TokenType.AndOperator))
            {
                var retTree = attributes["EXP"] as Expression;

                // SEM: Sólo puede aplicarse un AND entre booleanos
                if (retTree.Type != DataType.Boolean)
                {
                    LogTypeExpressionInvalid(lexer.GetCurrentToken(), DataType.Boolean, retTree.Type);
                    successfullyParsed = false;
                }

                do
                {
                    var tree = new AndExpression();

                    tree.LeftOperand = retTree;
                    successfullyParsed &= ParseRelationalOperatorIfExists(lexer, lexer.GetNextToken(), attributes);
                    tree.RightOperand = attributes["EXP"] as Expression;

                    // SEM: Sólo puede aplicarse un AND entre booleanos
                    if (tree.RightOperand.Type != DataType.Boolean)
                    {
                        LogTypeExpressionInvalid(lexer.GetCurrentToken(), DataType.Boolean, tree.RightOperand.Type);
                        successfullyParsed = false;
                    }

                    retTree = tree;

                } while (lexer.GetCurrentToken().Is(TokenType.AndOperator));

                attributes["EXP"] = retTree;
            }

            return successfullyParsed;
        }

        private bool ParseRelationalOperatorIfExists(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            successfullyParsed &= ParseAdditionOrSubstractionIfExists(lexer, token, attributes);

            if (lexer.GetCurrentToken().Is(TokenType.EqualOperator) ||
                lexer.GetCurrentToken().Is(TokenType.GreaterThanOperator) ||
                lexer.GetCurrentToken().Is(TokenType.GreaterThanOrEqualOperator) ||
                lexer.GetCurrentToken().Is(TokenType.LessThanOperator) ||
                lexer.GetCurrentToken().Is(TokenType.LessThanOrEqualOperator) ||
                lexer.GetCurrentToken().Is(TokenType.DistinctOperator))
            {
                BinaryExpression tree;

                if (lexer.GetCurrentToken().Is(TokenType.EqualOperator))
                    tree = new EqualExpression();
                else if (lexer.GetCurrentToken().Is(TokenType.GreaterThanOperator))
                    tree = new GreaterThanExpression();
                else if (lexer.GetCurrentToken().Is(TokenType.GreaterThanOrEqualOperator))
                    tree = new GreaterThanOrEqualExpression();
                else if (lexer.GetCurrentToken().Is(TokenType.LessThanOperator))
                    tree = new LessThanExpression();
                else if (lexer.GetCurrentToken().Is(TokenType.LessThanOrEqualOperator))
                    tree = new LessThanOrEqualExpression();
                else
                    tree = new DistinctExpression();

                tree.LeftOperand = attributes["EXP"] as Expression;
                successfullyParsed &= ParseAdditionOrSubstractionIfExists(lexer, lexer.GetNextToken(), attributes);
                tree.RightOperand = attributes["EXP"] as Expression;

                // SEM: Sólo pueden compararse expresiones del mismo tipo
                if (tree.LeftOperand.Type != tree.RightOperand.Type)
                {
                    LogTypeMismatch(lexer.GetCurrentToken(), tree.LeftOperand.Type, tree.RightOperand.Type);
                    successfullyParsed = false;
                } 
                    // SEM: Las comparaciones de tipo >, >=, <, <= sólo pueden realizarse entre enteros
                else if (!lexer.GetCurrentToken().Is(TokenType.EqualOperator) && 
                    !lexer.GetCurrentToken().Is(TokenType.DistinctOperator) && tree.LeftOperand.Type != DataType.Integer)
                {
                    LogTypeExpressionInvalid(lexer.GetCurrentToken(), DataType.Integer, tree.LeftOperand.Type);
                    successfullyParsed = false;
                }

                attributes["EXP"] = tree;
            }

            return successfullyParsed;
        }

        private bool ParseAdditionOrSubstractionIfExists(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            successfullyParsed &= ParseMultiplicationOrDivisionIfExists(lexer, token, attributes);

            if (lexer.GetCurrentToken().Is(TokenType.AdditionOperator) ||
                lexer.GetCurrentToken().Is(TokenType.SubstractionOperator))
            {
                var retTree = attributes["EXP"] as Expression;

                // SEM: Sólo puede sumarse o restarse entre enteros
                if (retTree.Type != DataType.Integer)
                {
                    LogTypeExpressionInvalid(lexer.GetCurrentToken(), DataType.Integer, retTree.Type);
                    successfullyParsed = false;
                }

                do
                {
                    var tree = lexer.GetCurrentToken().Is(TokenType.AdditionOperator) ?
                            (BinaryExpression)new AdditionExpression() :
                            (BinaryExpression)new SubstractionExpression();

                    tree.LeftOperand = retTree;

                    successfullyParsed &= ParseMultiplicationOrDivisionIfExists(lexer, lexer.GetNextToken(), attributes);
                    tree.RightOperand = attributes["EXP"] as Expression;

                    // SEM: Sólo puede sumarse o restarse entre enteros
                    if (tree.RightOperand.Type != DataType.Integer)
                    {
                        LogTypeExpressionInvalid(lexer.GetCurrentToken(), DataType.Integer, tree.RightOperand.Type);
                        successfullyParsed = false;
                    }

                    retTree = tree;

                } while (lexer.GetCurrentToken().Is(TokenType.AdditionOperator) ||
                         lexer.GetCurrentToken().Is(TokenType.SubstractionOperator));

                attributes["EXP"] = retTree;
            }

            return successfullyParsed;
        }

        private bool ParseMultiplicationOrDivisionIfExists(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            successfullyParsed &= ParseOperand(lexer, token, attributes);

            if (lexer.GetCurrentToken().Is(TokenType.MultiplicationOperator) ||
                lexer.GetCurrentToken().Is(TokenType.DivisionOperator))
            {
                var retTree = attributes["EXP"] as Expression;

                // SEM: Sólo puede multiplicarse o dividirse entre enteros
                if (retTree.Type != DataType.Integer)
                {
                    LogTypeExpressionInvalid(lexer.GetCurrentToken(), DataType.Integer, retTree.Type);
                    successfullyParsed = false;
                }

                do
                {
                    var tree = lexer.GetCurrentToken().Is(TokenType.MultiplicationOperator) ?
                            (BinaryExpression)new MultiplicationExpression() :
                            (BinaryExpression)new DivisionExpression();

                    tree.LeftOperand = retTree;

                    successfullyParsed &= ParseOperand(lexer, lexer.GetNextToken(), attributes);
                    tree.RightOperand = attributes["EXP"] as Expression;

                    // SEM: Sólo puede multiplicarse o dividirse entre enteros
                    if (tree.RightOperand.Type != DataType.Integer)
                    {
                        LogTypeExpressionInvalid(lexer.GetCurrentToken(), DataType.Integer, tree.RightOperand.Type);
                        successfullyParsed = false;
                    }

                    retTree = tree;

                } while (lexer.GetCurrentToken().Is(TokenType.MultiplicationOperator) ||
                         lexer.GetCurrentToken().Is(TokenType.DivisionOperator));

                attributes["EXP"] = retTree;
            }

            return successfullyParsed;
        }

        private bool ParseOperand(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            if (token.Is(TokenType.LeftParenthesis))
            {
                successfullyParsed &= Parse(lexer, lexer.GetNextToken(), attributes);
                ValidateToken(lexer.GetCurrentToken(), TokenType.RightParenthesis);
            }
            else
            {
                Expression tree = null;

                if (token.Is(TokenType.LiteralNumber))
                {
                    tree = new LiteralNumberExpression(Convert.ToInt16(token.Lexeme));
                }
                else if (token.Is(TokenType.LiteralBoolean))
                {
                    tree = new LiteralBooleanExpression(Convert.ToBoolean(token.Lexeme));
                }
                else if (token.Is(TokenType.Identifier))
                {
                    var identifierName = token.Lexeme;

                    token = lexer.GetNextToken();

                    if (token.Is(TokenType.LeftSquareBracket))
                    {
                        successfullyParsed &= ParsePositionInArray(lexer, attributes, identifierName, out tree);
                    }
                    else if (token.Is(TokenType.LeftParenthesis))
                    {
                        successfullyParsed &= ParseFunctionCall(lexer, attributes, identifierName, out tree);
                    }
                    else
                    {
                        successfullyParsed &= ParseVariableOrConstantOrParameter(lexer, attributes, identifierName, out tree);
                        UpdateExpressionAttribute(attributes, tree);

                        return successfullyParsed;
                    }
                }

                UpdateExpressionAttribute(attributes, tree);
            }

            lexer.GetNextToken();

            return successfullyParsed;
        }

        private bool ParseVariableOrConstantOrParameter(Lexer lexer, Attributes attributes, string identifierName, out Expression tree)
        {
            var environments = attributes["ENVS"] as Environments;

            // SEM: Verifico que exista un identificador en los entornos.
            if (environments.ExistsLocal(identifierName))
            {
                var localReference = environments.FindLocal(identifierName);
                var local = environments.GetLocal(localReference);

                if (local != null)
                {
                    // Las constantes se traducen directamente a los literales correspondientes.
                    if (local.IsConstant)
                    {
                        if (local.Type == DataType.Integer)
                            tree = new LiteralNumberExpression(Convert.ToInt16(local.Value));
                        else
                            tree = new LiteralBooleanExpression(Convert.ToBoolean(local.Value));
                    }
                    else
                    {
                        var identifierExpression = new IdentifierExpression();

                        identifierExpression.Name = identifierName;
                        identifierExpression.Reference = localReference;
                        identifierExpression.Type = local.Type;

                        tree = identifierExpression;
                    }
                }
                else
                {
                    tree = null;
                }
            }
            else if (environments[0].ExistsParameter(identifierName))
            {
                var parameterReference = environments[0].FindParameter(identifierName);
                var parameter = environments[0].GetParameter(parameterReference);

                if (parameter != null)
                {
                    var identifierExpression = new IdentifierExpression();

                    identifierExpression.Name = identifierName;
                    identifierExpression.Reference = parameterReference;
                    identifierExpression.Type = parameter.DataType;

                    tree = identifierExpression;
                }
                else
                {
                    tree = null;
                }
            }
            else
            {
                LogIdentifierNotFound(identifierName, lexer.GetCurrentToken());

                tree = null;

                return false;
            }

            return true;
        }

        private bool ParseFunctionCall(Lexer lexer, Attributes attributes, string identifierName, out Expression tree)
        {
            var successfullyParsed = true;
            var functionCall = new FunctionCallExpression();
            var environments = attributes["ENVS"] as Environments;
            var functionCallToken = lexer.GetCurrentToken();

            // SEM: Verifico que exista una funcion con el identificador indicado.
            if (!environments.ExistsFunction(identifierName))
            {
                LogFunctionNotFound(lexer.GetCurrentToken(), identifierName);
                successfullyParsed = false;
            }

            functionCall.Identifier = identifierName;
            functionCall.Reference = environments.FindFunction(identifierName);

            attributes.AddAttribute(functionCall.ActualParameters, "PARMS");

            successfullyParsed &= ParserFactory.GetActualParametersParser().Parse(lexer, lexer.GetNextToken(), attributes);

            var function = environments.GetSubroutine(functionCall.Reference) as Function;

            if (function != null)
            {
                // SEM: Verifico que la cantidad y el tipo de los parametros usados en la llamada coincidan con los de la 
                // definición de la funcion.
                if (!FormalParametersParser.ValidateFormalParametersVsActualParameters(
                    function.Environment.FormalParameters.ToList(), functionCall.ActualParameters.Parameters))
                {
                    LogInvalidParametersCountOrType(identifierName, functionCallToken);
                    successfullyParsed = false;
                }

                // SEM: Verifico que no se pasen arrays completos como parametro de una funcion.
                if (ActualParametersParser.VerifyIfArraysArePassedAsParameters(environments,
                        functionCall.ActualParameters.Parameters))
                {
                    LogArrayCantBeSubroutineParameter(functionCallToken, functionCall.Identifier);
                    successfullyParsed = false;
                }

                functionCall.Type = function.ReturnType;
            }

            attributes.RemoveAttribute("PARMS");

            ValidateToken(lexer.GetCurrentToken(), TokenType.RightParenthesis);

            tree = functionCall;

            return successfullyParsed;
        }

        public bool ParsePositionInArray(Lexer lexer, Attributes attributes, string identifierName, out Expression tree)
        {
            var successfullyParsed = true;
            var posInArray = new PositionInArrayExpression();
            var environments = attributes["ENVS"] as Environments;

            // SEM: Verifico que exista un array con el identificador indicado.
            if (!environments.ExistsArray(identifierName))
            {
                LogIdentifierNotFound(identifierName, lexer.GetCurrentToken());
                successfullyParsed = false;
            }

            posInArray.Identifier = identifierName;
            posInArray.Reference = environments.FindArray(identifierName);

            var array = environments.GetLocal(posInArray.Reference);

            if (array != null)
                posInArray.Type = array.Type;

            successfullyParsed &= Parse(lexer, lexer.GetNextToken(), attributes);

            // SEM: Verifico que el indice de un array sea entero.
            if (attributes.ContainsAttribute("EXP"))
            {
                posInArray.Position = attributes["EXP"] as Expression;

                if (posInArray.Position.Type != DataType.Integer)
                {
                    LogInvalidArrayIndex(lexer.GetCurrentToken());
                    successfullyParsed = false;
                }
            }
            else
            {
                LogInvalidArrayIndex(lexer.GetCurrentToken());
                successfullyParsed = false;
            }

            ValidateToken(lexer.GetCurrentToken(), TokenType.RightSquareBracket);

            tree = posInArray;

            return successfullyParsed;
        }

        private static void UpdateExpressionAttribute(Attributes attributes, Expression tree)
        {
            if (attributes.ContainsAttribute("EXP"))
                attributes["EXP"] = tree;
            else
                attributes.AddAttribute(tree, "EXP");
        }
    }
}
