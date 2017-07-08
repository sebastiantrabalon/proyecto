using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FTCCompiler.Parsing;
using FTCCompiler.Parsing.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FTCCompiler.Parsing.Common.Exceptions;

namespace FTCCompiler.Test.Parser
{
    [TestClass]
    public class ParserTest
    {
        private List<string> _resultList;

        [TestInitialize]
        public void Initialize()
        {
            _resultList = new List<string>();
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\Expression.txt")]
        public void ExpressionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("Expression.txt"))
            {
                var exp = new ExpressionParser((s) => _resultList.Add(s));

                exp.Parse(lexer, lexer.GetNextToken(), new Attributes());
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\Subroutine.txt")]
        public void SubroutineTest()
        {
            using (var lexer = new FTCCompiler.Lexer("Subroutine.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                //using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                //{
                //    tree.Serialize(tw.BaseStream);
                //}

                // Assert.AreEqual(_resultList.Count, 0);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\ExampleSinErrores.txt")]
        public void ExampleSinErroresTest()
        {
            using (var lexer = new FTCCompiler.Lexer("ExampleSinErrores.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"D:\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.AreEqual(_resultList.Count, 0);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\Example.txt")]
        public void ExampleTest()
        {
            using (var lexer = new FTCCompiler.Lexer("Example.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                parser.Parse();

                Assert.IsTrue(_resultList[0] == "[16,12] SIN: Se esperaba ':=' y se encontró ''Visualizacion''.");
                Assert.IsTrue(_resultList[1] == "[21,27] SIN: Se esperaba 'integer','boolean' y se encontró 'entero'.");
                Assert.IsTrue(_resultList[2] == "[35,12] SIN: Se esperaba 'end-proc' y se encontró ';'.");
                Assert.AreEqual(_resultList.Count, 3);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\Environment.txt")]
        [ExpectedException(typeof(MainNotFoundException))]
        public void EnvironmentTest()
        {
            using (var lexer = new FTCCompiler.Lexer("Environment.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.Fail("Should have failed with MainNotFoundException");

                Assert.IsTrue(_resultList[0] == "[4,7] SIN: Se esperaba 'identificador' y se encontró ','.");
                Assert.AreEqual(_resultList.Count, 1);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\DataType.txt")]
        [ExpectedException(typeof(MainNotFoundException))]
        public void DataTypeTest()
        {
            using (var lexer = new FTCCompiler.Lexer("DataType.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.Fail("Should have failed with MainNotFoundException");

                Assert.IsTrue(_resultList[0] == "[4,7] SIN: Se esperaba 'identificador' y se encontró ','.");
                Assert.AreEqual(_resultList.Count, 1);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\ErrorIdentifier.txt")]
        [ExpectedException(typeof(MainNotFoundException))]
        public void ShouldReturnErrorIdentifier()
        {
            using (var lexer = new FTCCompiler.Lexer("ErrorIdentifier.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.Fail("Should have failed with MainNotFoundException");

                Assert.IsTrue(_resultList[0] == "[1,7] SIN: Se esperaba 'identificador' y se encontró '4'.");
                Assert.AreEqual(_resultList.Count, 1);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\ErrorWhile.txt")]
        [ExpectedException(typeof(MainNotFoundException))]
        public void ShouldReturnErrorWhile()
        {
            using (var lexer = new FTCCompiler.Lexer("ErrorWhile.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.Fail("Should have failed with MainNotFoundException");

                Assert.IsTrue(_resultList[0] == "[14,3] SIN: Se esperaba 'end-while' y se encontró 'end-'.");
                Assert.IsTrue(_resultList[1] == "[15,9] SIN: Se esperaba 'end-proc' y se encontró ';'.");
                Assert.AreEqual(_resultList.Count, 2);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\ErrorProc.txt")]
        [ExpectedException(typeof(MainNotFoundException))]
        public void ShouldReturnErrorProc()
        {
            using (var lexer = new FTCCompiler.Lexer("ErrorProc.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.Fail("Should have failed with MainNotFoundException");

                Assert.IsTrue(_resultList[0] == "[15,1] SIN: Se esperaba 'end-proc' y se encontró 'end-'.");
                Assert.AreEqual(_resultList.Count, 1);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\ErrorArray.txt")]
        [ExpectedException(typeof(MainNotFoundException))]
        public void ShouldReturnErrorArray()
        {
            using (var lexer = new FTCCompiler.Lexer("ErrorArray.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.Fail("Should have failed with MainNotFoundException");

                Assert.IsTrue(_resultList[0] == "[3,7] SIN: Se esperaba 'número' y se encontró ']'.");
                Assert.AreEqual(_resultList.Count, 1);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\ErrorShowLn.txt")]
        [ExpectedException(typeof(MainNotFoundException))]
        public void ShouldReturnErrorShowLn()
        {
            using (var lexer = new FTCCompiler.Lexer("ErrorShowLn.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.Fail("Should have failed with MainNotFoundException");

                Assert.IsTrue(_resultList[0] == "[16,12] SIN: Se esperaba ':=' y se encontró ''Visualizacion''.");
                Assert.IsTrue(_resultList.Count == 1);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\ErrorParameters.txt")]
        [ExpectedException(typeof(MainNotFoundException))]
        public void ShouldReturnErrorParameters()
        {
            using (var lexer = new FTCCompiler.Lexer("ErrorParameters.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.Fail("Should have failed with MainNotFoundException");

                Assert.IsTrue(_resultList[0] == "[1,33] SIN: Se esperaba 'identificador' y se encontró ':'.");
                Assert.IsTrue(_resultList[1] == "[3,18] SIN: Se esperaba ':' y se encontró ')'.");
                Assert.IsTrue(_resultList[2] == "[11,9] SIN: Se esperaba 'end-func' y se encontró ';'.");
                Assert.AreEqual(_resultList.Count, 3);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\Recuperacion-1.txt")]
        public void ShouldNotReturnErrorRecuperacion1()
        {
            using (var lexer = new FTCCompiler.Lexer("Recuperacion-1.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.IsTrue(_resultList[0] == "[6,5] SIN: Se esperaba 'identificador' y se encontró ':'.");
                Assert.IsTrue(_resultList[1] == "[12,5] SIN: Se esperaba 'expresión entera' y se encontró ']'.");
                Assert.AreEqual(_resultList.Count, 2);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\Recuperacion-2.txt")]
        public void ShouldNotReturnErrorRecuperacion2()
        {
            using (var lexer = new FTCCompiler.Lexer("Recuperacion-2.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.IsTrue(_resultList[0] == "[2,12] SIN: Se esperaba 'integer','boolean' y se encontró 'enteros'.");
                Assert.IsTrue(_resultList[1] == "[16,13] SIN: Se esperaba ';' y se encontró ''Visualizacion''.");
                Assert.IsTrue(_resultList[2] == "[17,12] SIN: Se esperaba ';' y se encontró '?'.");
                Assert.AreEqual(_resultList.Count, 3);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\Recuperacion-3.txt")]
        [ExpectedException(typeof(MainNotFoundException))]
        public void ShouldNotReturnErrorRecuperacion3()
        {
            using (var lexer = new FTCCompiler.Lexer("Recuperacion-3.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.Fail("Should have failed with MainNotFoundException");

                Assert.IsTrue(_resultList[0] == "[1,31] SIN: Se esperaba ':' y se encontró 'R'.");
                Assert.IsTrue(_resultList[1] == "[2,21] SIN: Se esperaba 'número','true o false' y se encontró 'TRUE'.");
                Assert.IsTrue(_resultList[2] == "[21,13] SIN: Se esperaba ';' y se encontró 'Q'.");
                Assert.AreEqual(_resultList.Count, 3);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Parser\Sources\InvalidExpression1.txt")]
        public void ShouldFailWithInvalidExpression()
        {
            using (var lexer = new FTCCompiler.Lexer("InvalidExpression1.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => { _resultList.Add(s); });

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"C:\Users\Ivan\Documents\Proyectos\FTC Compiler\tree\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.IsTrue(_resultList[0] == "[4,13] SIN: Se esperaba ';' y se encontró 'M'.");
                Assert.AreEqual(_resultList.Count, 1);
            }
        }
    }
}
