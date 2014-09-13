// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenDataDefSubClassList : List<GenDataDefSubClass>
    {
        public int IndexOf(string name)
        {
            for (var index = 0; index < Count; index++)
            {
                var item = this[index];
                if (item.SubClass.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return index;
            }
            return -1;
        }

        public int IndexOf(int subClassId)
        {
            for (var index = 0; index < Count; index++)
            {
                var item = this[index];
                if (item.SubClass.ClassId.Equals(subClassId))
                    return index;
            }
            return -1;
        }
    }
}