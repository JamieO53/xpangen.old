// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace org.xpangen.Generator.Data
{
    public interface IGenNamedApplicationList<T> where T : GenNamedApplicationBase, new()
    {
        /// <summary>
        /// Find the named object.
        /// </summary>
        /// <param name="name">The name of the sought object.</param>
        /// <returns>The named object if it is in the list, otherwise the default object.</returns>
        T Find(string name);

        /// <summary>
        /// Does the list contain an object with this name?
        /// </summary>
        /// <param name="name">The name of the sought object.</param>
        /// <returns>Is the named object in the list?</returns>
        bool Contains(string name);

        void NameChanged(GenNamedApplicationBase item);
        bool Move(ListMove move, int itemIndex);
        void Add(T item);
    }

    public class GenNamedApplicationList<T> : GenApplicationList<T>, IGenNamedApplicationList<T> where T : GenNamedApplicationBase, new()
    {
        private Dictionary<string, int> _names;
        public GenNamedApplicationList(GenApplicationBase parent, int classId, int classIdx)
        {
            var className = typeof(T).Name;
            Parent = parent;
            ClassId = classId;
            ClassIdx = classIdx;
            ParentObject = parent.GenObject as GenObject;
            if (ParentObject != null)
            {
                if (ClassIdx >= ParentObject.SubClass.Count)
                    for (var i = ParentObject.SubClass.Count; i <= ClassIdx; i++)
                        ParentObject.SubClass.Add(new GenSubClass(ParentObject.GenDataBase, ParentObject, ClassId, null));
                PopulateList(parent);
            }
            parent.Lists.Add(className, this);
        }

        public new void Add(T item)
        {
            if (Contains(item.Name)) return;
            item.Parent = Parent;
            item.Classes = Parent.Classes;
            base.Add(item);
            if (_names == null)
                PopulateNameDictionary();
            else _names.Add(item.Name, Count - 1);
        }

        private void PopulateNameDictionary()
        {
            if (Count > 5)
            {
                _names = new Dictionary<string, int>();
                for (var i = 0; i < Count; i++)
                    _names.Add(this[i].Name, i);
            }
        }

        private void PopulateList(GenApplicationBase parent)
        {
            var list = ParentObject.SubClass[ClassIdx];
            foreach (var item in list)
                Add(new T {GenDataBase = parent.GenDataBase, GenObject = item, Parent = parent});
        }

        private int ClassIdx { get; set; }

        private int ClassId { get; set; }

        private GenObject ParentObject { get; set; }

        private GenApplicationBase Parent { get; set; }

        /// <summary>
        /// Find the named object.
        /// </summary>
        /// <param name="name">The name of the sought object.</param>
        /// <returns>The named object if it is in the list, otherwise the default object.</returns>
        public T Find(string name)
        {
            var idx = IndexOf(name);
            return idx != -1 ? this[idx] : default(T);
        }

        /// <summary>
        /// Does the list contain an object with this name?
        /// </summary>
        /// <param name="name">The name of the sought object.</param>
        /// <returns>Is the named object in the list?</returns>
        public bool Contains(string name)
        {
            return IndexOf(name) != -1;
        }
        
        /// <summary>
        /// Find the index of the object.
        /// </summary>
        /// <param name="name">The name of the sought object.</param>
        /// <returns>The index of the named object, otherwise -1.</returns>
        private int IndexOf(string name)
        {
            if (_names != null) return _names.ContainsKey(name) ? _names[name] : -1;
            for (var i = 0; i < Count; i++)
                if (this[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return i;
            return -1;
        }

        public void NameChanged(GenNamedApplicationBase item)
        {
            Contract.Requires(item.GetType() == typeof (T),
                "Invalid parameter type: " + item.GetType().Name + ", " + typeof (T).Name + " expected");
            Contract.Requires(IndexOf((T)item) != -1, "Item not found in list: " + item.Name);
            if (_names == null) return;
            var index = IndexOf((T) item);
            foreach (var namePair in _names)
            {
                if (namePair.Value != index) continue;
                _names.Remove(namePair.Key);
                _names.Add(item.Name, index);
                break;
            }
        }

        public override bool Move(ListMove move, int itemIndex)
        {
            var moved = base.Move(move, itemIndex);
            if (moved && _names != null)
                PopulateNameDictionary();
            return moved;
        }
    }
}
