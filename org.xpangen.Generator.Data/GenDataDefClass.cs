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
        public GenDataDefClass Parent { get; set; }
        
        /// <summary>
        /// Format the object as a string.
        /// </summary>
        /// <returns>The formatted object.</returns>
        public override string ToString()
        {
            return (ReferenceDefinition == "" ? "" : ReferenceDefinition + ".") + Name;
        }

        /// <summary>
        /// The class' properties
        /// </summary>
        public NameList Properties { get; private set; }

        public int ClassId { get; set; }

        public bool IsReference { get; set; }

        public int RefClassId { get; set; }

        public GenDataDef RefDef { get; set; }

        public string ReferenceDefinition { get; set; }

        public string Reference { get; set; }

        public GenDataDefSubClassList SubClasses;

        private IndexList Pseudos { get; set; }

        public GenDataDefClass()
        {
            Properties = new NameList();
            SubClasses = new GenDataDefSubClassList();
            IsReference = false;
            RefClassId = 0;
            Reference = "";
            ReferenceDefinition = "";
        }

        public bool IsPseudo(int propertyId)
        {
            return Pseudos != null && Pseudos.Contains(propertyId);
        }

        public void SetPseudo(int propertyId)
        {
            if (Pseudos == null) Pseudos = new IndexList();
            if (!Pseudos.Contains(propertyId)) Pseudos.Add(propertyId);
        }
    }
}
