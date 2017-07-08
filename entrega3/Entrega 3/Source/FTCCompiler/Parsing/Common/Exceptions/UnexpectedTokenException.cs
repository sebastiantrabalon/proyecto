using System;
using System.Collections.Generic;
using FTCCompiler.Common;
using FTCCompiler.Resources;

namespace FTCCompiler.Parsing.Common.Exceptions
{
    class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException(Token token, params TokenType[] expectedTokens)
            : base(string.Format(Messages.UnexpectedTokenMessage, token.Line, token.Column, 
                            GetTokenLexemes(expectedTokens), token.Lexeme))
        {
        }

        public UnexpectedTokenException(Token token, string expectedValue)
            : base(string.Format(Messages.UnexpectedTokenMessage, token.Line, token.Column, 
                            string.Concat("'", expectedValue, "'"), token.Lexeme))
        {
        }

        private static string GetTokenLexemes(IEnumerable<TokenType> tokenTypes)
        {
            var lexemes = string.Empty;

            foreach (var tokenType in tokenTypes)
                lexemes +=  string.Concat("'", Lexemes.ResourceManager.GetString(tokenType.ToString()), "',");

            return lexemes.TrimEnd(',');
        }
    }
}
