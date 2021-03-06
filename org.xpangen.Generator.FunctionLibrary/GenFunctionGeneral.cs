// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.FunctionLibrary
{
    public class GenFunctionGeneral : IGeneratorLibrary
    {
        private static GenFunctionGeneral _instance;

        private GenFunctionGeneral()
        {
        }

        /// <summary>
        ///     Returns the singleton instance.
        /// </summary>
        /// <returns>The library instance.</returns>
        public static GenFunctionGeneral GetInstance()
        {
            return _instance ?? (_instance = new GenFunctionGeneral());
        }

        /// <summary>
        ///     Returns a list of functions implemented by this class.
        /// </summary>
        /// <returns>The list of function names.</returns>
        public IEnumerable<string> Implements()
        {
            return
                "CutString,Date,File,QuoteString,StringOrName,Time,UnIdentifier,UnIdentifierLC,Decapitalize,Cond,Contains".Split(',');
        }

        /// <summary>
        ///     Checks if the first parameter contains the second. The comparison is case insensitive.
        /// </summary>
        /// <param name="container">The text being examined.</param>
        /// <param name="contained">The text being sought.</param>
        /// <returns>The container text if the sought text is contained, and an empty string otherwise.</returns>
        private static string Contains(string container, string contained)
        {
            return GenUtilities.Contains(container, contained);
        }

        /// <summary>
        ///     Checks the condition, and if not empty returns the second parameter else the third.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="value0">The value returned if the condition is not empty.</param>
        /// <param name="value1">The value returned if the condition is empty.</param>
        /// <returns>The conditional string.</returns>
        private static string Cond(string condition, string value0, string value1)
        {
            return GenUtilities.Cond(condition, value0, value1);
        }

        /// <summary>
        ///     Cuts the specified substring out of the given string.
        /// </summary>
        /// <param name="value1">The original string.</param>
        /// <param name="value2">The string being cut out.</param>
        /// <returns>The edited string.</returns>
        private static string CutString(string value1, string value2)
        {
            return GenUtilities.CutString(value1, value2);
        }

        /// <summary>
        ///     Returns the current long format date.
        /// </summary>
        /// <returns>Today.</returns>
        private static string Date()
        {
            return DateTime.Today.ToString("D");
        }

        /// <summary>
        ///     This method is a stub, simply returning the file name on a new line.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The formatted file name.</returns>
        private static string File(string fileName)
        {
            return "\r\n //File: " + fileName;
        }

        /// <summary>
        ///     Surrounds the given string with single quotes. Embedded quotes are doubled.
        /// </summary>
        /// <param name="value">The string being quoted.</param>
        /// <returns>The quoted string.</returns>
        private static string QuoteString(string value)
        {
            return GenUtilities.QuoteString(value);
        }

        /// <summary>
        ///     Determines if the given string is a valid identifier, and if not to surround it with quotes.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string StringOrName(string value)
        {
            return GenUtilities.StringOrName(value);
        }

        /// <summary>
        ///     Returns the current short format time.
        /// </summary>
        /// <returns>Now.</returns>
        private static string Time()
        {
            return DateTime.Now.ToString("t");
        }

        /// <summary>
        ///     Turns an identifier into a list of words. Words are identified by capitals (Camel Case), numerics and underscores or hyphens.
        /// </summary>
        /// <param name="value">The identifier being transformed.</param>
        /// <returns>The transformed identfier text.</returns>
        private static string UnIdentifier(string value)
        {
            return GenUtilities.UnIdentifier(value);
        }

        /// <summary>
        ///     Turns an identifier into a list of words. Words are identified by capitals (Camel Case), numerics and underscores or hyphens.
        ///     Words after the first are decapitalized.
        /// </summary>
        /// <param name="value">The identifier being transformed.</param>
        /// <returns>The transformed identfier text.</returns>
        private static string UnIdentifierLc(string value)
        {
            return GenUtilities.UnIdentifierLc(value);
        }

        /// <summary>
        ///     Decapitalize the first character of an identifier.
        /// </summary>
        /// <param name="value">The identifier being transformed.</param>
        /// <returns>The transformed identfier text.</returns>
        private static string Decapitalize(string value)
        {
            return GenUtilities.Decapitalize(value);
        }

        /// <summary>
        ///     Executes the named function with the specified parameters.
        /// </summary>
        /// <param name="function">The name of the function being executed.</param>
        /// <param name="param">The parameters being passed to the function.</param>
        /// <returns>The result of executing the function.</returns>
        public string Execute(string function, string[] param)
        {
            var name = "";
            if (param.Length > 0)
                name = param[0];
            var value0 = "";
            if (param.Length > 1)
                value0 = param[1];
            var value1 = "";
            if (param.Length > 2)
                value1 = param[2];

            switch (function.ToLowerInvariant())
            {
                case "cond":
                    return Cond(name, value0, value1);
                case "contains":
                    return Contains(name, value0);
                case "cutstring":
                    return CutString(name, value0);
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
                case "unidentifierlc":
                    return UnIdentifierLc(name);
                case "decapitalize":
                    return Decapitalize(name);
                default:
                    return "<<<<< General: function " + function + " not implemented >>>>>";
            }
        }
    }
}