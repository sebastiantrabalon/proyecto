using FTCCompiler.Common;
using System;
using System.IO;

namespace FTCCompiler.Utils
{
    class FileReader : IDisposable
    {
        private StreamReader _fileReader;
        private int _lineCounter = 1;
        private int _columnCounter = 0;

        public FileReader(string sourceFile)
        {
            _fileReader = new StreamReader(new BufferedStream(File.OpenRead(sourceFile)));
        }

        public Character GetNextChar()
        {
            var @char = new Character(_fileReader.Read());

            if (!@char.IsEof())
            {
                // Cuando encuentro un CR
                if (@char.IsSeparator(Separators.CR))
                {
                    // Leo el LF que deberia venir despues, descartandolo
                    _fileReader.Read();

                    // Ajusto los valores de linea y columna
                    _lineCounter++;
                    _columnCounter = 0;

                    // Retorno un valor ficticio que equivale a un salto de linea ('\n')
                    return new Character((int)Separators.CRLF);
                }

                _columnCounter++;
            }

            return @char;
        }

        public Character PeekNextChar()
        {
            var @char = new Character(_fileReader.Peek());

            if (@char.IsSeparator(Separators.CR))
                return new Character((int)Separators.CRLF);

            return @char;
        }

        public int Line 
        { 
            get { return _lineCounter; } 
        }

        public int Column 
        { 
            get { return _columnCounter; } 
        }

        public void Dispose()
        {
            if (_fileReader != null)
            {
                _fileReader.Close();
                _fileReader = null;
            }
        }
    }
}
