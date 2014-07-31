using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    /// <summary>
    /// A list of generator class definitions
    /// </summary>
    public class GenDataDefClassList : List<GenDataDefClass>
    {
        /// <summary>
        /// Checks if the list contains a class with the given name.
        /// </summary>
        /// <param name="name">The class name being sought</param>
        /// <returns>The class exists</returns>
        public bool Contains(string name)
        {
            foreach (var item in this)
                if (item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            return false;
        }

        public int IndexOf(string name)
        {
            for (var index = 0; index < this.Count; index++)
            {
                var item = this[index];
                if (item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return index;
            }
            return -1;
        }

    }
}
