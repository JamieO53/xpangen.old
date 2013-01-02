using System.Collections.Generic;
using System.Globalization;

namespace org.xpangen.Generator.Profile
{
    public class GenFunctionCounters : IGeneratorLibrary
    {
        private readonly Dictionary<string, long> _map = new Dictionary<string, long>();
        private static GenFunctionCounters _instance;

        private GenFunctionCounters()
        {
            
        }

        /// <summary>
        /// Sets the value of the counter to the given value or zero.
        /// </summary>
        /// <param name="name">The counter.</param>
        /// <param name="value">The value being set, or zero.</param>
        /// <returns>An empty string or an error message.</returns>
        private string Set(string name, string value)
        {
            if (value == "")
                value = "0";
            if (!GenUtilities.IsNumeric(value))
                return "<<<<< Set: Counter '" + value + "' is not numeric >>>>>";
            _map[name] = long.Parse(value);
            return "";
        }

        /// <summary>
        /// Gets the value of the counter.
        /// </summary>
        /// <param name="name">The counter.</param>
        /// <returns>The value of the counter or an error message.</returns>
        private string Get(string name)
        {
            if (!_map.ContainsKey(name))
                return "<<<<< Get: Counter " + name + " not defined >>>>>";
            return _map[name].ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Adds the value or 1 to the named counter.
        /// </summary>
        /// <param name="name">The counter.</param>
        /// <param name="value">The amount being added or 1.</param>
        /// <returns>An empty string or an error message.</returns>
        private string Add(string name, string value)
        {
            if (!_map.ContainsKey(name))
                return "<<<<< Add: Counter " + name + " not defined >>>>>";
            if (value == "")
                value = "1";
            else if (!GenUtilities.IsNumeric(value))
                return "<<<<< Add: Counter '" + value + "' is not numeric >>>>>";

            _map[name] = _map[name] + long.Parse(value);
            return "";
        }

        /// <summary>
        /// Subtracts the value or 1 from the named counter.
        /// </summary>
        /// <param name="name">The counter.</param>
        /// <param name="value">The amount being subtracted - 1 if blank.</param>
        /// <returns>An empty string or an error message.</returns>
        private string Sub(string name, string value)
        {
            if (!_map.ContainsKey(name))
                return "<<<<< Sub: Counter " + name + " not defined >>>>>";
            if (value == "")
                value = "1";
            else if (!GenUtilities.IsNumeric(value))
                return "<<<<< Sub: Counter '" + value + "' is not numeric >>>>>";

            _map[name] = _map[name] - long.Parse(value);
            return "";
        }

        /// <summary>
        /// Returns the singleton instance.
        /// </summary>
        /// <returns>The library instance.</returns>
        public static GenFunctionCounters GetInstance()
        {
            return _instance ?? (_instance = new GenFunctionCounters());
        }

        /// <summary>
        /// Returns a list of functions implemented by this class.
        /// </summary>
        /// <returns>The list of function names.</returns>
        public IEnumerable<string> Implements()
        {
            return "Set,Get,Add,Sub".Split(',');
        }

        /// <summary>
        /// Executes the named function with the specified parameters.
        /// </summary>
        /// <param name="function">The name of the function being executed.</param>
        /// <param name="param">The parameters being passed to the function.</param>
        /// <returns>The result of executing the function.</returns>
        public string Execute(string function, string[] param)
        {
            if (param.Length < 0)
                return "<<<<< " + function + " Counter name missing >>>>>";
            
            var name = param[0];
            var value = param.Length >= 2 ? param[1] : "";
            
            switch (function.ToLowerInvariant())
            {
                case "set":
                    return Set(name, value);
                case "get":
                    return Get(name);
                case "add":
                    return Add(name, value);
                case "sub":
                    return Sub(name, value);
                default:
                    return "<<<<< Counter: function " + function + " not implemented >>>>>";
            }
        }
    }
}
