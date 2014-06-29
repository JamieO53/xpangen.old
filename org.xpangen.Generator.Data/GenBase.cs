namespace org.xpangen.Generator.Data
{
    public class GenBase
    {
        protected static void Assert(bool condition, string message)
        {
            if (!condition) throw new GeneratorException(message, GenErrorType.Assertion);
        }
    }
}