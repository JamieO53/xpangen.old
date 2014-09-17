// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Diagnostics.Contracts;

namespace org.xpangen.Generator.Data
{
    public class GenObjectList: GenBase
    {
        private ISubClassBase _subClassBase;

        public GenObjectList ParentList { get; private set; }

        /// <summary>
        ///     Create a new <see cref="GenObjectList" /> list.
        /// </summary>
        /// <param name="subClassBase"> The underlying generator object list.</param>
        /// <param name="genDataBase">The underlying data container.</param>
        /// <param name="parentList">The parent list.</param>
        /// <param name="defSubClass">The subclass definition.</param>
        public GenObjectList(ISubClassBase subClassBase, GenDataBase genDataBase, GenObjectList parentList,
                             GenDataDefSubClass defSubClass)
        {
            DefSubClass = defSubClass;
            ParentList = parentList;
            ClassId = -1;
            GenDataBase = genDataBase;
            SubClassBase = subClassBase;
            if (SubClassBase.Definition != null) DefClass = SubClassBase.Definition.SubClass;
            ClassId = 0;
        }

        public ISubClassBase SubClassBase
        {
            get { return _subClassBase; }
            set
            {
                if (value != null && ClassId != -1)
                {
                    if (ParentList.Eol) ParentList.First();
                    if (String.IsNullOrEmpty(Reference))
                    {
                        Contract.Assert(value.ClassId == ClassId ||
                               DefClass.IsInherited && !DefClass.Parent.IsInherited &&
                               value.ClassId == DefClass.Parent.ClassId ||
                               DefClass.IsInherited && DefClass.Parent.IsInherited &&
                               value.ClassId == DefClass.Parent.Parent.ClassId,
                            "Object list assignment error: " +
                            string.Format("The expected class was {0}({1}) but is {2}({3})",
                                GenDataBase.GenDataDef.GetClassName(ClassId), ClassId,
                                GenDataBase.GenDataDef.GetClassName(value.ClassId), value.ClassId));
                        Contract.Assert(
                            ParentList.GenObject == null || (value.Parent == ParentList.GenObject ||
                            DefClass.IsInherited && value.Parent == ParentList.GenObject.Parent),
                            "Object list assignment error:" +
                            string.Format("The the incorrect subclass is being used for the current parent"));
                    }
                    else if (ReferenceData != null)
                    {
                        var refClassId = GetBaseReferenceClassId(RefClassId, ReferenceData);
                        Contract.Assert(value.ClassId == refClassId,
                            "Object list assignment error:" +
                            string.Format("The expected class was {0}({1}) but is {2}({3})",
                                ReferenceData.GenDataBase.GenDataDef.GetClassName(RefClassId),
                                ReferenceData.Context[refClassId].RefClassId,
                                ReferenceData.GenDataBase.GenDataDef.GetClassName(value.ClassId), value.ClassId));
                        Contract.Assert(value.Parent.ClassId == 0 || value.Parent == ParentList.GenObject,
                            "Object list assignment error:" +
                            "The the incorrect subclass is being used for the current parent");
                        var refGenDataBase = GetBaseReferenceData(RefClassId, ReferenceData);
                        Contract.Assert(value.GenDataBase == refGenDataBase,
                            "Object list assignment error: " +
                            string.Format(
                                "The assigned object list does not belong to the context generator data: {0} vs {1}",
                                value.GenDataBase, ReferenceData.GenDataBase));
                    }
                }
                _subClassBase = value;
            }
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

        public GenObject this[int index]
        {
            get { return SubClassBase[index]; }
        }

        /// <summary>
        ///     The currently selected item.
        /// </summary>
        public GenObject GenObject
        {
            get { return SubClassBase == null || SubClassBase.Count == 0 || Index < 0 || Index >= SubClassBase.Count ? null : SubClassBase[Index]; }
        }

        /// <summary>
        ///     Is the current object at the end of the list?
        /// </summary>
        public bool Eol
        {
            get
            {
                return SubClassBase == null || SubClassBase.Count == 0 || Index < 0 || Index >= SubClassBase.Count ||
                       DefClass != null && DefClass.IsInherited && GenObject.ClassId != ObjectClassId;
            }
        }

        private int ObjectClassId
        {
            get { return (DefClass.IsReference ? RefClassId : ClassId); }
        }

        /// <summary>
        ///     The index of the currently selected item.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     The number of items in the list.
        /// </summary>
        public int Count
        {
            get { return SubClassBase.Count; }
        }

        /// <summary>
        ///     The data reference of the list.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        ///     Is the selected item in the first place?
        /// </summary>
        /// <returns></returns>
        public bool IsFirst()
        {
            return Index == 0 && SubClassBase.Count > 0;
        }

        /// <summary>
        ///     Is the selected item in the last place?
        /// </summary>
        /// <returns></returns>
        public bool IsLast()
        {
            return Index == SubClassBase.Count - 1 && SubClassBase.Count > 0;
        }

        /// <summary>
        ///     Make the first item, if any, the current selection.
        /// </summary>
        public void First()
        {
            if (SubClassBase.Count == 0)
                Reset();
            else
            {
                Index = 0;
                if (DefClass != null && DefClass.IsInherited) SetInheritedIndex();
            }
            if (!string.IsNullOrEmpty(Reference)) ReferenceData.First(RefClassId);
        }

        /// <summary>
        ///     Make the next item the current selection.
        /// </summary>
        public void Next()
        {
            Index++;
            if (DefClass != null && DefClass.IsInherited) SetInheritedIndex();
            if (Eol)
                Reset();
            if (!string.IsNullOrEmpty(Reference)) ReferenceData.Next(RefClassId);
        }

        /// <summary>
        ///     Make the last item, if any, the current selection.
        /// </summary>
        public void Last()
        {
            Index = SubClassBase.Count - 1;
            if (DefClass != null && DefClass.IsInherited) SetPriorInheritedIndex();
            if (!string.IsNullOrEmpty(Reference)) ReferenceData.Last(RefClassId);
        }

        /// <summary>
        ///     Make the prior item, if any, the current selection.
        /// </summary>
        public void Prior()
        {
            Index--;
            if (DefClass != null && DefClass.IsInherited) SetPriorInheritedIndex();
            if (Eol)
                Reset();
            if (!string.IsNullOrEmpty(Reference)) ReferenceData.Prior(RefClassId);
        }

        public GenData ReferenceData { get; set; }

        /// <summary>
        ///     Reset the current selection.
        /// </summary>
        public void Reset()
        {
            Index = -1;
            SubClassBase.Reset();
        }

        private void SetInheritedIndex()
        {
            while (Index < Count && GenObject != null && GenObject.ClassId != ObjectClassId) Index++;
            if (Index >= Count) Reset();
        }

        private void SetPriorInheritedIndex()
        {
            while (Index >= 0 && GenObject != null && GenObject.ClassId != ObjectClassId) Index--;
            if (Index < 0) Reset();
        }

        /// <summary>
        ///     Move the specified item up, down, to the top or to the bottom of the list.
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
            if (itemIndex <= 0 || itemIndex >= SubClassBase.Count) return;
            var genObject = SubClassBase[itemIndex];
            SubClassBase.RemoveAt(itemIndex);
            SubClassBase.Insert(0, genObject);
            Index = 0;
            GenDataBase.Changed = true;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.GetClassName(ClassId), "");
        }

        private void MoveUp(int itemIndex)
        {
            if (itemIndex <= 0 || itemIndex >= SubClassBase.Count) return;
            var genObject = SubClassBase[itemIndex];
            SubClassBase[itemIndex] = SubClassBase[itemIndex - 1];
            SubClassBase[itemIndex - 1] = genObject;
            Index = itemIndex - 1;
            GenDataBase.Changed = true;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.GetClassName(ClassId), "");
        }

        private void MoveDown(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= SubClassBase.Count - 1) return;
            var genObject = SubClassBase[itemIndex];
            SubClassBase[itemIndex] = SubClassBase[itemIndex + 1];
            SubClassBase[itemIndex + 1] = genObject;
            Index = itemIndex + 1;
            GenDataBase.Changed = true;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.GetClassName(ClassId), "");
        }

        private void MoveToBottom(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= SubClassBase.Count - 1) return;
            var genObject = SubClassBase[itemIndex];
            SubClassBase.RemoveAt(itemIndex);
            SubClassBase.Add(genObject);
            Index = SubClassBase.Count - 1;
            GenDataBase.Changed = true;
            GenDataBase.RaiseDataChanged(GenDataBase.GenDataDef.GetClassName(ClassId), "");
        }

        public int IndexOf(GenObject genObject)
        {
            return SubClassBase.IndexOf(genObject);
        }

        public override string ToString()
        {
            return GenDataBase.GenDataDef.GetClassDef(ClassId) +
                   (Eol || GenObject.Attributes.Count == 0 ? "" : ":" + GenObject.Attributes[0]);
        }

        /// <summary>
        ///     Delete the currently selected object if any.
        /// </summary>
        public void Delete()
        {
            if (Eol) return;
            SubClassBase.RemoveAt(Index);
            GenDataBase.Changed = true;
        }
    }
}