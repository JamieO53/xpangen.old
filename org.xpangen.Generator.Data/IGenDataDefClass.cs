// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data.Definition;

namespace org.xpangen.Generator.Data
{
    public interface IGenDataDefClass
    {
        /// <summary>
        /// The name of the class
        /// </summary>
        string Name { get; set; }

        Class Definition { get; }

        /// <summary>
        /// The parent of the class
        /// </summary>
        IGenDataDefClass Parent { get; set; }

        /// <summary>
        /// The class' properties 
        /// </summary>
        NameList InstanceProperties { get; }

        /// <summary>
        /// The class' properties
        /// </summary>
        NameList Properties { get; }

        int ClassId { get; set; }
        bool IsReference { get; set; }
        int RefClassId { get; set; }
        IGenDataDef RefDef { get; set; }
        string ReferenceDefinition { get; set; }
        string Reference { get; set; }
        GenDataDefSubClassList SubClasses { get; }
        GenDataDefClassList Inheritors { get; }
        bool IsInherited { get; set; }
        bool IsAbstract { get; }

        /// <summary>
        /// Format the object as a string.
        /// </summary>
        /// <returns>The formatted object.</returns>
        string ToString();

        int AddInstanceProperty(string name);
        bool IsPseudo(int propertyId);
        void SetPseudo(int propertyId);
        void CreateInstanceProperties();
    }
}