// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    public class GenApplicationList<T> : GenList<T> where T : GenApplicationBase
    {
        public override bool Move(ListMove move, int itemIndex)
        {
            var item = this[itemIndex] as GenApplicationBase;
            if (item == null) return base.Move(move, itemIndex);
            var genObject = item.GenObject as GenObject;
            if (genObject == null) return base.Move(move, itemIndex);
            var genList = (GenSubClass) ((GenObject)item.GenObject).ParentSubClass;
            genList.Move(move, itemIndex);

            return base.Move(move, itemIndex);
        }
    }
}
