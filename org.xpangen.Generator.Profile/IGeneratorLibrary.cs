using System.Collections.Generic;

namespace org.xpangen.Generator.Profile
{
    public interface IGeneratorLibrary
    {
        /// <summary>
        /// Returns a list of functions implemented by this class.
        /// </summary>
        /// <returns>The list of function names.</returns>
        IEnumerable<string> Implements();
        /// <summary>
        /// Executes the named function with the specified parameters.
        /// </summary>
        /// <param name="function">The name of the function being executed.</param>
        /// <param name="param">The parameters being passed to the function.</param>
        /// <returns>The result of executing the function.</returns>
        string Execute(string function, string[] param);
    }
}