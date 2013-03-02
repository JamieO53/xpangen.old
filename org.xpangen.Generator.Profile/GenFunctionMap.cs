// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace org.xpangen.Generator.Profile
{
    public class GenFunctionMap : IGeneratorLibrary
    {
        private readonly Dictionary<string, string> _map = new Dictionary<string, string>();
        private static GenFunctionMap _instance;

        private GenFunctionMap()
        {
            
        }

        /// <summary>
        /// Sets the map value.
        /// </summary>
        /// <param name="name">The name of the map.</param>
        /// <param name="value">The new value of the map.</param>
        /// <returns>A blank string or an error message.</returns>
        private string MapSet(string name, string value)
        {
            _map[name] = value;
            return "";
        }

        /// <summary>
        /// Gets the mapped value.
        /// </summary>
        /// <param name="name">The map name.</param>
        /// <returns>The value of the map, or an error message.</returns>
        private string MapGet(string name)
        {
            if (_map.ContainsKey(name))
                return _map[name];
            return "<<Mapping not set: " + name + ">>";
        }

        /// <summary>
        /// Clears all mapped values.
        /// </summary>
        /// <returns>A blank string.</returns>
        private string MapClear()
        {
            _map.Clear();
            return "";
        }

        /// <summary>
        /// Returns the singleton instance.
        /// </summary>
        /// <returns>The library instance.</returns>
        public static GenFunctionMap GetInstance()
        {
            return _instance ?? (_instance = new GenFunctionMap());
        }

        /// <summary>
        /// Returns a list of functions implemented by this class.
        /// </summary>
        /// <returns>The list of function names.</returns>
        public IEnumerable<string> Implements()
        {
            return "MapSet,MapGet,MapClear".Split(',');
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
                case "mapset":
                    return MapSet(name, value);
                case "mapget":
                    return MapGet(name);
                case "mapclear":
                    return MapClear();
                default:
                    return "<<<<< Map: function " + function + " not implemented >>>>>";
            }
        }
    }
}
