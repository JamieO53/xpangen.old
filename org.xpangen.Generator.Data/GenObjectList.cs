// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace org.xpangen.Generator.Data
{
    public class GenObjectList
    {
        private IGenObjectListBase _genObjectListBase;

        public GenObjectList ParentList { get; private set; }

        /// <summary>
        /// Create a new <see cref="GenObjectList"/> list.
        /// </summary>
        /// <param name="genObjectListBase"> The underlying generator object list.</param>
        /// <param name="genDataBase">The underlying data container.</param>
        /// <param name="parentList">The parent list.</param>
        /// <param name="defSubClass">The subclass definition.</param>
        public GenObjectList(IGenObjectListBase genObjectListBase, GenDataBase genDataBase, GenObjectList parentList,
                             GenDataDefSubClass defSubClass)
        {
            DefSubClass = defSubClass;
            ParentList = parentList;
            ClassId = -1;
            GenDataBase = genDataBase;
            GenObjectListBase = genObjectListBase;
            DefClass = GenObjectListBase.Definition.SubClass;
            ClassId = 0;
        }

        public IGenObjectListBase GenObjectListBase
        {
            get { return _genObjectListBase; }
            set
            {
                if (value != null && ClassId != -1)
                {
                    if (ParentList.Eol) ParentList.First();
                    if (String.IsNullOrEmpty(Reference))
                    {
                        Assert(value.ClassId == ClassId, "Object list assignment error",
                                     string.Format("The expected class was {0}({1}) but is {2}({3})",
                                                   GenDataBase.GenDataDef.Classes[ClassId].Name,
                                                   ClassId, GenDataBase.GenDataDef.Classes[value.ClassId].Name,
                                                   value.ClassId));
                        Assert(value.Parent == ParentList.GenObject, "Object list assignment error",
                               string.Format("The the incorrect subclass is being used for the current parent"));
                    }
                    else if (ReferenceData != null)
                    {
                        var refClassId = GetBaseReferenceClassId(RefClassId, ReferenceData);
                        Assert(value.ClassId == refClassId, "Object list assignment error",
                                     string.Format("The expected class was {0}({1}) but is {2}({3})",
                                                   ReferenceData.GenDataBase.GenDataDef.Classes[RefClassId].Name,
                                                   ReferenceData.Context[refClassId].RefClassId,
                                                   ReferenceData.GenDataBase.GenDataDef.Classes[value.ClassId].Name,
                                                   value.ClassId));
                        Assert(value.Parent.ClassId == 0 || value.Parent == ParentList.GenObject,
                               "Object list assignment error",
                               "The the incorrect subclass is being used for the current parent");
                        var refGenDataBase = GetBaseReferenceData(RefClassId, ReferenceData);
                        Assert(value.GenDataBase == refGenDataBase, "Object list assignment error",
                               string.Format(
                                   "The assigned object list does not belong to the context generator data: {0} vs {1}",
                                   value.GenDataBase, ReferenceData.GenDataBase));
                    }
                }
                _genObjectListBase = value;
            }
        }

        private static void Assert(bool condition, string message, string details)
        {
            if (!condition)
                throw new GeneratorException(message + ": " + details, GenErrorType.Assertion);
        }

        private int GetBaseReferenceClassId(int refClassId, GenData referenceData)
        {
            if (ReferenceData.Context[refClassId].ReferenceData != null &&
                referenceData != ReferenceData.Context[refClassId].ReferenceData)
                return GetBaseReferenceClassId(ReferenceData.Context[refClassId].RefClassId,
                                               ReferenceData.Context[refClassId].ReferenceData);
            return referenceData.Context[refClassId].RefClassId;
        }

        private GenDataBase GetBaseReferenceData(int refClassId, GenData referenceData)
        {
            if (ReferenceData.Context[refClassId].ReferenceData != null &&
                referenceData != ReferenceData.Context[refClassId].ReferenceData)
                return GetBaseReferenceData(ReferenceData.Context[refClassId].RefClassId,
                                               ReferenceData.Context[refClassId].ReferenceData);
            return referenceData.GenDataBase;
        }

        public int ClassId { get; set; }

        public int RefClassId { get; set; }

        private GenDataBase GenDataBase { get; set; }
        public GenDataDefClass DefClass { get; private set; }
        public GenDataDefSubClass DefSubClass { get; private set; }

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
            if (!string.IsNullOrEmpty(Reference) && ReferenceData != null) ReferenceData.First(RefClassId);
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
