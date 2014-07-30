// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public interface IGenDataDefClassList : IList<IGenDataDefClass>
    {
        /// <summary>
        /// Checks if the list contains a class with the given name.
        /// </summary>
        /// <param name="name">The class name being sought.</param>
        /// <returns>The class exists?</returns>
        bool Contains(string name);

        /// <summary>
        /// Returns the index of the class with the given name.
        /// </summary>
        /// <param name="name">The class name being sought.</param>
        /// <returns>The index of the item being sought, and -1 if not found.</returns>
        int IndexOf(string name);

        /// <summary>
        /// Add all the classes in the specified list.
        /// Classes with existing names override the existing class.
        /// </summary>
        /// <param name="classes">The classes to be added.</param>
        void AddRange(IGenDataDefClassList classes);
    }
}