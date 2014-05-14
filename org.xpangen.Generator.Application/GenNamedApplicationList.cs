using System;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Application
{
    public class GenNamedApplicationList<T> : GenApplicationList<T> where T: GenNamedApplicationBase, new()
    {
        public GenNamedApplicationList(GenApplicationBase parent)
        {
            var className = typeof(T).Name;
            var classId = parent.GenDataDef.Classes.IndexOf(className);
            var classIdx = parent.GenDataDef.IndexOfSubClass(parent.ClassId, classId);
            if (classIdx != -1)
            {
                var list = new GenObjectList(parent.GenObject.SubClass[classIdx], parent.GenObject.GenDataBase,
                                             parent.GenData.Context[parent.ClassId], parent.GenDataDef.Classes[parent.ClassId].SubClasses[classIdx]);
                list.First();
                while (!list.Eol)
                {
                    Add(new T { GenData = parent.GenData, GenObject = list.GenObject });
                    list.Next();
                }
                parent.Lists.Add(className, this);
            }
        }

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
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return this[i];
            }
            return default(T);
        }
    }
}
