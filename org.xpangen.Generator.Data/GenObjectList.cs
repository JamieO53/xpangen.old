// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenObjectList : List<GenObject>
    {
        /// <summary>
        /// Create a new <see cref="GenObjectList"/> list.
        /// </summary>
        /// <param name="data">The generator data containing the list.</param>
        /// <param name="parent">The generator object owning the list.</param>
        /// <param name="classId">The ID of objects in the list.</param>
        public GenObjectList(GenData data, GenObject parent, int classId)
        {
            GenData = data;
            Parent = parent;
            ClassId = classId;
        }

        /// <summary>
        /// The currently selected item.
        /// </summary>
        public GenObject Context
        {
            get
            {
                return Eol ? null : this[Index];
            }
        }

        /// <summary>
        /// Is the current object at the end of the list?
        /// </summary>
        public bool Eol
        {
            get { return Count == 0 || Index < 0 || Index >= Count; }
        }

        /// <summary>
        /// The generator data containing the list.
        /// </summary>
        private GenData GenData { get; set; }

        /// <summary>
        /// The generator object owning the list.
        /// </summary>
        public GenObject Parent { get; private set; }

        /// <summary>
        /// The ID of objects in the list.
        /// </summary>
        public int ClassId { get; private set; }

        /// <summary>
        /// The index of the currently selected item.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Is the selected item in the first place?
        /// </summary>
        /// <returns></returns>
        public bool IsFirst()
        {
            return Index == 0 && Count > 0;
        }

        /// <summary>
        /// Is the selected item in the last place?
        /// </summary>
        /// <returns></returns>
        public bool IsLast()
        {
            return Index == Count - 1 && Count > 0;
        }

        /// <summary>
        /// Make the first item, if any, the current selection.
        /// </summary>
        public void First()
        {
            if (Count == 0)
                Reset();
            else
                Index = 0;
        }

        /// <summary>
        /// Make the next item the current selection.
        /// </summary>
        public void Next()
        {
            Index++;
            if (Eol)
                Reset();
        }

        /// <summary>
        /// Make the last item, if any, the current selection.
        /// </summary>
        public void Last()
        {
            Index = Count - 1;
        }

        /// <summary>
        /// Make the prior item, if any, the current selection.
        /// </summary>
        public void Prior()
        {
            Index--;
            if (Eol)
                Reset();
        }

        /// <summary>
        /// Reset the current selection.
        /// </summary>
        private void Reset()
        {
            Index = -1;
        }

        /// <summary>
        /// Create a new <see cref="GenObject"/> and add it to the list.
        /// </summary>
        /// <returns>The new item.</returns>
        public GenObject CreateObject()
        {
            var o = new GenObject(Parent, this, ClassId);
            Add(o);
            return o;

        }

        /// <summary>
        /// Move the specified item up, down, to the top or to the bottom of the list.
        /// </summary>
        /// <param name="move">The move to be made.</param>
        /// <param name="itemIndex">The item to be moved.</param>
        public void MoveItem(ListMove move, int itemIndex)
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
            Index = 0;
            GenData.RaiseDataChanged(GenData.GenDataDef.Classes[ClassId].Name, "");
            GenData.Changed = true;
        }

        private void MoveUp(int itemIndex)
        {
            if (itemIndex <= 0 || itemIndex >= Count) return;
            var genObject = this[itemIndex];
            this[itemIndex] = this[itemIndex - 1];
            this[itemIndex - 1] = genObject;
            Index = itemIndex - 1;
            GenData.RaiseDataChanged(GenData.GenDataDef.Classes[ClassId].Name, "");
            GenData.Changed = true;
        }

        private void MoveDown(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= Count - 1) return;
            var genObject = this[itemIndex];
            this[itemIndex] = this[itemIndex + 1];
            this[itemIndex + 1] = genObject;
            Index = itemIndex + 1;
            GenData.RaiseDataChanged(GenData.GenDataDef.Classes[ClassId].Name, "");
            GenData.Changed = true;
        }

        private void MoveToBottom(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= Count - 1) return;
            var genObject = this[itemIndex];
            RemoveAt(itemIndex);
            Add(genObject);
            Index = Count - 1;
            GenData.RaiseDataChanged(GenData.GenDataDef.Classes[ClassId].Name, "");
            GenData.Changed = true;
        }
    }
}
