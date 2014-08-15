using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    /// <summary>
    /// A list of generator class definitions
    /// </summary>
    public class GenDataDefClassList : List<GenDataDefClass>
    {
        private Dictionary<string, int> _lookup = new Dictionary<string, int>();
        /// <summary>
        /// Checks if the list contains a class with the given name.
        /// </summary>
        /// <param name="name">The class name being sought</param>
        /// <returns>The class exists</returns>
        public bool Contains(string name)
        {
            return IndexOf(name) != -1;
        }

        public int IndexOf(string name)
        {
            if (_lookup.ContainsKey(name)) return _lookup[name];
            for (var index = 0; index < Count; index++)
            {
                var item = this[index];
                if (item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    _lookup.Add(name, index);
                    return index;
                }
            }
            return -1;
        }

    }
}
