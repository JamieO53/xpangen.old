// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    /// <summary>
    /// Extends List&lt;T&gt; with move operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenList<T> : List<T>
    {
        public virtual bool Move(ListMove move, int itemIndex)
        {
            switch (move)
            {
                case ListMove.ToTop:
                    return MoveToTop(itemIndex);
                case ListMove.Up:
                    return MoveUp(itemIndex);
                case ListMove.Down:
                    return MoveDown(itemIndex);
                case ListMove.ToBottom:
                    return MoveToBottom(itemIndex);
                default:
                    throw new ArgumentOutOfRangeException("move");
            }
        }

        private bool MoveToTop(int itemIndex)
        {
            if (itemIndex <= 0 || itemIndex >= Count) return false;
            var genObject = this[itemIndex];
            RemoveAt(itemIndex);
            Insert(0, genObject);
            return true;
        }

        private bool MoveUp(int itemIndex)
        {
            if (itemIndex <= 0 || itemIndex >= Count) return false;
            var genObject = this[itemIndex];
            this[itemIndex] = this[itemIndex - 1];
            this[itemIndex - 1] = genObject;
            return true;
        }

        private bool MoveDown(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= Count - 1) return false;
            var genObject = this[itemIndex];
            this[itemIndex] = this[itemIndex + 1];
            this[itemIndex + 1] = genObject;
            return true;
        }

        private bool MoveToBottom(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= Count - 1) return false;
            var genObject = this[itemIndex];
            RemoveAt(itemIndex);
            Add(genObject);
            return true;
        }
    }
}
