// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenApplicationList<T> : GenList<T> where T : GenApplicationBase
    {
        private Dictionary<string, object> _links;
        public override void Move(ListMove move, int itemIndex)
        {
            var item = this[itemIndex] as GenApplicationBase;
            if (item != null)
            {
                var genObject = item.GenObject as GenObject;
                if (genObject != null)
                {
                    var genList = new GenObjectList(genObject.ParentSubClass, item.GenObject.GenDataBase,
                                                    item.GenData.Context[item.ClassId],
                                                    item.GenData.Context[item.ClassId].DefSubClass);
                    genList.MoveItem(move, itemIndex);
                }
            }

            base.Move(move, itemIndex);
        }

        public Dictionary<string, object> Links
        {
            get { return _links ?? (_links = new Dictionary<string, object>()); }
        }
    }
}
