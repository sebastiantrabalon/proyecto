@echo off

echo . Corriendo tests...

FTCCompiler In\ArrayIndiceExpresionNoEntera.txt Out\ArrayIndiceExpresionNoEntera.xml > Out\ArrayIndiceExpresionNoEntera.txt
FTCCompiler In\AsignacionArrayCorrecta.txt Out\AsignacionArrayCorrecta.xml > Out\AsignacionArrayCorrecta.txt 
FTCCompiler In\AsignacionArrayIncorrecta.txt Out\AsignacionArrayIncorrecta.xml > Out\AsignacionArrayIncorrecta.txt 
FTCCompiler In\AsignacionDistintoTipoDatoAsignable.txt Out\AsignacionDistintoTipoDatoAsignable.xml > Out\AsignacionDistintoTipoDatoAsignable.txt 
FTCCompiler In\AsignacionExpresionAConstante.txt Out\AsignacionExpresionAConstante.xml > Out\AsignacionExpresionAConstante.txt 
FTCCompiler In\AsignacionIdentificadorFueraDeScope.txt Out\AsignacionIdentificadorFueraDeScope.xml > Out\AsignacionIdentificadorFueraDeScope.txt 
FTCCompiler In\AsignacionIdentificadorInexistente.txt Out\AsignacionIdentificadorInexistente.xml > Out\AsignacionIdentificadorInexistente.txt 
FTCCompiler In\DeclaracionDeArrayFueraDeEntornoGlobal.txt Out\DeclaracionDeArrayFueraDeEntornoGlobal.xml > Out\DeclaracionDeArrayFueraDeEntornoGlobal.txt 
FTCCompiler In\DeclaracionParametrosDuplicados.txt Out\DeclaracionParametrosDuplicados.xml > Out\DeclaracionParametrosDuplicados.txt 
FTCCompiler In\EjemploSinErrores.txt Out\EjemploSinErrores.xml > Out\EjemploSinErrores.txt 
FTCCompiler In\FuncionInexistente.txt Out\FuncionInexistente.xml > Out\FuncionInexistente.txt 
FTCCompiler In\FuncionRetornoNoCoincideConTipoDatoRetorno.txt Out\FuncionRetornoNoCoincideConTipoDatoRetorno.xml > Out\FuncionRetornoNoCoincideConTipoDatoRetorno.txt 
FTCCompiler In\FuncionUsaParametros.txt Out\FuncionUsaParametros.xml > Out\FuncionUsaParametros.txt 
FTCCompiler In\IdentificadorDuplicadoDiferenteEntorno.txt Out\IdentificadorDuplicadoDiferenteEntorno.xml > Out\IdentificadorDuplicadoDiferenteEntorno.txt 
FTCCompiler In\IdentificadorDuplicadoMismoEntorno.txt Out\IdentificadorDuplicadoMismoEntorno.xml > Out\IdentificadorDuplicadoMismoEntorno.txt 
FTCCompiler In\IfCondicionFuncionBoolean.txt Out\IfCondicionFuncionBoolean.xml > Out\IfCondicionFuncionBoolean.txt 
FTCCompiler In\IfCondicionFuncionInteger.txt Out\IfCondicionFuncionInteger.xml > Out\IfCondicionFuncionInteger.txt 
FTCCompiler In\IfCondicionNoBooleana.txt Out\IfCondicionNoBooleana.xml > Out\IfCondicionNoBooleana.txt 
FTCCompiler In\IfCondicionProcedimiento.txt Out\IfCondicionProcedimiento.xml > Out\IfCondicionProcedimiento.txt 
FTCCompiler In\LlamadasRecursivas.txt Out\LlamadasRecursivas.xml > Out\LlamadasRecursivas.txt 
FTCCompiler In\LlamarProcEnEntornoLocal.txt Out\LlamarProcEnEntornoLocal.xml > Out\LlamarProcEnEntornoLocal.txt 
FTCCompiler In\MainConEntornoLocal.txt  Out\MainConEntornoLocal.xml > Out\MainConEntornoLocal.txt 
FTCCompiler In\MainConParametros.txt  Out\MainConParametros.xml > Out\MainConParametros.txt 
FTCCompiler In\OperacionBinariaDistintosTipoDeDatos.txt Out\OperacionBinariaDistintosTipoDeDatos.xml > Out\OperacionBinariaDistintosTipoDeDatos.txt 
FTCCompiler In\OperacionDivisionNoEnterosEnCondicion.txt Out\OperacionDivisionNoEnterosEnCondicion.xml > Out\OperacionDivisionNoEnterosEnCondicion.txt 
FTCCompiler In\OperacionMayorIgualNoEnterosEnCondicion.txt Out\OperacionMayorIgualNoEnterosEnCondicion.xml > Out\OperacionMayorIgualNoEnterosEnCondicion.txt 
FTCCompiler In\OperacionMayorNoEnterosEnCondicion.txt Out\OperacionMayorNoEnterosEnCondicion.xml > Out\OperacionMayorNoEnterosEnCondicion.txt 
FTCCompiler In\OperacionMenorIgualNoEnterosEnCondicion.txt Out\OperacionMenorIgualNoEnterosEnCondicion.xml > Out\OperacionMenorIgualNoEnterosEnCondicion.txt 
FTCCompiler In\OperacionMenorNoEnterosEnCondicion.txt Out\OperacionMenorNoEnterosEnCondicion.xml > Out\OperacionMenorNoEnterosEnCondicion.txt 
FTCCompiler In\OperacionMultiplicacionNoEnterosEnCondicion.txt Out\OperacionMultiplicacionNoEnterosEnCondicion.xml > Out\OperacionMultiplicacionNoEnterosEnCondicion.txt 
FTCCompiler In\OperacionRestaNoEnterosEnCondicion.txt Out\OperacionRestaNoEnterosEnCondicion.xml > Out\OperacionRestaNoEnterosEnCondicion.txt 
FTCCompiler In\OperacionSumaNoEnterosEnCondicion.txt Out\OperacionSumaNoEnterosEnCondicion.xml > Out\OperacionSumaNoEnterosEnCondicion.txt 
FTCCompiler In\OperadorAndNoBooleanoEnCondicion.txt Out\OperadorAndNoBooleanoEnCondicion.xml > Out\OperadorAndNoBooleanoEnCondicion.txt 
FTCCompiler In\OperadorOrNoBooleanoEnCondicion.txt Out\OperadorOrNoBooleanoEnCondicion.xml > Out\OperadorOrNoBooleanoEnCondicion.txt 
FTCCompiler In\OperadorXorNoBooleanoEnCondicion.txt Out\OperadorXorNoBooleanoEnCondicion.xml > Out\OperadorXorNoBooleanoEnCondicion.txt 
FTCCompiler In\PasajeParametrosDistintoTiposSegunVariable.txt Out\PasajeParametrosDistintoTiposSegunVariable.xml > Out\PasajeParametrosDistintoTiposSegunVariable.txt 
FTCCompiler In\PasajeParametrosFueraDeScope.txt Out\PasajeParametrosFueraDeScope.xml > Out\PasajeParametrosFueraDeScope.txt 
FTCCompiler In\PasajeParametrosIdentificadorInexistente.txt Out\PasajeParametrosIdentificadorInexistente.xml > Out\PasajeParametrosIdentificadorInexistente.txt 
FTCCompiler In\PasajeParametrosMenosParametros.txt Out\PasajeParametrosMenosParametros.xml > Out\PasajeParametrosMenosParametros.txt 
FTCCompiler In\PasajeParametrosTipoArray.txt Out\PasajeParametrosTipoArray.xml > Out\PasajeParametrosTipoArray.txt 
FTCCompiler In\ProcedimientoInexistente.txt Out\ProcedimientoInexistente.xml > Out\ProcedimientoInexistente.txt 
FTCCompiler In\ReadIdentificadorInexistente.txt Out\ReadIdentificadorInexistente.xml > Out\ReadIdentificadorInexistente.txt 
FTCCompiler In\ShowFuncionInexistente.txt Out\ShowFuncionInexistente.xml > Out\ShowFuncionInexistente.txt 
FTCCompiler In\ShowIdentificadorInexistente.txt Out\ShowIdentificadorInexistente.xml > Out\ShowIdentificadorInexistente.txt 
FTCCompiler In\WhileCondicionFuncionBoolean.txt Out\WhileCondicionFuncionBoolean.xml > Out\WhileCondicionFuncionBoolean.txt 
FTCCompiler In\WhileCondicionFuncionInteger.txt Out\WhileCondicionFuncionInteger.xml > Out\WhileCondicionFuncionInteger.txt 
FTCCompiler In\WhileCondicionNoBooleana.txt Out\WhileCondicionNoBooleana.xml > Out\WhileCondicionNoBooleana.txt 
FTCCompiler In\WhileCondicionProcedimiento.txt Out\WhileCondicionProcedimiento.xml > Out\WhileCondicionProcedimiento.txt 

echo . Terminado. Vea resultados en directorio Out.
