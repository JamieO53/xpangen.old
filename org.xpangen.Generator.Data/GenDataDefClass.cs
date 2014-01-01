using System;
using System.Collections.Generic;
using System.Text;

namespace org.xpangen.Generator.Data
{
    /// <summary>
    /// The definition of a generator class
    /// </summary>
    public class GenDataDefClass
    {
        /// <summary>
        /// The name of the class
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parent of the class
        /// </summary>
        public GenDataDefClass Parent
        {
            get; set;
        }
        
        /// <summary>
        /// The class' properties
        /// </summary>
        public NameList Properties { get; private set; }

        public int ClassId { get; set; }

        //
        public GenDataDefSubClassList SubClasses;

        public GenDataDefClass()
        {
            Properties = new NameList();
            SubClasses = new GenDataDefSubClassList();
        }
    }
}
