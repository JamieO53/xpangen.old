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
        public virtual void Move(ListMove move, int itemIndex)
        {
            switch (move)
            {
                case ListMove.ToTop:
                    MoveToTop(itemIndex);
                    break;
                case ListMove.Up:
                    MoveUp(itemIndex);
                    break;
                case ListMove.Down:
                    MoveDown(itemIndex);
                    break;
                case ListMove.ToBottom:
                    MoveToBottom(itemIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("move");
            }
        }

        private void MoveToTop(int itemIndex)
        {
            if (itemIndex <= 0 || itemIndex >= Count) return;
            var genObject = this[itemIndex];
            RemoveAt(itemIndex);
            Insert(0, genObject);
        }

        private void MoveUp(int itemIndex)
        {
            if (itemIndex <= 0 || itemIndex >= Count) return;
            var genObject = this[itemIndex];
            this[itemIndex] = this[itemIndex - 1];
            this[itemIndex - 1] = genObject;
        }

        private void MoveDown(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= Count - 1) return;
            var genObject = this[itemIndex];
            this[itemIndex] = this[itemIndex + 1];
            this[itemIndex + 1] = genObject;
        }

        private void MoveToBottom(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= Count - 1) return;
            var genObject = this[itemIndex];
            RemoveAt(itemIndex);
            Add(genObject);
        }
    }
}
