using FTCCompiler.Common;
using FTCCompiler.Resources;
using FTCCompiler.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FTCCompiler
{
    class Lexer : IDisposable
    {
        private Dictionary<string, TokenType> _keywords;
        private readonly FileReader _fileReader;
        private Token _currentToken;
        private int _curTokenLineCounter = 0;
        private int _curTokenColumnCounter = 0;

        public Lexer(string sourceFile)
        {
            _fileReader = new FileReader(sourceFile);
            PrepareKeywords();
        }

        public Token GetCurrentToken()
        {
            return _currentToken;
        }

        public Token GetNextToken()
        {
            _currentToken = ReadToken();
            return _currentToken;
        }

        private Token ReadToken()
        {
            var moreToRead = SkipSeparatorsAndComments();

            if (moreToRead)
            {
                var curChar = _fileReader.GetNextChar();

                SetTokenStart();

                if (curChar.BelongsToAlphabet())
                {
                    if (curChar.IsSymbol())
                        return ReadSymbol(curChar);

                    if (curChar.IsDigit())
                        return ReadLiteralNumber(curChar);

                    if (curChar.IsSymbol(Symbols.Quote))
                        return ReadLiteralString(curChar);

                    var lexeme = ReadUnknownToken(curChar);

                    if (_keywords.ContainsKey(lexeme))
                        return BuildToken(_keywords[lexeme], lexeme);

                    if (lexeme.Contains(Symbols.Minus))
                        return BuildToken(TokenType.Error, lexeme);

                    return BuildToken(TokenType.Identifier, lexeme);
                }
                else
                {
                    return ReadUnrecognizedToken(curChar);
                }
            }

            SetTokenStart();

            return BuildToken(TokenType.EndOfFile);
        }

        #region Token building

        private Token BuildToken(TokenType type)
        {
            return BuildToken(type, (string)null);
        }

        private Token BuildToken(TokenType type, params Character[] lexemeChars)
        {
            return BuildToken(type, 
                string.Concat(lexemeChars.Select(x => x.ToString()).ToList()));
        }

        private Token BuildToken(TokenType type, string lexeme)
        {
            return new Token()
            {
                Type = type,
                Lexeme = lexeme,
                Column = _curTokenColumnCounter,
                Line = _curTokenLineCounter
            };
        }

        #endregion

        #region Token recognition

        private Token ReadUnrecognizedToken(Character firstChar)
        {
            var lexeme = firstChar.ToString();
            var curChar = _fileReader.PeekNextChar();

            while (!curChar.IsEof() && !curChar.IsSeparator() && 
                    !curChar.BelongsToAlphabet())
            {
                lexeme += _fileReader.GetNextChar().ToString();
                curChar = _fileReader.PeekNextChar();
            }

            return BuildToken(TokenType.Error, lexeme);
        }

        private Token ReadLiteralNumber(Character firstChar)
        {
            var lexeme = firstChar.ToString();
            var curChar = _fileReader.PeekNextChar();

            while (!curChar.IsEof() && !curChar.IsSeparator() &&
                    curChar.IsDigit())
            {
                lexeme += _fileReader.GetNextChar().ToString();
                curChar = _fileReader.PeekNextChar();
            }

            return BuildToken(TokenType.LiteralNumber, lexeme);
        }

        private Token ReadLiteralString(Character firstChar)
        {
            var lexeme = firstChar.ToString();
            var curChar = _fileReader.PeekNextChar();

            while (!curChar.IsEof() && curChar.BelongsToAlphabet() && 
                    !curChar.IsSymbol(Symbols.Quote))
            {
                lexeme += _fileReader.GetNextChar().ToString();
                curChar = _fileReader.PeekNextChar();
            }

            if (!curChar.IsEof())
                lexeme += _fileReader.GetNextChar().ToString();

            if (curChar.IsSymbol(Symbols.Quote))
                return BuildToken(TokenType.LiteralString, lexeme);

            return BuildToken(TokenType.Error, lexeme);
        }

        private Token ReadSymbol(Character firstChar)
        {
            if (firstChar.IsSymbol(Symbols.Comma))
                return BuildToken(TokenType.ListSeparator, firstChar);

            if (firstChar.IsSymbol(Symbols.Semicolon))
                return BuildToken(TokenType.EndOfInstruction, firstChar);

            if (firstChar.IsSymbol(Symbols.RightParenthesis))
                return BuildToken(TokenType.RightParenthesis, firstChar);

            if (firstChar.IsSymbol(Symbols.LeftParenthesis))
                return BuildToken(TokenType.LeftParenthesis, firstChar);

            if (firstChar.IsSymbol(Symbols.LeftSquareBracket))
                return BuildToken(TokenType.LeftSquareBracket, firstChar);

            if (firstChar.IsSymbol(Symbols.RightSquareBracket))
                return BuildToken(TokenType.RightSquareBracket, firstChar);

            if (firstChar.IsSymbol(Symbols.Equal))
                return BuildToken(TokenType.EqualOperator, firstChar);

            if (firstChar.IsSymbol(Symbols.Asterisk))
                return BuildToken(TokenType.MultiplicationOperator, firstChar);

            if (firstChar.IsSymbol(Symbols.Slash))
                return BuildToken(TokenType.DivisionOperator, firstChar);

            if (firstChar.IsSymbol(Symbols.Plus))
                return BuildToken(TokenType.AdditionOperator, firstChar);

            if (firstChar.IsSymbol(Symbols.Minus))
                return BuildToken(TokenType.SubstractionOperator, firstChar);

            if (firstChar.IsSymbol(Symbols.Colon))
            {
                if (_fileReader.PeekNextChar().IsSymbol(Symbols.Equal))
                    return BuildToken(TokenType.AssignOperator, firstChar, _fileReader.GetNextChar());

                return BuildToken(TokenType.IdTypeSeparator, firstChar);
            }

            if (firstChar.IsSymbol(Symbols.GreaterThan))
            {
                if (_fileReader.PeekNextChar().IsSymbol(Symbols.Equal))
                    return BuildToken(TokenType.GreaterThanOrEqualOperator, firstChar, _fileReader.GetNextChar());

                return BuildToken(TokenType.GreaterThanOperator, firstChar);
            }

            if (firstChar.IsSymbol(Symbols.LessThan))
            {
                var nextChar = _fileReader.PeekNextChar();

                if (nextChar.IsSymbol(Symbols.Equal))
                    return BuildToken(TokenType.LessThanOrEqualOperator, firstChar, _fileReader.GetNextChar());
                else if (nextChar.IsSymbol(Symbols.GreaterThan))
                    return BuildToken(TokenType.DistinctOperator, firstChar, _fileReader.GetNextChar());

                return BuildToken(TokenType.LessThanOperator, firstChar);
            }

            return BuildToken(TokenType.Error, firstChar);
        }

        private string ReadUnknownToken(Character firstChar)
        {
            var lexeme = firstChar.ToString();
            var curChar = _fileReader.PeekNextChar();

            while (!curChar.IsEof() && curChar.BelongsToAlphabet() && !curChar.IsSymbol() && 
                    !curChar.IsSeparator() || curChar.IsSymbol(Symbols.Minus))
            {
                if (curChar.IsSymbol(Symbols.Minus) && lexeme != Lexemes.End)
                    break;

                lexeme += _fileReader.GetNextChar().ToString();
                curChar = _fileReader.PeekNextChar();
            }

            return lexeme;
        }

        #endregion

        #region Utilities

        private bool SkipSeparatorsAndComments()
        {
            var curChar = _fileReader.PeekNextChar();
            var insideComment = false;

            while (!curChar.IsEof())
            {
                if (curChar.IsSymbol(Symbols.CommentStart))
                    insideComment = true;
                else if (curChar.IsSymbol(Symbols.CommentEnd))
                    insideComment = false;
                else if (!curChar.IsSeparator() && !insideComment)
                    return true;

                _fileReader.GetNextChar();

                curChar = _fileReader.PeekNextChar();
            }

            return false;
        }

        private void SetTokenStart()
        {
            _curTokenLineCounter = _fileReader.Line;
            _curTokenColumnCounter = _fileReader.Column;
        }

        private void PrepareKeywords()
        {
            _keywords = new Dictionary<string, TokenType>();

            _keywords.Add(Lexemes.IntegerDataType, TokenType.IntegerDataType);
            _keywords.Add(Lexemes.BooleanDataType, TokenType.BooleanDataType);
            _keywords.Add(Lexemes.ConstDefinition, TokenType.ConstDefinition);
            _keywords.Add(Lexemes.VariableDefinition, TokenType.VariableDefinition);
            _keywords.Add(Lexemes.ProcedureDefinition, TokenType.ProcedureDefinition);
            _keywords.Add(Lexemes.ScopeStart, TokenType.ScopeStart);
            _keywords.Add(Lexemes.OrOperator, TokenType.OrOperator);
            _keywords.Add(Lexemes.AndOperator, TokenType.AndOperator);
            _keywords.Add(Lexemes.XorOperator, TokenType.XorOperator);
            _keywords.Add(Lexemes.ProcedureEnd, TokenType.ProcedureEnd);
            _keywords.Add(Lexemes.FunctionDefinition, TokenType.FunctionDefinition);
            _keywords.Add(Lexemes.FunctionEnd, TokenType.FunctionEnd);
            _keywords.Add(Lexemes.ConditionalIf, TokenType.ConditionalIf);
            _keywords.Add(Lexemes.ConditionalThen, TokenType.ConditionalThen);
            _keywords.Add(Lexemes.ConditionalElse, TokenType.ConditionalElse);
            _keywords.Add(Lexemes.ConditionalEnd, TokenType.ConditionalEnd);
            _keywords.Add(Lexemes.IterationWhile, TokenType.IterationWhile);
            _keywords.Add(Lexemes.IterationDo, TokenType.IterationDo);
            _keywords.Add(Lexemes.IterationEnd, TokenType.IterationEnd);
            _keywords.Add(Lexemes.ByVal, TokenType.ByVal);
            _keywords.Add(Lexemes.ByRef, TokenType.ByRef);
            _keywords.Add(Lexemes.Read, TokenType.Read);
            _keywords.Add(Lexemes.Write, TokenType.Write);
            _keywords.Add(Lexemes.WriteLine, TokenType.WriteLine);
            _keywords.Add(Lexemes.LiteralTrue, TokenType.LiteralBoolean);
            _keywords.Add(Lexemes.LiteralFalse, TokenType.LiteralBoolean);
            _keywords.Add(Lexemes.End, TokenType.Error);
        }

        #endregion

        public void Dispose()
        {
            _fileReader.Dispose();
            _keywords.Clear();
            _keywords = null;
        }
    }
}
