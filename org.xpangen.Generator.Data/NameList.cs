// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class NameList : List<string>
    {
        private Dictionary<string, int> _names;
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
            if (_names != null) return _names.ContainsKey(item) ? _names[item] : -1;
            var i = 0;
            while (i < Count && String.Compare(this[i], item, StringComparison.OrdinalIgnoreCase) != 0)
                i++;
            return i >= Count ? -1 : i;
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
                if (_names == null)
                {
                    if (Count > 5)
                    {
                        _names = new Dictionary<string, int>();
                        for (var j = 0; j < Count; j++)
                            _names.Add(this[j], j);
                    }
                }
                else _names.Add(item, Count - 1);
            }
            return i;
        }
    }
}
