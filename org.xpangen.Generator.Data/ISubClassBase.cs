using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public interface ISubClassBase : IList<GenObject>
    {
        /// <summary>
        /// The generator data containing the list.
        /// </summary>
        GenDataBase GenDataBase { get; }

        /// <summary>
        /// The generator object owning the list.
        /// </summary>
        GenObject Parent { get; }

        /// <summary>
        /// The ID of objects in the list.
        /// </summary>
        int ClassId { get; }

        /// <summary>
        /// The data reference of the list
        /// </summary>
        string Reference { get; set; }
        
        /// <summary>
        /// The definition of the subclass
        /// </summary>
        GenDataDefSubClass Definition { get; }

        /// <summary>
        /// The relationship between the subclass and the class
        /// </summary>
        string Relationship { get; set; }

        /// <summary>
        /// Reset the underlying data to force it to be reinitialized.
        /// </summary>
        void Reset();
    }
}