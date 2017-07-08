using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using FTCCompiler.Parsing.Trees;
using System;
using Environment = FTCCompiler.Parsing.Context.Environment;

namespace FTCCompiler.Parsing
{
    class ProcedureParser : ParserBase, IParseable
    {
        public ProcedureParser(Action<string> logger)
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            var environments = ((Environments) attributes["ENVS"]);

            var procedure = ParseProcedureDeclaration(lexer, token, environments[0], out successfullyParsed);

            environments[0].Subroutines.AddSubroutine(procedure);

            var globalEnvironmentElementsCount = environments[0].Locals.Count() + 
                environments[0].Subroutines.Count();

            if (procedure.Identifier == "MAIN")
            {
                // SEM: Main no puede tener parametros
                if (procedure.Environment.FormalParameters.HasParameters())
                {
                    LogMainHasParameters();
                    successfullyParsed = false;
                }

                procedure.Environment = environments[0];
            }

            environments.Push(procedure.Environment);

            successfullyParsed &= ParserFactory.GetEnvironmentParser().Parse(lexer, lexer.GetNextToken(), attributes);

            // SEM: Main no puede tener entorno local
            if (globalEnvironmentElementsCount < (environments[0].Locals.Count() + environments[0].Subroutines.Count()))
            {
                LogMainHasLocalEnvironment();
                successfullyParsed = false;
            }

            successfullyParsed &= ParseProcedureBody(lexer, lexer.GetCurrentToken(), procedure, attributes);
            successfullyParsed &= ParseProcedureEnd(lexer, lexer.GetCurrentToken());

            environments.Pop();

            return successfullyParsed;
        }

        private Procedure ParseProcedureDeclaration(Lexer lexer, Token token, 
                            Environment parentEnvironment, out bool successfullyParsed)
        {
            Procedure procedure = null;
            successfullyParsed = true;

            try
            {
                ValidateToken(token, TokenType.ProcedureDefinition);

                var identifierToken = lexer.GetNextToken();
                ValidateToken(identifierToken, TokenType.Identifier);

                // SEM: Verificar que no exista una subrutina con el mismo nombre.
                if (parentEnvironment.Subroutines.Exists(identifierToken.Lexeme))
                {
                    LogDuplicateIdentifierFound(identifierToken.Lexeme, identifierToken);
                    successfullyParsed = false;
                }

                procedure = new Procedure(identifierToken.Lexeme);

                successfullyParsed &= ParserFactory.GetFormalParametersParser().Parse(
                            lexer, lexer.GetNextToken(), Attributes.Create(procedure.Environment, "ENV"));

                ValidateToken(lexer.GetNextToken(), TokenType.EndOfInstruction);
            }
            catch (Exception ex)
            {
                successfullyParsed = false;

                Logger(ex.Message);
                ErrorRecovery(lexer);
            }
            
            return procedure;
        }

        private bool ParseProcedureBody(Lexer lexer, Token token, Procedure procedure, Attributes attributes)
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

            bodyAttributes.AddAttribute(procedure.Statements, "STMS");

            successfullyParsed &= ParserFactory.GetBodyParser().Parse(lexer, lexer.GetNextToken(), bodyAttributes);

            return successfullyParsed;
        }

        private bool ParseProcedureEnd(Lexer lexer, Token token)
        {
            try
            {
                ValidateToken(token, TokenType.ProcedureEnd);
                ValidateToken(lexer.GetNextToken(), TokenType.EndOfInstruction);

                return true;
            }
            catch (Exception ex)
            {
                Logger(ex.Message);
                ErrorRecovery(lexer);

                return false;
            }
        }
    }
}
