using System;
using FTCCompiler.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FTCCompiler;
using FTCCompiler.Common;
using System.Diagnostics;

namespace FTCCompiler.Test.Lexer
{
    [TestClass]
    public class LexerTest
    {
        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\SeparatorsAndComments.txt")]
        public void ShouldSkipSeparatorsAndComments()
        {
            using (var lexer = new FTCCompiler.Lexer("SeparatorsAndComments.txt"))
            {
                var token = lexer.GetNextToken();

                Assert.IsTrue(token.Type == TokenType.EndOfFile);
                Assert.IsTrue(token.Column == 0);
                Assert.IsTrue(token.Line == 11);
                Assert.IsTrue(token.Lexeme == null);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\EmptyFile.txt")]
        public void ShouldAcceptEmptyFile()
        {
            using (var lexer = new FTCCompiler.Lexer("EmptyFile.txt"))
            {
                var token = lexer.GetNextToken();

                Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\LongFile.txt")]
        public void ShouldProcessALongFile()
        {
            using (var lexer = new FTCCompiler.Lexer("LongFile.txt"))
            {
                var token = lexer.GetNextToken();

                while (token.Type != TokenType.EndOfFile)
                    token = lexer.GetNextToken();

                //Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\LiteralNumber.txt")]
        public void ShouldAcceptLiteralNumber()
        {
            using (var lexer = new FTCCompiler.Lexer("LiteralNumber.txt"))
            {
                var token = lexer.GetNextToken();

                Assert.IsTrue(token.Type == TokenType.LiteralNumber);
                Assert.IsTrue(token.Lexeme == "8763243625");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\UnrecognizedToken.txt")]
        public void ShouldProcessUnrecognizedToken()
        {
            using (var lexer = new FTCCompiler.Lexer("UnrecognizedToken.txt"))
            {
                var token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.Error);
                Assert.IsTrue(token.Lexeme == "#&%?¿");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\LiteralString.txt")]
        public void ShouldAcceptLiteralString()
        {
            using (var lexer = new FTCCompiler.Lexer("LiteralString.txt"))
            {
                var token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.LiteralString);
                Assert.IsTrue(token.Lexeme == "\'Testing\'");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(lexer.GetNextToken().Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\Identifier.txt")]
        public void ShouldAcceptIdentifier()
        {
            using (var lexer = new FTCCompiler.Lexer("Identifier.txt"))
            {
                var token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.Identifier);
                Assert.IsTrue(token.Lexeme == "identifier");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(lexer.GetNextToken().Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\Keywords.txt")]
        public void ShouldAcceptKeywords()
        {
            using (var lexer = new FTCCompiler.Lexer("Keywords.txt"))
            {
                var token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.ConditionalIf);
                Assert.IsTrue(token.Lexeme == Lexemes.ConditionalIf);
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.ConditionalThen);
                Assert.IsTrue(token.Lexeme == Lexemes.ConditionalThen);
                Assert.IsTrue(token.Column == 4);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.ConditionalEnd);
                Assert.IsTrue(token.Lexeme == Lexemes.ConditionalEnd);
                Assert.IsTrue(token.Column == 9);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\Symbols.txt")]
        public void ShouldAcceptSymbols()
        {
            using (var lexer = new FTCCompiler.Lexer("Symbols.txt"))
            {
                var token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.IdTypeSeparator);
                Assert.IsTrue(token.Lexeme == ":");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.ListSeparator);
                Assert.IsTrue(token.Lexeme == ",");
                Assert.IsTrue(token.Column == 3);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfInstruction);
                Assert.IsTrue(token.Lexeme == ";");
                Assert.IsTrue(token.Column == 5);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.LeftSquareBracket);
                Assert.IsTrue(token.Lexeme == "[");
                Assert.IsTrue(token.Column == 7);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.RightSquareBracket);
                Assert.IsTrue(token.Lexeme == "]");
                Assert.IsTrue(token.Column == 9);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.LeftParenthesis);
                Assert.IsTrue(token.Lexeme == "(");
                Assert.IsTrue(token.Column == 11);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.RightParenthesis);
                Assert.IsTrue(token.Lexeme == ")");
                Assert.IsTrue(token.Column == 13);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EqualOperator);
                Assert.IsTrue(token.Lexeme == "=");
                Assert.IsTrue(token.Column == 15);
                Assert.IsTrue(token.Line == 1);
                
                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.GreaterThanOperator);
                Assert.IsTrue(token.Lexeme == ">");
                Assert.IsTrue(token.Column == 17);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.LessThanOperator);
                Assert.IsTrue(token.Lexeme == "<");
                Assert.IsTrue(token.Column == 19);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.MultiplicationOperator);
                Assert.IsTrue(token.Lexeme == "*");
                Assert.IsTrue(token.Column == 21);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.DivisionOperator);
                Assert.IsTrue(token.Lexeme == "/");
                Assert.IsTrue(token.Column == 23);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.AdditionOperator);
                Assert.IsTrue(token.Lexeme == "+");
                Assert.IsTrue(token.Column == 25);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.SubstractionOperator);
                Assert.IsTrue(token.Lexeme == "-");
                Assert.IsTrue(token.Column == 27);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.GreaterThanOrEqualOperator);
                Assert.IsTrue(token.Lexeme == ">=");
                Assert.IsTrue(token.Column == 29);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.LessThanOrEqualOperator);
                Assert.IsTrue(token.Lexeme == "<=");
                Assert.IsTrue(token.Column == 32);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.AssignOperator);
                Assert.IsTrue(token.Lexeme == ":=");
                Assert.IsTrue(token.Column == 35);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.DistinctOperator);
                Assert.IsTrue(token.Lexeme == "<>");
                Assert.IsTrue(token.Column == 38);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\LiteralTrueFalse.txt")]
        public void ShouldAcceptLiteralTrueFalse()
        {
            using (var lexer = new FTCCompiler.Lexer("LiteralTrueFalse.txt"))
            {
                var token = lexer.GetNextToken();

                Assert.IsTrue(token.Type == TokenType.LiteralBoolean);
                Assert.IsTrue(token.Lexeme == "true");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.LiteralBoolean);
                Assert.IsTrue(token.Lexeme == "false");
                Assert.IsTrue(token.Column == 6);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\Operators.txt")]
        public void ShouldAcceptOperators()
        {
            using (var lexer = new FTCCompiler.Lexer("Operators.txt"))
            {
                var token = lexer.GetNextToken();

                Assert.IsTrue(token.Type == TokenType.OrOperator);
                Assert.IsTrue(token.Lexeme == "or");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.AndOperator);
                Assert.IsTrue(token.Lexeme == "and");
                Assert.IsTrue(token.Column == 4);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.XorOperator);
                Assert.IsTrue(token.Lexeme == "xor");
                Assert.IsTrue(token.Column == 8);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\ReadWrite.txt")]
        public void ShouldAcceptReadWrite()
        {
            using (var lexer = new FTCCompiler.Lexer("ReadWrite.txt"))
            {
                var token = lexer.GetNextToken();

                Assert.IsTrue(token.Type == TokenType.Read);
                Assert.IsTrue(token.Lexeme == "read");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.WriteLine);
                Assert.IsTrue(token.Lexeme == "showln");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 2);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.Write);
                Assert.IsTrue(token.Lexeme == "show");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 3);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\DataTypes.txt")]
        public void ShouldAcceptDataTypes()
        {
            using (var lexer = new FTCCompiler.Lexer("DataTypes.txt"))
            {
                var token = lexer.GetNextToken();

                Assert.IsTrue(token.Type == TokenType.IntegerDataType);
                Assert.IsTrue(token.Lexeme == "integer");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.BooleanDataType);
                Assert.IsTrue(token.Lexeme == "boolean");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 2);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.ConstDefinition);
                Assert.IsTrue(token.Lexeme == "const");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 3);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.VariableDefinition);
                Assert.IsTrue(token.Lexeme == "var");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 4);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\MinusError.txt")]
        public void ShouldNotAcceptMinusError()
        {
            using (var lexer = new FTCCompiler.Lexer("MinusError.txt"))
            {
                var token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.Error);
                Assert.IsTrue(token.Lexeme == "end-asd");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\LiteralStringError.txt")]
        public void ShouldAcceptLiteralStringError()
        {
            using (var lexer = new FTCCompiler.Lexer("LiteralStringError.txt"))
            {
                var token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.Error);
                Assert.IsTrue(token.Lexeme == "\'Error");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\Error1.txt")]
        public void ShouldNotAcceptError1()
        {
            using (var lexer = new FTCCompiler.Lexer("Error1.txt"))
            {
                var token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.LiteralNumber);
                Assert.IsTrue(token.Lexeme == "151");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.AdditionOperator);
                Assert.IsTrue(token.Lexeme == "+");
                Assert.IsTrue(token.Column == 5);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.LiteralNumber);
                Assert.IsTrue(token.Lexeme == "18");
                Assert.IsTrue(token.Column == 7);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.Error);
                Assert.IsTrue(token.Lexeme == ".");
                Assert.IsTrue(token.Column == 9);
                Assert.IsTrue(token.Line == 1);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\Error2.txt")]
        public void ShouldNotAcceptError2()
        {
            using (var lexer = new FTCCompiler.Lexer("Error2.txt"))
            {
                var token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.VariableDefinition);
                Assert.IsTrue(token.Lexeme == "var");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.Identifier);
                Assert.IsTrue(token.Lexeme == "integer1");
                Assert.IsTrue(token.Column == 5);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.IdTypeSeparator);
                Assert.IsTrue(token.Lexeme == ":");
                Assert.IsTrue(token.Column == 14);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.IntegerDataType);
                Assert.IsTrue(token.Lexeme == "integer");
                Assert.IsTrue(token.Column == 16);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfInstruction);
                Assert.IsTrue(token.Lexeme == ";");
                Assert.IsTrue(token.Column == 23);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.Identifier);
                Assert.IsTrue(token.Lexeme == "integer1");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 2);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.AssignOperator);
                Assert.IsTrue(token.Lexeme == ":=");
                Assert.IsTrue(token.Column == 10);
                Assert.IsTrue(token.Line == 2);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.LiteralNumber);
                Assert.IsTrue(token.Lexeme == "24");
                Assert.IsTrue(token.Column == 13);
                Assert.IsTrue(token.Line == 2);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfInstruction);
                Assert.IsTrue(token.Lexeme == ";");
                Assert.IsTrue(token.Column == 15);
                Assert.IsTrue(token.Line == 2);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.WriteLine);
                Assert.IsTrue(token.Lexeme == "showln");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 3);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.Error);
                Assert.IsTrue(token.Lexeme == "\'Hola Mund, integer1;");
                Assert.IsTrue(token.Column == 8);
                Assert.IsTrue(token.Line == 3);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfFile);
                Assert.IsTrue(token.Lexeme == null);
                Assert.IsTrue(token.Column == 28);
                Assert.IsTrue(token.Line == 3);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\Error3.txt")]
        public void ShouldNotAcceptError3()
        {
            using (var lexer = new FTCCompiler.Lexer("Error3.txt"))
            {
                var token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.VariableDefinition);
                Assert.IsTrue(token.Lexeme == "var");
                Assert.IsTrue(token.Column == 1);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.Identifier);
                Assert.IsTrue(token.Lexeme == "A");
                Assert.IsTrue(token.Column == 5);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.LeftSquareBracket);
                Assert.IsTrue(token.Lexeme == "[");
                Assert.IsTrue(token.Column == 6);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.LiteralNumber);
                Assert.IsTrue(token.Lexeme == "12");
                Assert.IsTrue(token.Column == 7);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.RightSquareBracket);
                Assert.IsTrue(token.Lexeme == "]");
                Assert.IsTrue(token.Column == 9);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.IdTypeSeparator);
                Assert.IsTrue(token.Lexeme == ":");
                Assert.IsTrue(token.Column == 11);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.IntegerDataType);
                Assert.IsTrue(token.Lexeme == "integer");
                Assert.IsTrue(token.Column == 13);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfInstruction);
                Assert.IsTrue(token.Lexeme == ";");
                Assert.IsTrue(token.Column == 20);
                Assert.IsTrue(token.Line == 1);

                token = lexer.GetNextToken();
                Assert.IsTrue(token.Type == TokenType.EndOfFile);
                Assert.IsTrue(token.Lexeme == null);
                Assert.IsTrue(token.Column == 10);
                Assert.IsTrue(token.Line == 3);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\CompleteLanguage.txt")]
        public void ShouldAcceptCompleteLanguage()
        {
            using (var lexer = new FTCCompiler.Lexer("CompleteLanguage.txt"))
            {
                var token = lexer.GetNextToken();

                while (token.Type != TokenType.EndOfFile)
                    token = lexer.GetNextToken();

                Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Lexer\Sources\Operations.txt")]
        public void ShouldAcceptOperations()
        {
            using (var lexer = new FTCCompiler.Lexer("Operations.txt"))
            {
                var token = lexer.GetNextToken();

                while (token.Type != TokenType.EndOfFile)
                    token = lexer.GetNextToken();

                Assert.IsTrue(token.Type == TokenType.EndOfFile);
            }
        }
    }
}
