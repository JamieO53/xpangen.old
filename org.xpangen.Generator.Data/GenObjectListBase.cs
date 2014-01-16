using System.Collections;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenObjectListBase : List<GenObject>, IGenObjectListBase
    {
        /// <summary>
        /// Create a new <see cref="GenObjectList"/> list.
        /// </summary>
        /// <param name="data">The generator data containing the list.</param>
        /// <param name="parent">The generator object owning the list.</param>
        /// <param name="classId">The ID of objects in the list.</param>
        /// <param name="subClassDef">The definition of the subclass</param>
        public GenObjectListBase(GenDataBase data, GenObject parent, int classId, GenDataDefSubClass subClassDef)
        {
            SubClassDef = subClassDef;
            GenDataBase = data;
            Parent = parent;
            ClassId = classId;
            IsReset = false;
        }

        /// <summary>
        /// The generator data containing the list.
        /// </summary>
        public GenDataBase GenDataBase { get; private set; }

        /// <summary>
        /// The generator object owning the list.
        /// </summary>
        public GenObject Parent { get; private set; }

        /// <summary>
        /// The ID of objects in the list.
        /// </summary>
        public int ClassId { get; private set; }

        public GenDataDefSubClass SubClassDef { get; private set; }
        public bool IsReset { get; set; }

        public void Reset()
        {
            // Do nothing
        }

        /// <summary>
        /// Create a new <see cref="Data.GenObject"/> and add it to the list.
        /// </summary>
        /// <returns>The new item.</returns>
        public GenObject CreateObject()
        {
            var o = new GenObject(Parent, this, ClassId);
            Add(o);
            return o;

        }
    }
}