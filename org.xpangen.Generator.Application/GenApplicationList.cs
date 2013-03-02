// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Application
{
    public class GenApplicationList<T> : GenList<T>
    {
        public override void Move(ListMove move, int itemIndex)
        {

            var item = this[itemIndex] as GenApplicationBase;
            if (item != null)
            {
                var genList = item.GenObject.ParentSubClass;
                genList.MoveItem(move, itemIndex);
            }

            base.Move(move, itemIndex);
        }
    }
}
