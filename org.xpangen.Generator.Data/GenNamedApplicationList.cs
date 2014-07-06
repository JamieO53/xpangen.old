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
            }
            parent.Lists.Add(className, this);
        }
        
        public GenNamedApplicationList(GenApplicationBase parent)
        {
            Parent = parent;
            ParentObject = parent.GenObject as GenObject;
            if (ParentObject != null)
            {
                var className = typeof(T).Name;
                ClassId = parent.Classes.IndexOf(className);
                ClassIdx = parent.SubClasses.IndexOf(className);
                if (ClassIdx != -1)
                {
                    ParentSubClass = ParentObject.SubClass[ClassIdx];
                    var list = new GenObjectList(ParentObject.SubClass[ClassIdx], ParentObject.GenDataBase,
                                                 parent.GenData.Context[parent.ClassId],
                                                 null);//parent.GenDataDef.Classes[parent.ClassId].SubClasses[ClassIdx]);
                    list.First();
                    while (!list.Eol)
                    {
                        Add(new T { GenData = parent.GenData, GenObject = list.GenObject, Parent = parent });
                        list.Next();
                    }
                    parent.Lists.Add(className, this);
                }
            }
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
        /// Find the index of the object.
        /// </summary>
        /// <param name="name">The name of the sought object.</param>
        /// <returns>The index of the named object, otherwise -1.</returns>
        public int IndexOf(string name)
        {
            var idx = -1;
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    idx = i;
                }
            }
            return idx;
        }
    }
}
