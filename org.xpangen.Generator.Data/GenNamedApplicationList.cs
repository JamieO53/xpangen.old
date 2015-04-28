// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace org.xpangen.Generator.Data
{
    public interface IGenNamedApplicationList : IGenApplicationList
    {
        /// <summary>
        /// Indicates that the specified item's name has changed
        /// </summary>
        /// <param name="item">The changed item</param>
        void NameChanged(GenNamedApplicationBase item);

        /// <summary>
        /// Does the list contain an object with this name?
        /// </summary>
        /// <param name="name">The name of the sought object.</param>
        /// <returns>Is the named object in the list?</returns>
        bool Contains(string name);

        /// <summary>
        /// Find the index of the object.
        /// </summary>
        /// <param name="name">The name of the sought object.</param>
        /// <returns>The index of the named object, otherwise -1.</returns>
        int IndexOf(string name);
    }
    
    public interface IGenNamedApplicationList<T> : IGenNamedApplicationList, IGenApplicationList<T> where T : GenNamedApplicationBase, new()
    {
        /// <summary>
        /// Find the named object.
        /// </summary>
        /// <param name="name">The name of the sought object.</param>
        /// <returns>The named object if it is in the list, otherwise the default object.</returns>
        T Find(string name);

        /// <summary>
        /// Adds an object to the end of the <see cref="T:IGenNamedApplicationList`1"/>.
        /// </summary>
        /// <param name="item">The object to be added to the end of the <see cref="T:IGenNamedApplicationList`1"/>. The value can be null for reference types.</param>
        new void Add(T item);

        /// <summary>
        /// Removes the specified item from the  <see cref="T:IGenNamedApplicationList`1"/>.
        /// </summary>
        /// <param name="item"></param>
        new void Remove(T item);
    }

    /// <summary>
    /// A generic application list of named generator data objects.
    /// </summary>
    /// <typeparam name="T">The type of the objects contained in the list.</typeparam>
    public class GenNamedApplicationList<T> : GenApplicationList<T>, IGenNamedApplicationList<T> where T : GenNamedApplicationBase, new()
    {
        private Dictionary<string, int> _names = new Dictionary<string, int>();

        /// <summary>
        /// Create a new Generic named application list.
        /// </summary>
        /// <param name="parent">The list's parent object.</param>
        /// <param name="classId">The ClassID of the list's objects.</param>
        /// <param name="classIdx">The index of the underlying data in the parent object's subclasses list.</param>
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

        /// <summary>
        /// Adds an object to the end of the <see cref="T:IGenNamedApplicationList`1"/>.
        /// </summary>
        /// <param name="item">The object to be added to the end of the <see cref="T:IGenNamedApplicationList`1"/>. The value can be null for reference types.</param>
        public new void Add(T item)
        {
            if (Contains(item.Name)) return;
            item.Parent = Parent;
            item.Classes = Parent.Classes;
            item.List = this;
            base.Add(item);
            _names.Add(item.Name, Count - 1);
        }

        public new void Remove(T item)
        {
            base.Remove(item);
            PopulateNameDictionary();
        }

        private void PopulateNameDictionary()
        {
            _names = new Dictionary<string, int>();
            for (var i = 0; i < Count; i++)
                _names.Add(this[i].Name, i);
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
        public int IndexOf(string name)
        {
            return _names.ContainsKey(name) ? _names[name] : -1;
        }

        public void NameChanged(GenNamedApplicationBase item)
        {
            Contract.Requires(item.GetType() == typeof (T),
                "Invalid parameter type: " + item.GetType().Name + ", " + typeof (T).Name + " expected");
            Contract.Requires(IndexOf((T)item) != -1, "Item not found in list: " + item.Name);
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
            if (moved)
                PopulateNameDictionary();
            return moved;
        }
    }
}
