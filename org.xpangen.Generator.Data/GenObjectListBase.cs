using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenObjectListBase : List<GenObject>
    {
        /// <summary>
        /// Create a new <see cref="GenObjectList"/> list.
        /// </summary>
        /// <param name="data">The generator data containing the list.</param>
        /// <param name="parent">The generator object owning the list.</param>
        /// <param name="classId">The ID of objects in the list.</param>
        public GenObjectListBase(GenDataBase data, GenObject parent, int classId)
        {
            GenDataBase = data;
            Parent = parent;
            ClassId = classId;
        }

        /// <summary>
        /// The generator data containing the list.
        /// </summary>
        protected internal GenDataBase GenDataBase { get; set; }

        /// <summary>
        /// The generator object owning the list.
        /// </summary>
        public GenObject Parent { get; private set; }

        /// <summary>
        /// The ID of objects in the list.
        /// </summary>
        public int ClassId { get; private set; }

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