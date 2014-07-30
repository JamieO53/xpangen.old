// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    /// <summary>
    /// A list of generator class definitions
    /// </summary>
    public class GenDataDefClassList : List<IGenDataDefClass>, IGenDataDefClassList
    {
        /// <summary>
        /// Checks if the list contains a class with the given name.
        /// </summary>
        /// <param name="name">The class name being sought.</param>
        /// <returns>The class exists?</returns>
        public bool Contains(string name)
        {
            foreach (var item in this)
                if (item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            return false;
        }

        /// <summary>
        /// Returns the index of the class with the given name.
        /// </summary>
        /// <param name="name">The class name being sought.</param>
        /// <returns>The index of the item being sought, and -1 if not found.</returns>
        public int IndexOf(string name)
        {
            for (var index = 0; index < Count; index++)
            {
                var item = this[index];
                if (item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return index;
            }
            return -1;
        }

        /// <summary>
        /// Add all the classes in the specified list.
        /// Classes with existing names override the existing class.
        /// </summary>
        /// <param name="classes">The classes to be added.</param>
        public void AddRange(IGenDataDefClassList classes)
        {
            foreach (var c in classes)
                if (!Contains(c.Name))
                    Add(c);
        }
    }
}
