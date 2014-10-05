// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace org.xpangen.Generator.Profile
{
    public class ExternalFunctionSection : ConfigurationSection
    {
        public const string SectionName = "ExternalFunctionSection";
        private const string ExternalFunctionClassCollectionName = "ExternalFunctionClassCollection";

        [ConfigurationProperty("ExternalFunctionClassCollection")]
        [ConfigurationCollection(typeof(ExternalFunctionClassCollection), AddItemName = "add")]
        public ExternalFunctionClassCollection ExternalFunctionClassCollection
        {
            get { return (ExternalFunctionClassCollection) base[ExternalFunctionClassCollectionName]; }
        }
    }

    public class ExternalFunctionClassCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExternalFunctionClass();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var xfc = ((ExternalFunctionClass) element);
            return xfc.Assemblyname + "." + xfc.Functionclass;
        }
    }
    public class ExternalFunctionClass : ConfigurationElement
    {
        [ConfigurationProperty("assemblypath", IsRequired = true)]
        public string Assemblypath
        {
            get { return (string) this["assemblypath"]; }
            private set { this["assemblypath"] = value; }
        }
        [ConfigurationProperty("assemblyname", IsRequired = true)]
        public string Assemblyname
        {
            get { return (string)this["assemblyname"]; }
            private set { this["assemblyname"] = value; }
        }
        [ConfigurationProperty("functionclass", IsRequired = true)]
        public string Functionclass 
        {
            get { return (string)this["functionclass"]; }
            private set { this["functionclass"] = value; } 
        }
    }

    public class LibraryManager : IGeneratorLibrary
    {
        private readonly Dictionary<string, IGeneratorLibrary> _map =
            new Dictionary<string, IGeneratorLibrary>();

        private static LibraryManager _instance;

        /// <summary>
        ///     Create a new library manager and registers the standard libraries.
        /// </summary>
        private LibraryManager()
        {
            Register(GenFunctionCounters.GetInstance());
            Register(GenFunctionMap.GetInstance());
            Register(GenFunctionGeneral.GetInstance());
            RegisterExternals();
        }

        private void RegisterExternals()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var s = (ExternalFunctionSection)config.GetSection("ExternalFunctionSection");
            if (s == null)
                return;
            foreach (var functionClass in s.ExternalFunctionClassCollection)
            {
                RegisterExternalFunctionClass((ExternalFunctionClass)functionClass);
            }
        }

        private void RegisterExternalFunctionClass(ExternalFunctionClass externalFunctionClass)
        {
            var assemblypath = externalFunctionClass.Assemblypath;
            var assemblyname = externalFunctionClass.Assemblyname;
            var functionclass = externalFunctionClass.Functionclass;
            RegisterExternalFunctionClass(assemblypath, assemblyname, functionclass);
        }

        private void RegisterExternalFunctionClass(string assemblypath, string assemblyname, string functionclass)
        {
            var dll = Assembly.LoadFile(Path.GetFullPath(assemblypath));
            var c = dll.GetType(assemblyname + "." + functionclass);
            var m = c.GetMethod("GetInstance");
            var instance =
                m.Invoke(null, BindingFlags.Public | BindingFlags.Static, null, null, CultureInfo.InvariantCulture) as
                    IGeneratorLibrary;
            if (instance != null) Register(instance);
        }

        /// <summary>
        ///     Registers the functions implemented by the library. Previously registered functions are overridden.
        /// </summary>
        /// <param name="library">The library instance.</param>
        public void Register(IGeneratorLibrary library)
        {
            foreach (var functionName in library.Implements())
            {
                if (_map.ContainsKey(functionName.ToLowerInvariant()))
                    _map[functionName.ToLowerInvariant()] = library;
                else
                    _map.Add(functionName.ToLowerInvariant(), library);
            }
        }

        /// <summary>
        ///     Returns the singleton instance.
        /// </summary>
        /// <returns>The library instance.</returns>
        public static IGeneratorLibrary GetInstance()
        {
            return _instance ?? (_instance = new LibraryManager());
        }

        /// <summary>
        ///     Returns a list of functions implemented by this class.
        /// </summary>
        /// <returns>The list of function names.</returns>
        public IEnumerable<string> Implements()
        {
            var list = new List<string>();
            foreach (var key in _map.Keys)
                list.Add(key);
            return list.ToArray();
        }

        /// <summary>
        ///     Executes the named function with the specified parameters.
        /// </summary>
        /// <param name="function">The name of the function being executed. This is case insensitive.</param>
        /// <param name="param">The parameters being passed to the function.</param>
        /// <returns>The result of executing the function.</returns>
        public string Execute(string function, string[] param)
        {
            var name = function.ToLowerInvariant();
            if (_map.ContainsKey(name))
                return _map[name].Execute(name, param);
            return "<<<<< Function " + function + " not implemented >>>>>";
        }
    }
}