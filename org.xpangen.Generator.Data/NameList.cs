// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace org.xpangen.Generator.Data
{
    public class NameList : List<string>
    {
        private readonly Dictionary<string, int> _names = new Dictionary<string, int>();

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire text list. The comparisons are case insensitive.
        /// </summary>
        /// 
        /// <returns>
        /// The zero-based index of the first occurrence of <paramref name="item"/> within the entire text list, if found; otherwise, –1.
        /// </returns>
        /// <param name="item">The object to locate in the text list. The value can be null.</param>
        /// <returns>The index of the added item.</returns>
        public new int IndexOf(string item)
        {
            int result;
            if (_names.TryGetValue(item.ToUpperInvariant(), out result)) return result;
            return -1;
        }

        public new void Clear()
        {
            base.Clear();
            _names.Clear();
        }
        /// <summary>
        /// Adds an object to the end of the string list.
        /// </summary>
        /// <param name="item">The object to be added to the end of the string list. The value can be null.</param>
        public new int Add(string item)
        {
            var i = IndexOf(item);
            if (i == -1)
            {
                i = Count;
                base.Add(item);
                _names.Add(item.ToUpperInvariant(), Count - 1);
            }
            return i;
        }
    }
}
