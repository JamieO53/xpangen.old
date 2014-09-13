// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public interface ISubClassBase : IList<GenObject>
    {
        /// <summary>
        /// The generator data containing the list.
        /// </summary>
        GenDataBase GenDataBase { get; }

        /// <summary>
        /// The generator object owning the list.
        /// </summary>
        GenObject Parent { get; }

        /// <summary>
        /// The ID of objects in the list.
        /// </summary>
        int ClassId { get; }

        /// <summary>
        /// The data reference of the list
        /// </summary>
        string Reference { get; set; }
        
        /// <summary>
        /// The definition of the subclass
        /// </summary>
        GenDataDefSubClass Definition { get; }

        /// <summary>
        /// Reset the underlying data to force it to be reinitialized.
        /// </summary>
        void Reset();
    }
}