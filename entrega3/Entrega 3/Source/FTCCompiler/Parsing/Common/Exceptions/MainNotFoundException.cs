using FTCCompiler.Resources;
using System;

namespace FTCCompiler.Parsing.Common.Exceptions
{
    class MainNotFoundException : Exception
    {
        public MainNotFoundException() 
            : base(Messages.MainNotFoundMessage)
        {
        }
    }
}
