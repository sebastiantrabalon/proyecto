using FTCCompiler.Common;
using FTCCompiler.Parsing.Common;
using FTCCompiler.Parsing.Context;
using System;
using Environment = FTCCompiler.Parsing.Context.Environment;

namespace FTCCompiler.Parsing
{
    class ProgramParser : ParserBase, IParseable
    {
        public ProgramParser(Action<string> logger)
            : base(logger)
        {
        }

        public bool Parse(Lexer lexer, Token token, Attributes attributes)
        {
            var successfullyParsed = true;

            try
            {
                var environments = new Environments();
                
                environments.Push(new Environment() {
                    IsGlobal = true
                });

                attributes.AddAttribute(environments, "ENVS");

                successfullyParsed &= ParserFactory.GetEnvironmentParser().Parse(lexer, token, attributes);

                ValidateToken(lexer.GetCurrentToken(), TokenType.EndOfFile);
            }
            catch (Exception ex)
            {
                successfullyParsed = false;
                Logger(ex.Message);
            }

            return successfullyParsed;
        }
    }
}
