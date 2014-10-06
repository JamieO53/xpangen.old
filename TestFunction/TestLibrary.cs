// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using org.xpangen.Generator.FunctionLibrary;

namespace TestFunction
{
    public class TestLibrary : IGeneratorLibrary
    {
        private static TestLibrary _instance;

        /// <summary>
        /// Hide constructor for singleton pattern.
        /// </summary>
        private TestLibrary()
        {
            
        }
        
        /// <summary>
        ///     Returns a list of functions implemented by this class.
        /// </summary>
        /// <returns>The list of function names.</returns>
        public IEnumerable<string> Implements()
        {
            return new [] { "TestFunction" };
        }

        /// <summary>
        ///     Returns the singleton instance.
        /// </summary>
        /// <returns>The library instance.</returns>
        public static TestLibrary GetInstance()
        {
            return _instance ?? (_instance = new TestLibrary());
        }

        private string TestFunction(string param)
        {
            return param;
        }
        
        /// <summary>
        ///     Executes the named function with the specified parameters.
        /// </summary>
        /// <param name="function">The name of the function being executed.</param>
        /// <param name="param">The parameters being passed to the function.</param>
        /// <returns>The result of executing the function.</returns>
        public string Execute(string function, string[] param)
        {
            var parameter = param.Length > 0 ? param[0] : "";
            switch (function.ToLowerInvariant())
            {
                case "testfunction":
                    return TestFunction(parameter);
                default:
                    return "<<<<< TestLibrary: " + function + " not implemented >>>>>";
            }
        }
    }
}
