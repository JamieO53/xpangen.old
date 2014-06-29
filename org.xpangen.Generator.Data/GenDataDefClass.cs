﻿namespace org.xpangen.Generator.Data
{
    /// <summary>
    /// The definition of a generator class
    /// </summary>
    public class GenDataDefClass : GenBase
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
        public NameList InstanceProperties
        {
            get
            {
                if (_instanceProperties != null) return _instanceProperties;
                else
                {
                    CreateInstanceProperties();
                    return _instanceProperties;
                }
            }
        }

        /// <summary>
        /// The class' properties
        /// </summary>
        public NameList Properties
        {
            get
            {
                if (_properties != null) return _properties;
                else
                {
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
        }

        private void CopyInstanceProperties(NameList nameList)
        {
            for (var i = 0; i < InstanceProperties.Count; i++)
                if (!nameList.Contains(InstanceProperties[i]))
                    nameList.Add(InstanceProperties[i]);
        }

        private void CopyInheritedProperties(NameList nameList)
        {
            if (Parent.IsInherited)
                Parent.CopyInheritedProperties(nameList);
            Parent.CopyInstanceProperties(nameList);
        }

        public int ClassId { get; set; }

        public bool IsReference { get; set; }

        public int RefClassId { get; set; }

        public GenDataDef RefDef { get; set; }

        public string ReferenceDefinition { get; set; }

        public string Reference { get; set; }

        public readonly GenDataDefSubClassList SubClasses;

        public readonly GenDataDefClassList Inheritors;

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
