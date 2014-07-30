// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data.Definition;

namespace org.xpangen.Generator.Data
{
    /// <summary>
    /// The definition of a generator class
    /// </summary>
    public class GenDataDefClass : GenBase, IGenDataDefClass
    {
        /// <summary>
        /// The name of the class
        /// </summary>
        public string Name { get; set; }

        public Class Definition { get; internal set; }
        /// <summary>
        /// The parent of the class
        /// </summary>
        
        public IGenDataDefClass Parent { get; set; }
        
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
        public NameList InstanceProperties
        {
            get
            {
                if (_instanceProperties == null)
                    CreateInstanceProperties();
                return _instanceProperties;
            }
        }

        public int AddInstanceProperty(string name)
        {
            if (Definition != null && !Definition.PropertyList.Contains(name))
                Definition.AddProperty(name);
            return InstanceProperties.Add(name);
        }
        
        /// <summary>
        /// The class' properties
        /// </summary>
        public NameList Properties
        {
            get
            {
                if (_properties != null) return _properties;
                Assert(_instanceProperties != null, "The instance properties have not been initialized");
                NameList nameList;
                if (RefDef != null && RefClassId >= 0 && RefClassId < RefDef.Classes.Count)
                    nameList = RefDef.Classes[RefClassId].Properties;
                else
                {
                    nameList = new NameList();
                    if (IsInherited)
                        CopyInheritedProperties(nameList);
                    CopyInstanceProperties(nameList);
                }
                return _properties = nameList;
            }
        }

        private void CopyInstanceProperties(NameList nameList)
        {
            foreach (var instanceProperty in InstanceProperties)
                if (!nameList.Contains(instanceProperty))
                    nameList.Add(instanceProperty);
        }

        private void CopyInheritedProperties(NameList nameList)
        {
            var parent = (GenDataDefClass) Parent;
            if (parent.IsInherited)
                parent.CopyInheritedProperties(nameList);
            parent.CopyInstanceProperties(nameList);
        }

        public int ClassId { get; set; }

        public bool IsReference { get; set; }

        public int RefClassId { get; set; }

        public IGenDataDef RefDef { get; set; }

        public string ReferenceDefinition { get; set; }

        public string Reference { get; set; }

        public IGenDataDefSubClassList SubClasses { get; private set; }

        public IGenDataDefClassList Inheritors { get; private set; }

        private NameList _properties;

        private NameList _instanceProperties;

        private IndexList Pseudos { get; set; }

        public bool IsInherited { get; set; }

        public bool IsAbstract
        {
            get { return Inheritors.Count > 0; }
        }

        public GenDataDefClass()
        {
            SubClasses = new GenDataDefSubClassList();
            Inheritors = new GenDataDefClassList();
            IsInherited = false;
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

        public void CreateInstanceProperties()
        {
            _instanceProperties = RefDef != null && RefClassId >= 0 && RefClassId < RefDef.Classes.Count
                                      ? RefDef.Classes[RefClassId].InstanceProperties
                                      : new NameList();
        }
    }
}
