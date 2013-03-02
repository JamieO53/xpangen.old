// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Profile
{
    public class GenFunctionGeneral : IGeneratorLibrary
    {
        private static GenFunctionGeneral _instance;

        private GenFunctionGeneral()
        {
            
        }

        /// <summary>
        /// Returns the singleton instance.
        /// </summary>
        /// <returns>The library instance.</returns>
        public static GenFunctionGeneral GetInstance()
        {
            return _instance ?? (_instance = new GenFunctionGeneral());
        }

        /// <summary>
        /// Returns a list of functions implemented by this class.
        /// </summary>
        /// <returns>The list of function names.</returns>
        public IEnumerable<string> Implements()
        {
            return "CutString,Date,File,QuoteString,StringOrName,Time,UnIdentifier".Split(',');
        }

        /// <summary>
        /// Cust the specified substring out of the given string.
        /// </summary>
        /// <param name="value1">The original string.</param>
        /// <param name="value2">The string being cut out.</param>
        /// <returns>The edited string.</returns>
        private static string CutString(string value1, string value2)
        {
            return GenUtilities.CutString(value1, value2);
        }

        /// <summary>
        /// Returns the current long format date.
        /// </summary>
        /// <returns>Today.</returns>
        private static string Date()
        {
            return DateTime.Today.ToString("D");
        }
        
        /// <summary>
        /// This method is a stub, simply returning the file name on a new line.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The formatted file name.</returns>
        private static string File(string fileName)
        {
            return "\r\n //File: " + fileName;
        }
        
        /// <summary>
        /// Surrounds the given string with single quotes. Embedded quotes are doubled.
        /// </summary>
        /// <param name="value">The string being quoted.</param>
        /// <returns>The quoted string.</returns>
        private static string QuoteString(string value)
        {
            return GenUtilities.QuoteString(value);
        }

        /// <summary>
        /// Determines if the given string is a valid identifier, and if not to surround it with quotes.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string StringOrName(string value)
        {
            return GenUtilities.StringOrName(value);
        }

        /// <summary>
        /// Returns the current short format time.
        /// </summary>
        /// <returns>Now.</returns>
        private static string Time()
        {
            return DateTime.Now.ToString("t");
        }

        /// <summary>
        /// Turns an identifier into a list of words. Words are identified by capitals (Camel Case), numerics and underscores or hyphens.
        /// </summary>
        /// <param name="value">The identifier being transformed.</param>
        /// <returns>The transformed identfier text.</returns>
        private static string UnIdentifier(string value)
        {
            return GenUtilities.UnIdentifier(value);
        }

        /// <summary>
        /// Executes the named function with the specified parameters.
        /// </summary>
        /// <param name="function">The name of the function being executed.</param>
        /// <param name="param">The parameters being passed to the function.</param>
        /// <returns>The result of executing the function.</returns>
        public string Execute(string function, string[] param)
        {
            var name = "";
            if (param.Length > 0)
                name = param[0];
            var value = "";
            if (param.Length > 1)
                value = param[1];

            switch (function.ToLowerInvariant())
            {
                case "cutstring":
                    return CutString(name, value);
                case "date":
                    return Date();
                case "file":
                    return File(name);
                case "quotestring":
                    return QuoteString(name);
                case "stringorname":
                    return StringOrName(name);
                case "time":
                    return Time();
                case "unidentifier":
                    return UnIdentifier(name);
                default:
                    return "<<<<< General: function " + function + " not implemented >>>>>";
            }
        }
    }
}
