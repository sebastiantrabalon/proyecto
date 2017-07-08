﻿using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using FTCCompiler.Parsing.Trees;
using System;
using System.Linq;

namespace FTCCompiler.Parsing
{
    class AssignmentOrCallParser : ParserBase, IParseable
    {
        public AssignmentOrCallParser(Action<string> logger)
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            try
            {
                ValidateToken(token, TokenType.Identifier);
                var identifier = token.Lexeme;
                
                token = lexer.GetNextToken();
                ValidateToken(token, TokenType.LeftSquareBracket, TokenType.LeftParenthesis, TokenType.AssignOperator);

                switch (token.Type)
                {
                    case TokenType.LeftSquareBracket: //Es un array
                        successfullyParsed &= ParseAssignmentToArray(identifier, lexer, attributes);
                        break;
                    case TokenType.LeftParenthesis: //Es una llamada a funcion o procedimiento
                        successfullyParsed &= ParseProcedureCall(identifier, lexer, attributes);
                        break;
                    default: //Es la asignacion de una expresion
                        successfullyParsed &= ParseAssignment(identifier, lexer, attributes);
                        break;
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

        private bool ParseAssignmentToArray(string identifier, Lexer lexer, Attributes attributes)
        {
            var tree = new Assignment();
            Expression posInArray;

            var expressionParser = ParserFactory.GetExpressionParser() as ExpressionParser;

            var successfullyParsed = expressionParser.ParsePositionInArray(lexer, attributes, identifier, out posInArray);

            ValidateToken(lexer.GetCurrentToken(), TokenType.RightSquareBracket);

            tree.Destination = posInArray;

            successfullyParsed &= ParseAssignedExpression(lexer, lexer.GetNextToken(), attributes, tree);

            attributes.AddAttribute(tree, "AOC");

            return successfullyParsed;
        }

        private bool ParseProcedureCall(string identifier, Lexer lexer, Attributes attributes)
        {
            var successfullyParsed = true;
            var procedureCallToken = lexer.GetCurrentToken();
            var environments = attributes["ENVS"] as Environments;

            //SEM: Validar que la subrutina exista en el scope y sea un procedimiento
            if (!environments.ExistsProcedure(identifier))
            {
                LogProcedureNotFound(lexer.GetCurrentToken(), identifier);
                successfullyParsed = false;
            }

            var procedureCallTree = new ProcedureCall();
            procedureCallTree.Identifier = identifier;
            procedureCallTree.Reference = environments.FindProcedure(identifier);

            attributes.AddAttribute(procedureCallTree.ActualParameters, "PARMS");
            
            successfullyParsed &= ParserFactory.GetActualParametersParser().Parse(lexer, lexer.GetNextToken(), attributes);

            var procedure = environments.GetSubroutine(procedureCallTree.Reference);

            if (procedure != null)
            {
                // SEM: Verifico que la cantidad y el tipo de los parametros usados en la llamada coincidan con los de la 
                // definición de la funcion.
                if (!FormalParametersParser.ValidateFormalParametersVsActualParameters(
                    procedure.Environment.FormalParameters.ToList(), procedureCallTree.ActualParameters.Parameters))
                {
                    LogInvalidParametersCountOrType(identifier, procedureCallToken);
                    successfullyParsed = false;
                }

                // SEM: Verifico que no se pasen arrys completos como parametro de un procedimiento.
                if (ActualParametersParser.VerifyIfArraysArePassedAsParameters(environments,
                        procedureCallTree.ActualParameters.Parameters))
                {
                    LogArrayCantBeSubroutineParameter(procedureCallToken, procedure.Identifier);
                    successfullyParsed = false;
                }
            }

            attributes.RemoveAttribute("PARMS");

            ValidateToken(lexer.GetCurrentToken(), TokenType.RightParenthesis);
            ValidateToken(lexer.GetNextToken(), TokenType.EndOfInstruction);

            attributes.AddAttribute(procedureCallTree, "AOC");

            return successfullyParsed;
        }

        private bool ParseAssignment(string identifier, Lexer lexer, Attributes attributes)
        {
            var successfullyParsed = true;
            var environments = attributes["ENVS"] as Environments;

            //SEM: Validar que el asignable que sea variable (no constante)
            if (environments.ExistsConstant(identifier))
            {
                LogConstantIsNotAssignable(lexer.GetCurrentToken());
                successfullyParsed = false;
            }
            else
            {
                //SEM: Validar que el asignable exista en el scope
                if (!environments.ExistsVariable(identifier) && !environments[0].ExistsParameter(identifier))
                {
                    LogIdentifierNotFound(identifier, lexer.GetCurrentToken());
                    successfullyParsed = false;
                }
            }

            var tree = new Assignment();
            var identifierExpression = new IdentifierExpression();

            identifierExpression.Name = identifier;

            if (environments.ExistsVariable(identifier))
            {
                var variableRefence = environments.FindVariable(identifier);
                var variable = environments.GetLocal(variableRefence);

                identifierExpression.Reference = variableRefence;

                if (variable != null)
                    identifierExpression.Type = variable.Type;
            }
            else
            {
                var parameterReference = environments[0].FindParameter(identifier);
                var parameter = environments[0].GetParameter(parameterReference);

                identifierExpression.Reference = parameterReference;

                if (parameter != null)
                    identifierExpression.Type = parameter.DataType;
            }

            tree.Destination = identifierExpression;

            successfullyParsed &= ParseAssignedExpression(lexer, lexer.GetCurrentToken(), attributes, tree);

            attributes.AddAttribute(tree, "AOC");

            return successfullyParsed;
        }

        private bool ParseAssignedExpression(Lexer lexer, Token token, Attributes attributes, Assignment tree)
        {
            var successfullyParsed = true;

            ValidateToken(token, TokenType.AssignOperator);

            successfullyParsed &= ParserFactory.GetExpressionParser().Parse(lexer, lexer.GetNextToken(), attributes);

            if (attributes.ContainsAttribute("EXP"))
            {
                tree.Expression = attributes["EXP"] as Expression;
                attributes.RemoveAttribute("EXP");

                //SEM: Validar que el tipo de asignable coincida con el tipo de expresión a asignar
                if (tree.Destination != null && tree.Expression != null)
                {
                    if (tree.Destination.Type != tree.Expression.Type)
                    {
                        successfullyParsed = false;
                        LogTypeExpressionInvalid(lexer.GetCurrentToken(), tree.Destination.Type, tree.Expression.Type);
                    }
                }
            }
            return successfullyParsed;
        }
    }
}