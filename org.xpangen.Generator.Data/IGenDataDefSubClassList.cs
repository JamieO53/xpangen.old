// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public interface IGenDataDefSubClassList : IList<IGenDataDefSubClass>
    {
        /// <summary>
        /// Returns the index of the subclass with the given name.
        /// </summary>
        /// <param name="name">The name of the subclass being sought.</param>
        /// <returns>The index of the sought item, or -1 if not found.</returns>
        int IndexOf(string name);

        /// <summary>
        /// Returns the index of the subclass with the given ClassID.
        /// </summary>
        /// <param name="subClassId">The ClassID of the subclass being sought.</param>
        /// <returns>The index of the sought item, or -1 if not found.</returns>
        int IndexOf(int subClassId);
    }
}