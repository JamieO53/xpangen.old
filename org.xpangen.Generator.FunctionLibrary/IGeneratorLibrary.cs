// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace org.xpangen.Generator.FunctionLibrary
{
    public interface IGeneratorLibrary
    {
        /// <summary>
        ///     Returns a list of functions implemented by this class.
        /// </summary>
        /// <returns>The list of function names.</returns>
        IEnumerable<string> Implements();

        /// <summary>
        ///     Executes the named function with the specified parameters.
        /// </summary>
        /// <param name="function">The name of the function being executed.</param>
        /// <param name="param">The parameters being passed to the function.</param>
        /// <returns>The result of executing the function.</returns>
        string Execute(string function, string[] param);
    }
}