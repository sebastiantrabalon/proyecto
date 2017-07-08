using FTCCompiler.Common;
using System;

namespace FTCCompiler.Utils
{
    class Character
    {
        private const string Numbers = "0123456789";
        private const string Letters = "abcdefghijklmnñopqrstuvwxyz";
        private const string Alphabet = "01234567890abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ:,;[]()=><*/+-'";
        private int _char;

        public Character(int @char)
        {
            _char = @char;
        }

        public bool IsSymbol(Symbols type)
        {
            return _char == (int)type;
        }

        public bool IsSymbol()
        {
            return IsSymbol(Symbols.Asterisk) ||
                   IsSymbol(Symbols.Colon) ||
                   IsSymbol(Symbols.Comma) ||
                   IsSymbol(Symbols.Equal) ||
                   IsSymbol(Symbols.GreaterThan) ||
                   IsSymbol(Symbols.LeftParenthesis) ||
                   IsSymbol(Symbols.LeftSquareBracket) ||
                   IsSymbol(Symbols.LessThan) ||
                   IsSymbol(Symbols.Minus) ||
                   IsSymbol(Symbols.Plus) ||
                   IsSymbol(Symbols.RightParenthesis) ||
                   IsSymbol(Symbols.RightSquareBracket) ||
                   IsSymbol(Symbols.Semicolon) ||
                   IsSymbol(Symbols.Slash);
        }

        public bool IsSeparator(Separators type)
        {
            return _char == (int)type;
        }

        public bool IsSeparator()
        {
            return IsSeparator(Separators.Space) ||
                   IsSeparator(Separators.Tab) ||
                   IsSeparator(Separators.CRLF);
        }

        public bool IsDigit()
        {
            return Numbers.Contains(ToString());
        }

        public bool IsAlpha()
        {
            return Letters.Contains(ToString().ToLower());
        }

        public bool IsEof()
        {
            return _char == -1;
        }

        public bool BelongsToAlphabet()
        {
            return IsSeparator() || Alphabet.Contains(ToString());
        }

        public override string ToString()
        {
            return Convert.ToChar(_char).ToString();
        }
    }
}
