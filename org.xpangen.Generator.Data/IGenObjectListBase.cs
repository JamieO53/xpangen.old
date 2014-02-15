using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public interface IGenObjectListBase : IList<GenObject>
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
        
        GenDataDefSubClass Definition { get; }
        bool IsReset { get; set; }

        /// <summary>
        /// Reset the underlying data to force it to be reinitialized.
        /// </summary>
        void Reset();
    }
}