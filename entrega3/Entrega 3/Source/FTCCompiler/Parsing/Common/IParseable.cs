using FTCCompiler.Common;

namespace FTCCompiler.Parsing.Common
{
    interface IParseable
    {
        bool Parse(Lexer lexer, Token token, Attributes attributes);
    }
}
