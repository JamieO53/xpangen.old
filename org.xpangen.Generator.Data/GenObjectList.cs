// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace org.xpangen.Generator.Data
{
    public class GenObjectList
    {
        /// <summary>
        /// Create a new <see cref="GenObjectList"/> list.
        /// </summary>
        /// <param name="genObjectListBase"> The underlying generator object list. </param>
        public GenObjectList(IGenObjectListBase genObjectListBase)
        {
            GenObjectListBase = genObjectListBase;
            ClassIdOffset = 0;
        }


        public IGenObjectListBase GenObjectListBase { get; set; }

        public int ClassIdOffset { get; set; }

        public int ClassId { get { return GenObjectListBase.ClassId + ClassIdOffset; } }

        private GenDataBase GenDataBase { get { return GenObjectListBase.GenDataBase; } }
        private GenDataDef GenDataDef { get { return GenDataBase.GenDataDef; } }
        public GenDataDefClass DefClass { get { return GenDataDef.Classes[ClassId]; } }

        public GenObject this[int index] { get { return GenObjectListBase[index]; } }
        
        /// <summary>
        /// The currently selected item.
        /// </summary>
        public GenObject GenObject
        {
            get
            {
                return Eol ? null : GenObjectListBase[Index];
            }
        }

        /// <summary>
        /// Is the current object at the end of the list?
        /// </summary>
        public bool Eol
        {
            get { return GenObjectListBase.Count == 0 || Index < 0 || Index >= GenObjectListBase.Count; }
        }

        /// <summary>
        /// The index of the currently selected item.
        /// </summary>
        public int Index { get; set; }

        public int Count
        {
            get { return GenObjectListBase.Count; }
        }

        /// <summary>
        /// Is the selected item in the first place?
        /// </summary>
        /// <returns></returns>
        public bool IsFirst()
        {
            return Index == 0 && GenObjectListBase.Count > 0;
        }

        /// <summary>
        /// Is the selected item in the last place?
        /// </summary>
        /// <returns></returns>
        public bool IsLast()
        {
            return Index == GenObjectListBase.Count - 1 && GenObjectListBase.Count > 0;
        }

        /// <summary>
        /// Make the first item, if any, the current selection.
        /// </summary>
        public void First()
        {
            if (GenObjectListBase.Count == 0)
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
            Index = GenObjectListBase.Count - 1;
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
        public void Reset()
        {
            Index = -1;
            GenObjectListBase.Reset();
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
            if (itemIndex <= 0 || itemIndex >= GenObjectListBase.Count) return;
            var genObject = GenObjectListBase[itemIndex];
            GenObjectListBase.RemoveAt(itemIndex);
            GenObjectListBase.Insert(0, genObject);
            Index = 0;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.Classes[ClassId].Name, "");
            GenDataBase.Changed = true;
        }

        private void MoveUp(int itemIndex)
        {
            if (itemIndex <= 0 || itemIndex >= GenObjectListBase.Count) return;
            var genObject = GenObjectListBase[itemIndex];
            GenObjectListBase[itemIndex] = GenObjectListBase[itemIndex - 1];
            GenObjectListBase[itemIndex - 1] = genObject;
            Index = itemIndex - 1;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.Classes[ClassId].Name, "");
            GenDataBase.Changed = true;
        }

        private void MoveDown(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= GenObjectListBase.Count - 1) return;
            var genObject = GenObjectListBase[itemIndex];
            GenObjectListBase[itemIndex] = GenObjectListBase[itemIndex + 1];
            GenObjectListBase[itemIndex + 1] = genObject;
            Index = itemIndex + 1;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.Classes[ClassId].Name, "");
            GenDataBase.Changed = true;
        }

        private void MoveToBottom(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= GenObjectListBase.Count - 1) return;
            var genObject = GenObjectListBase[itemIndex];
            GenObjectListBase.RemoveAt(itemIndex);
            GenObjectListBase.Add(genObject);
            Index = GenObjectListBase.Count - 1;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.Classes[ClassId].Name, "");
            GenDataBase.Changed = true;
        }

        public GenObject CreateObject()
        {
            var genObjectListBase = GenObjectListBase as GenObjectListBase;
            if (genObjectListBase != null)
                return genObjectListBase.CreateObject();
            return null;
        }

        public int IndexOf(GenObject genObject)
        {
            return GenObjectListBase.IndexOf(genObject);
        }
    }
}
