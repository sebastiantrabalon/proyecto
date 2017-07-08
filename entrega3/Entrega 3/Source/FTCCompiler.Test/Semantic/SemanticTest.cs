using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FTCCompiler.Test.Semantic 
{
    [TestClass]
    public class SemanticTest
    {
        private List<string> _resultList;

        [TestInitialize]
        public void Initialize()
        {
            _resultList = new List<string>();
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\IdentificadorDuplicadoDiferenteEntorno.txt")]
        public void IdentificadorDuplicadoDiferenteEntornoTest()
        {
            using (var lexer = new FTCCompiler.Lexer("IdentificadorDuplicadoDiferenteEntorno.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(false, tree.HasErrors);
                Assert.AreEqual(0, _resultList.Count);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\IdentificadorDuplicadoMismoEntorno.txt")]
        public void IdentificadorDuplicadoMismoEntornoTest()
        {
            using (var lexer = new FTCCompiler.Lexer("IdentificadorDuplicadoMismoEntorno.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[3,5] SEM: Ya existe un identificador con el nombre 'A'.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\LlamarProcEnEntornoLocal.txt")]
        public void LlamarProcEnEntornoLocalTest()
        {
            using (var lexer = new FTCCompiler.Lexer("LlamarProcEnEntornoLocal.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(false, tree.HasErrors);
                Assert.AreEqual(0, _resultList.Count);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\ArrayIndiceExpresionNoEntera.txt")]
        public void ArrayIndiceExpresionNoEnteraTest()
        {
            using (var lexer = new FTCCompiler.Lexer("ArrayIndiceExpresionNoEntera.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(3, _resultList.Count);

                Assert.AreEqual("[10,16] SEM: El indice de un vector debe ser una expresión entera.", _resultList[0]);
                Assert.AreEqual("[11,14] SEM: El indice de un vector debe ser una expresión entera.", _resultList[1]);
                Assert.AreEqual("[12,15] SEM: El indice de un vector debe ser una expresión entera.", _resultList[2]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperacionBinariaDistintosTipoDeDatos.txt")]
        public void OperacionBinariaDistintosTipoDeDatosTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperacionBinariaDistintosTipoDeDatos.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(8, _resultList.Count);

                Assert.AreEqual("[11,15] SEM: No se pueden comparar una expresión de tipo 'Integer' con una de tipo 'Boolean'.", _resultList[0]);
                Assert.AreEqual("[12,15] SEM: No se pueden comparar una expresión de tipo 'Integer' con una de tipo 'Boolean'.", _resultList[1]);
                Assert.AreEqual("[13,16] SEM: No se pueden comparar una expresión de tipo 'Integer' con una de tipo 'Boolean'.", _resultList[2]);
                Assert.AreEqual("[14,15] SEM: No se pueden comparar una expresión de tipo 'Integer' con una de tipo 'Boolean'.", _resultList[3]);
                Assert.AreEqual("[15,16] SEM: No se pueden comparar una expresión de tipo 'Integer' con una de tipo 'Boolean'.", _resultList[4]);
                Assert.AreEqual("[16,16] SEM: No se pueden comparar una expresión de tipo 'Integer' con una de tipo 'Boolean'.", _resultList[5]);
                Assert.AreEqual("[17,19] SEM: No se pueden comparar una expresión de tipo 'Integer' con una de tipo 'Boolean'.", _resultList[6]);
                Assert.AreEqual("[18,21] SEM: No se pueden comparar una expresión de tipo 'Integer' con una de tipo 'Boolean'.", _resultList[7]);
            }
        }

		[TestMethod]
        [DeploymentItem(@"Semantic\Sources\ReadIdentificadorInexistente.txt")]
        public void ReadIdentificadorInexistenteTest()
        {
            using (var lexer = new FTCCompiler.Lexer("ReadIdentificadorInexistente.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[7,9] SEM: No se encontró el identificador 'X'.", _resultList[0]);
            }
        }        

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\IfCondicionNoBooleana.txt")]
        public void IfCondicionNoBooleanaTest()
        {
            using (var lexer = new FTCCompiler.Lexer("IfCondicionNoBooleana.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[7,3] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\WhileCondicionNoBooleana.txt")]
        public void WhileCondicionNoBooleanaTest()
        {
            using (var lexer = new FTCCompiler.Lexer("WhileCondicionNoBooleana.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[7,3] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperacionDivisionNoEnterosEnCondicion.txt")]
        public void OperacionDivisionNoEnterosEnCondicionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperacionDivisionNoEnterosEnCondicion.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(6, _resultList.Count);

                Assert.AreEqual("[11,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[0]);
                Assert.AreEqual("[12,11] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[1]);
                Assert.AreEqual("[13,19] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[2]);
                Assert.AreEqual("[14,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[3]);
                Assert.AreEqual("[14,19] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[4]);
                Assert.AreEqual("[15,21] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[5]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperacionMultiplicacionNoEnterosEnCondicion.txt")]
        public void OperacionMultiplicacionNoEnterosEnCondicionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperacionMultiplicacionNoEnterosEnCondicion.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(6, _resultList.Count);

                Assert.AreEqual("[11,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[0]);
                Assert.AreEqual("[12,11] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[1]);
                Assert.AreEqual("[13,19] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[2]);
                Assert.AreEqual("[14,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[3]);
                Assert.AreEqual("[14,19] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[4]);
                Assert.AreEqual("[15,21] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[5]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperacionRestaNoEnterosEnCondicion.txt")]
        public void OperacionRestaNoEnterosEnCondicionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperacionRestaNoEnterosEnCondicion.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(6, _resultList.Count);

                Assert.AreEqual("[11,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[0]);
                Assert.AreEqual("[12,11] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[1]);
                Assert.AreEqual("[13,19] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[2]);
                Assert.AreEqual("[14,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[3]);
                Assert.AreEqual("[14,19] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[4]);
                Assert.AreEqual("[15,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[5]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperacionSumaNoEnterosEnCondicion.txt")]
        public void OperacionSumaNoEnterosEnCondicionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperacionSumaNoEnterosEnCondicion.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(6, _resultList.Count);

                Assert.AreEqual("[11,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[0]);
                Assert.AreEqual("[12,11] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[1]);
                Assert.AreEqual("[13,19] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[2]);
                Assert.AreEqual("[14,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[3]);
                Assert.AreEqual("[14,19] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[4]);
                Assert.AreEqual("[15,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[5]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperacionMayorIgualNoEnterosEnCondicion.txt")]
        public void OperacionMayorIgualNoEnterosEnCondicionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperacionMayorIgualNoEnterosEnCondicion.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(3, _resultList.Count);

                Assert.AreEqual("[11,16] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[0]);
                Assert.AreEqual("[12,20] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[1]);
                Assert.AreEqual("[13,22] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[2]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperacionMayorNoEnterosEnCondicion.txt")]
        public void OperacionMayorNoEnterosEnCondicionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperacionMayorNoEnterosEnCondicion.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(3, _resultList.Count);

                Assert.AreEqual("[11,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[0]);
                Assert.AreEqual("[12,19] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[1]);
                Assert.AreEqual("[13,21] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[2]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperacionMenorIgualNoEnterosEnCondicion.txt")]
        public void OperacionMenorIgualNoEnterosEnCondicionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperacionMenorIgualNoEnterosEnCondicion.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(3, _resultList.Count);

                Assert.AreEqual("[11,16] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[0]);
                Assert.AreEqual("[12,20] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[1]);
                Assert.AreEqual("[13,22] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[2]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperacionMenorNoEnterosEnCondicion.txt")]
        public void OperacionMenorNoEnterosEnCondicionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperacionMenorNoEnterosEnCondicion.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(3, _resultList.Count);

                Assert.AreEqual("[11,15] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[0]);
                Assert.AreEqual("[12,19] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[1]);
                Assert.AreEqual("[13,21] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[2]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperadorAndNoBooleanoEnCondicion.txt")]
        public void OperadorAndNoBooleanoEnCondicionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperadorAndNoBooleanoEnCondicion.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(8, _resultList.Count);

                Assert.AreEqual("[11,11] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[0]);
                Assert.AreEqual("[11,17] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[1]);
                Assert.AreEqual("[12,11] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[2]);
                Assert.AreEqual("[12,21] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[3]);
                Assert.AreEqual("[13,15] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[4]);
                Assert.AreEqual("[13,21] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[5]);
                Assert.AreEqual("[14,15] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[6]);
                Assert.AreEqual("[14,23] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[7]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperadorOrNoBooleanoEnCondicion.txt")]
        public void OperadorOrNoBooleanoEnCondicionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperadorOrNoBooleanoEnCondicion.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(8, _resultList.Count);

                Assert.AreEqual("[11,11] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[0]);
                Assert.AreEqual("[11,16] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[1]);
                Assert.AreEqual("[12,11] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[2]);
                Assert.AreEqual("[12,20] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[3]);
                Assert.AreEqual("[13,15] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[4]);
                Assert.AreEqual("[13,20] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[5]);
                Assert.AreEqual("[14,15] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[6]);
                Assert.AreEqual("[14,22] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[7]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\OperadorXorNoBooleanoEnCondicion.txt")]
        public void OperadorXorNoBooleanoEnCondicionTest()
        {
            using (var lexer = new FTCCompiler.Lexer("OperadorXorNoBooleanoEnCondicion.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(8, _resultList.Count);

                Assert.AreEqual("[11,11] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[0]);
                Assert.AreEqual("[11,17] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[1]);
                Assert.AreEqual("[12,11] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[2]);
                Assert.AreEqual("[12,21] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[3]);
                Assert.AreEqual("[13,15] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[4]);
                Assert.AreEqual("[13,21] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[5]);
                Assert.AreEqual("[14,15] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[6]);
                Assert.AreEqual("[14,23] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[7]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\MainConParametros.txt")]
        public void MainConParametrosTest()
        {
            using (var lexer = new FTCCompiler.Lexer("MainConParametros.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("SEM: El procedimiento MAIN no puede tener parámetros.", _resultList[0]);
                
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\MainConEntornoLocal.txt")]
        public void MainConEntornoLocalTest()
        {
            using (var lexer = new FTCCompiler.Lexer("MainConEntornoLocal.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("SEM: El procedimiento MAIN no puede tener un entorno local.", _resultList[0]);

            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\FuncionUsaParametros.txt")]
        public void FuncionUsaParametrosTest()
        {
            using (var lexer = new FTCCompiler.Lexer("FuncionUsaParametros.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(false, tree.HasErrors);
                Assert.AreEqual(0, _resultList.Count);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\ShowIdentificadorInexistente.txt")]
        public void ShowIdentificadorInexistenteTest()
        {
            using (var lexer = new FTCCompiler.Lexer("ShowIdentificadorInexistente.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(2, _resultList.Count);

                Assert.AreEqual("[13,9] SEM: No se encontró el identificador 'X'.", _resultList[0]);
                Assert.AreEqual("[13,8] SEM: La expresión no puede ser nula.", _resultList[1]);                
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\ShowFuncionInexistente.txt")]
        public void ShowFuncionInexistenteTest()
        {
            using (var lexer = new FTCCompiler.Lexer("ShowFuncionInexistente.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[13,12] SEM: No se encontró una función con nombre 'FUN1'.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\PasajeParametrosDistintoTiposSegunVariable.txt")]
        public void PasajeParametrosDistintoTiposSegunVariableTest()
        {
            using (var lexer = new FTCCompiler.Lexer("PasajeParametrosDistintoTiposSegunVariable.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[13,7] SEM: La cantidad de parametros proporcionados o el tipo de los mismos no coinciden con la definición de 'PROC'.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\PasajeParametrosFueraDeScope.txt")]
        public void PasajeParametrosFueraDeScopeTest()
        {
            using (var lexer = new FTCCompiler.Lexer("PasajeParametrosFueraDeScope.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(2, _resultList.Count);

                Assert.AreEqual("[14,10] SEM: No se encontró el identificador 'X'.", _resultList[0]);
                Assert.AreEqual("[14,8] SEM: La cantidad de parametros proporcionados o el tipo de los mismos no coinciden con la definición de 'PROC1'.", _resultList[1]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\PasajeParametrosIdentificadorInexistente.txt")]
        public void PasajeParametrosIdentificadorInexistenteTest()
        {
            using (var lexer = new FTCCompiler.Lexer("PasajeParametrosIdentificadorInexistente.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(2, _resultList.Count);

                Assert.AreEqual("[14,10] SEM: No se encontró el identificador 'K'.", _resultList[0]);
                Assert.AreEqual("[14,8] SEM: La cantidad de parametros proporcionados o el tipo de los mismos no coinciden con la definición de 'PROC1'.", _resultList[1]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\PasajeParametrosMenosParametros.txt")]
        public void PasajeParametrosMenosParametrosTest()
        {
            using (var lexer = new FTCCompiler.Lexer("PasajeParametrosMenosParametros.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[13,7] SEM: La cantidad de parametros proporcionados o el tipo de los mismos no coinciden con la definición de 'PROC'.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\PasajeParametrosTipoArray.txt")]
        public void PasajeParametrosTipoArrayTest()
        {
            using (var lexer = new FTCCompiler.Lexer("PasajeParametrosTipoArray.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[13,7] SEM: En la llamada a 'PROC' no se puede pasar un array como parámetro.", _resultList[0]);
            }
        }

		[TestMethod]
        [DeploymentItem(@"Semantic\Sources\FuncionRetornoNoCoincideConTipoDatoRetorno.txt")]
        public void FuncionRetornoNoCoincideConTipoDatoRetornoTest()
        {
            using (var lexer = new FTCCompiler.Lexer("FuncionRetornoNoCoincideConTipoDatoRetorno.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[17,1] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[0]);
            }
        }
        
        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\FuncionInexistente.txt")]
        public void FuncionInexistenteTest()
        {
            using (var lexer = new FTCCompiler.Lexer("FuncionInexistente.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[22,13] SEM: No se encontró una función con nombre 'FUN10'.", _resultList[0]);
            }
        }  

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\ProcedimientoInexistente.txt")]
        public void ProcedimientoInexistenteTest()
        {
            using (var lexer = new FTCCompiler.Lexer("ProcedimientoInexistente.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[49,9] SEM: No se encontró un procedimiento con nombre 'PROC20'.", _resultList[0]);
            }
        }  
     
        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\DeclaracionParametrosDuplicados.txt")]
        public void DeclaracionParametrosDuplicadosTest()
        {
            using (var lexer = new FTCCompiler.Lexer("DeclaracionParametrosDuplicados.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[5,42] SEM: Ya existe un identificador con el nombre 'N'.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\DeclaracionDeArrayFueraDeEntornoGlobal.txt")]
        public void DeclaracionDeArrayFueraDeEntornoGlobalTest()
        {
            using (var lexer = new FTCCompiler.Lexer("DeclaracionDeArrayFueraDeEntornoGlobal.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[4,5] SEM: No se puede declarar un array fuera del entorno global.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\LlamadasRecursivas.txt")]
        public void LlamadasRecursivasTest()
        {
            using (var lexer = new FTCCompiler.Lexer("LlamadasRecursivas.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(false, tree.HasErrors);
                Assert.AreEqual(0, _resultList.Count);
            }
        }


        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\IfCondicionFuncionBoolean.txt")]
        public void IfCondicionFuncionBooleanTest()
        {
            using (var lexer = new FTCCompiler.Lexer("IfCondicionFuncionBoolean.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(false, tree.HasErrors);
                Assert.AreEqual(0, _resultList.Count);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\IfCondicionFuncionInteger.txt")]
        public void IfCondicionFuncionIntegerTest()
        {
            using (var lexer = new FTCCompiler.Lexer("IfCondicionFuncionInteger.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[9,3] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\IfCondicionProcedimiento.txt")]
        public void IfCondicionProcedimientoTest()
        {
            using (var lexer = new FTCCompiler.Lexer("IfCondicionProcedimiento.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(2, _resultList.Count);

                Assert.AreEqual("[10,10] SEM: No se encontró una función con nombre 'PROC'.", _resultList[0]);
                Assert.AreEqual("[10,3] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[1]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\WhileCondicionFuncionBoolean.txt")]
        public void WhileCondicionFuncionBooleanTest()
        {
            using (var lexer = new FTCCompiler.Lexer("WhileCondicionFuncionBoolean.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(false, tree.HasErrors);
                Assert.AreEqual(0, _resultList.Count);

            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\WhileCondicionFuncionInteger.txt")]
        public void WhileCondicionFuncionIntegerTest()
        {
            using (var lexer = new FTCCompiler.Lexer("WhileCondicionFuncionInteger.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[9,3] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\WhileCondicionProcedimiento.txt")]
        public void WhileCondicionProcedimientoTest()
        {
            using (var lexer = new FTCCompiler.Lexer("WhileCondicionProcedimiento.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(2, _resultList.Count);

                Assert.AreEqual("[10,13] SEM: No se encontró una función con nombre 'PROC'.", _resultList[0]);
                Assert.AreEqual("[10,3] SEM: Se esperaba una expresión 'Boolean' y se encontró una del tipo 'Integer'.", _resultList[1]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\AsignacionDistintoTipoDatoAsignable.txt")]
        public void AsignacionDistintoTipoDatoAsignableTest()
        {
            using (var lexer = new FTCCompiler.Lexer("AsignacionDistintoTipoDatoAsignable.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[5,9] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\AsignacionExpresionAConstante.txt")]
        public void AsignacionExpresionAConstanteTest()
        {
            using (var lexer = new FTCCompiler.Lexer("AsignacionExpresionAConstante.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[5,2] SEM: No se puede hacer una asignación a una constante", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\AsignacionIdentificadorFueraDeScope.txt")]
        public void AsignacionIdentificadorFueraDeScopeTest()
        {
            using (var lexer = new FTCCompiler.Lexer("AsignacionIdentificadorFueraDeScope.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(2, _resultList.Count);

                Assert.AreEqual("[8,7] SEM: No se encontró el identificador 'X'.", _resultList[0]);
                Assert.AreEqual("[14,7] SEM: No se encontró el identificador 'B'.", _resultList[1]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\AsignacionIdentificadorInexistente.txt")]
        public void AsignacionIdentificadorInexistenteTest()
        {
            using (var lexer = new FTCCompiler.Lexer("AsignacionIdentificadorInexistente.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(1, _resultList.Count);

                Assert.AreEqual("[4,2] SEM: No se encontró el identificador 'M'.", _resultList[0]);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\EjemploSinErrores.txt")]
        public void EjemploSinErroresTest()
        {
            using (var lexer = new FTCCompiler.Lexer("EjemploSinErrores.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                using (var tw = File.CreateText(@"D:\syntaxTree.xml"))
                {
                    tree.Serialize(tw.BaseStream);
                }

                Assert.AreEqual(false, tree.HasErrors);
                Assert.AreEqual(0, _resultList.Count);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\AsignacionArrayCorrecta.txt")]
        public void AsignacionArrayCorrectaTest()
        {
            using (var lexer = new FTCCompiler.Lexer("AsignacionArrayCorrecta.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(false, tree.HasErrors);
                Assert.AreEqual(0, _resultList.Count);
            }
        }

        [TestMethod]
        [DeploymentItem(@"Semantic\Sources\AsignacionArrayIncorrecta.txt")]
        public void AsignacionArrayIncorrectaTest()
        {
            using (var lexer = new FTCCompiler.Lexer("AsignacionArrayIncorrecta.txt"))
            {
                var parser = new FTCCompiler.Parser(lexer, (s) => _resultList.Add(s));

                var tree = parser.Parse();

                Assert.AreEqual(true, tree.HasErrors);
                Assert.AreEqual(3, _resultList.Count);

                Assert.AreEqual("[7,11] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[0]);
                Assert.AreEqual("[8,8] SEM: No se encontró el identificador 'X'.", _resultList[1]);
                Assert.AreEqual("[9,8] SEM: Se esperaba una expresión 'Integer' y se encontró una del tipo 'Boolean'.", _resultList[2]);
            }
        }
    }
}
