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
        /// <param name="genObjectListBase"> The underlying generator object list.</param>
        /// <param name="genDataBase">The underlying data container.</param>
        public GenObjectList(IGenObjectListBase genObjectListBase, GenDataBase genDataBase)
        {
            GenDataBase = genDataBase;
            GenObjectListBase = genObjectListBase;
            DefClass = GenObjectListBase.Definition.SubClass;
        }

        public IGenObjectListBase GenObjectListBase { get; set; }

        public int ClassId { get; set; }

        public int RefClassId { get; set; }

        private GenDataBase GenDataBase { get; set; }
        public GenDataDefClass DefClass { get; private set; }

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

        /// <summary>
        /// The number of items in the list.
        /// </summary>
        public int Count
        {
            get { return GenObjectListBase.Count; }
        }

        /// <summary>
        /// The data reference of the list.
        /// </summary>
        public string Reference { get; set; }

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
            if (!string.IsNullOrEmpty(Reference)) ReferenceData.First(RefClassId);
        }

        /// <summary>
        /// Make the next item the current selection.
        /// </summary>
        public void Next()
        {
            Index++;
            if (Eol)
                Reset();
            if (!string.IsNullOrEmpty(Reference)) ReferenceData.Next(RefClassId);
        }

        /// <summary>
        /// Make the last item, if any, the current selection.
        /// </summary>
        public void Last()
        {
            Index = GenObjectListBase.Count - 1;
            if (!string.IsNullOrEmpty(Reference)) ReferenceData.Last(RefClassId);
        }

        /// <summary>
        /// Make the prior item, if any, the current selection.
        /// </summary>
        public void Prior()
        {
            Index--;
            if (Eol)
                Reset();
            if (!string.IsNullOrEmpty(Reference)) ReferenceData.Prior(RefClassId);
        }

        public GenData ReferenceData { get; set; }

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
            GenDataBase.Changed = true;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.Classes[ClassId].Name, "");
        }

        private void MoveUp(int itemIndex)
        {
            if (itemIndex <= 0 || itemIndex >= GenObjectListBase.Count) return;
            var genObject = GenObjectListBase[itemIndex];
            GenObjectListBase[itemIndex] = GenObjectListBase[itemIndex - 1];
            GenObjectListBase[itemIndex - 1] = genObject;
            Index = itemIndex - 1;
            GenDataBase.Changed = true;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.Classes[ClassId].Name, "");
        }

        private void MoveDown(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= GenObjectListBase.Count - 1) return;
            var genObject = GenObjectListBase[itemIndex];
            GenObjectListBase[itemIndex] = GenObjectListBase[itemIndex + 1];
            GenObjectListBase[itemIndex + 1] = genObject;
            Index = itemIndex + 1;
            GenDataBase.Changed = true;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.Classes[ClassId].Name, "");
        }

        private void MoveToBottom(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= GenObjectListBase.Count - 1) return;
            var genObject = GenObjectListBase[itemIndex];
            GenObjectListBase.RemoveAt(itemIndex);
            GenObjectListBase.Add(genObject);
            Index = GenObjectListBase.Count - 1;
            GenDataBase.Changed = true;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.Classes[ClassId].Name, "");
        }

        /// <summary>
        /// Create a new <see cref="GenObject"/> and append it to the list.
        /// </summary>
        /// <returns>The new object.</returns>
        public GenObject CreateObject()
        {
            var genObjectListBase = GenObjectListBase as GenObjectListBase;
            return genObjectListBase != null ? genObjectListBase.CreateObject() : null;
        }

        public int IndexOf(GenObject genObject)
        {
            return GenObjectListBase.IndexOf(genObject);
        }

        public override string ToString()
        {
            return GenDataBase.GenDataDef.Classes[ClassId] + (Eol || GenObject.Attributes.Count == 0 ? "" : ":" + GenObject.Attributes[0]) ;
        }

        /// <summary>
        /// Delete the currently selected object if any.
        /// </summary>
        public void Delete()
        {
            if (Eol) return;
            GenObjectListBase.RemoveAt(Index);
            GenDataBase.Changed = true;
        }
    }
}
