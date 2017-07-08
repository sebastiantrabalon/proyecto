using System;
using System.IO;

namespace FTCCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("================================================================================");
            Console.WriteLine("==  Compilador FTC :: 2014 :: Procesadores de Lenguajes :: Universidad CAECE  ==");
            Console.WriteLine("================================================================================");

            if (args.Length != 2)
            {
                Console.WriteLine("USO: ftccompiler <archivo entrada> <archivo salida>");
                return;
            }

            var compiler = new Compiler(args[0], args[1], Log);

            Console.WriteLine(string.Format("Compilando '{0}'...", args[0]));

            if (compiler.Run())
            {
                //Console.WriteLine(string.Format("Terminado. El archivo '{1}' se generó exitosamente.", args[2]));
                return;
            }

            //Console.WriteLine("Finalizado con errores.");
        }

        static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
