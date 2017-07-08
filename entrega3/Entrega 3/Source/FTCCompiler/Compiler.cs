using System;
using System.IO;

namespace FTCCompiler
{
    public class Compiler
    {
        private string _inputFile;
        private string _outputFile;
        private Action<string> _log;

        public Compiler(string inputFile, string outputFile, Action<string> logger)
        {
            _inputFile = inputFile;
            _outputFile = outputFile;
            _log = logger;
        }

        public bool Run()
        {
            if (!File.Exists(_inputFile))
            {
                _log(string.Format("ERROR: No se encontró el archivo '{0}'.", _inputFile));
                return false;
            }

            if (File.Exists(_outputFile))
            {
                _log(string.Format("ERROR: Ya existe el archivo '{0}'.", _outputFile));
                return false;
            }

            using (var lexer = new Lexer(_inputFile))
            {
                var parser = new Parser(lexer, Console.WriteLine);

                var tree = parser.Parse();

                using (var tw = File.CreateText(_outputFile))
                {
                    tree.Serialize(tw.BaseStream);
                }
            }

            return true;
        }
    }
}
