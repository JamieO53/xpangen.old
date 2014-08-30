// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace org.xpangen.Generator.Data
{
    public class GenNamedApplicationList<T> : GenApplicationList<T> where T: GenNamedApplicationBase, new()
    {
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
                ParentSubClass = ParentObject.SubClass[ClassIdx];
                PopulateList(parent);
            }
            parent.Lists.Add(className, this);
        }
        
        public GenNamedApplicationList(GenApplicationBase parent)
        {
            var className = typeof(T).Name;
            Parent = parent;
            ParentObject = parent.GenObject as GenObject;
            if (ParentObject != null)
            {
                ClassId = parent.Classes.IndexOf(className);
                ClassIdx = parent.SubClasses.IndexOf(className);
                if (ClassIdx != -1)
                {
                    ParentSubClass = ParentObject.SubClass[ClassIdx];
                    PopulateList(parent);
                }
            }
            parent.Lists.Add(className, this);
        }

        public new void Add(T item)
        {
            item.Parent = Parent;
            item.Classes = Parent.Classes;
            base.Add(item);
        }

        private void PopulateList(GenApplicationBase parent)
        {
            var list = ParentObject.SubClass[ClassIdx];
            foreach (var item in list)
                Add(new T {GenData = parent.GenData, GenObject = item, Parent = parent});
            //var list = new GenObjectList(ParentObject.SubClass[ClassIdx], ParentObject.GenDataBase,
            //                             parent.GenData.Context.Count > parent.ClassId
            //                                 ? parent.GenData.Context[parent.ClassId]
            //                                 : null,
            //                             null);
            //list.First();
            //while (!list.Eol)
            //{
            //    Add(new T {GenData = parent.GenData, GenObject = list.GenObject, Parent = parent});
            //    list.Next();
            //}
        }

        public int ClassIdx { get; set; }

        public int ClassId { get; set; }

        public GenObject ParentObject { get; set; }

        protected GenApplicationBase Parent { get; set; }

        public ISubClassBase ParentSubClass { get; set; }

        public GenNamedApplicationList()
        {
        }

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
            for (var i = 0; i < Count; i++)
                if (this[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return i;
            return -1;
        }
    }
}
