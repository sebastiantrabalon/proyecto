
namespace FTCCompiler.Common
{
    class Token
    {
        public Token()
        { }

        public Token(Token token)
        {
            this.Lexeme = string.Copy(token.Lexeme);
            this.Type = token.Type;
            this.Line = token.Line;
            this.Column = token.Column;
        }

        public string Lexeme { get; set; }
        public TokenType Type { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
    }
}
