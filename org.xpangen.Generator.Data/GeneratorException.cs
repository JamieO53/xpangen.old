using System;

namespace org.xpangen.Generator.Data
{
    public class GeneratorException : ApplicationException
    {
        public GeneratorException(string message, GenErrorType errorType = GenErrorType.Unspecified)
        {
            GenErrorType = errorType;
        }

        public GenErrorType GenErrorType { get; private set; }
    }
}