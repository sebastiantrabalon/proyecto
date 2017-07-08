using System;

namespace FTCCompiler.Common
{
    static class Extensions
    {
        public static bool Contains(this string lexeme, Symbols symbol)
        {
            var symbolString = Convert.ToChar((int)symbol).ToString();

            return lexeme.Contains(symbolString);
        }

        public static bool Is(this Token token, TokenType type)
        {
            return token.Type == type;
        }
    }
}
