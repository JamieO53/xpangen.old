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
    public class GenDataDefClassList : List<IGenDataDefClass>
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
            for (var index = 0; index < Count; index++)
            {
                var item = this[index];
                if (item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return index;
            }
            return -1;
        }

    }
}
