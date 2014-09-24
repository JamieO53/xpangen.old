// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Definition
{
    public static class DefinitionCreator
    {

        public static Definition Create()
        {
            var d = CreateEmpty();

            var b = d.GenDataBase;
            var root = b.Root;
            
            var @class = CreateDefinitionClass(root, "Class", "Class Definition", "", b);
            CreateDefinitionSubClass(@class, "SubClass", "", "");
            CreateDefinitionSubClass(@class, "Property", "", "");
            CreateDefinitionProperty(@class, "Name", "Class name: must be well formed", "Identifier");
            CreateDefinitionProperty(@class, "Title", "Class description: used as a hint when editing", "String");
            CreateDefinitionProperty(@class, "Inheritance", "What kind of inheritance do extended subclasses have",
                                     "String", "", "Standard", "", "Inheritance");
            
            var subClass = CreateDefinitionClass(root, "SubClass", "Class to SubClass link", "", b);
            CreateDefinitionProperty(subClass, "Name",
                                     "Subclass name: refers to a class and is used for hierarchical browsing",
                                     "Identifier");
            CreateDefinitionProperty(subClass, "Reference", "Location of the subclass", "String");
            CreateDefinitionProperty(subClass, "Relationship", "SubClass relationship between parent and child",
                                     "String", "", "Standard", "", "Relationship");
            
            var property = CreateDefinitionClass(root, "Property", "Property definition", "", b);
            CreateDefinitionProperty(property, "Name", "Property name: must be well formed", "Identifier");
            CreateDefinitionProperty(property, "Title", "Property description: used as a hint when editing", "String");
            CreateDefinitionProperty(property, "DataType",
                                     "Data type of property (used for editing) - String, integer or boolean", "String",
                                     "", "Standard", "", "DataType");
            CreateDefinitionProperty(property, "Default",
                                     "Default value of the property when a new object is created (used for editing)",
                                     "String");
            CreateDefinitionProperty(property, "LookupType",
                                     "A standard lookup, a lookup to this data or the root of referenced data", "String",
                                     "", "Standard", "", "LookupTypes");
            CreateDefinitionProperty(property, "LookupDependence", "The lookup on which this one depends", "String", "",
                                     "Standard", "LookupType", "LookupDependencesX");
            CreateDefinitionProperty(property, "LookupTable", "The lookup table used for the property's values",
                                     "String", "", "Standard", "LookupType", "LookupTablesX");

            return new Definition(d.GenDataBase);
        }

        public static GenData CreateEmpty()
        {
            var d = new GenData((GenDataDef) null) {DataName = "Definition"};
            var b0 = d.GenDataBase;
            var root0 = b0.Root;
            root0.SubClass.Add(new GenSubClass(b0, root0, 1, null));
            return d;
        }

        public static GenObject CreateDefinitionClass(GenObject root, string name, string title, string inheritance,
                                                       GenDataBase d)
        {
            var @class = new GenObject(root, root.SubClass[0], 1);
            SetAttribute(@class, 0, name);
            SetAttribute(@class, 1, title);
            SetAttribute(@class, 2, inheritance);
            @class.SubClass.Add(new GenSubClass(d, @class, 2, null));
            @class.SubClass.Add(new GenSubClass(d, @class, 3, null));
            root.SubClass[0].Add(@class);
            return @class;
        }

        public static GenObject CreateDefinitionSubClass(GenObject @class, string name, string reference,
                                                     string relationship)
        {
            var subClass = new GenObject(@class, @class.SubClass[0], 2);

            SetAttribute(subClass, 0, name);
            SetAttribute(subClass, 1, reference);
            SetAttribute(subClass, 2, relationship);
            @class.SubClass[0].Add(subClass);
            return subClass;
        }

        private static void CreateDefinitionProperty(GenObject @class, string name, string title, string dataType,
                                                     string @default = "", string lookupType = "",
                                                     string lookupDependence = "", string lookupTable = "")
        {
            var property = new GenObject(@class, @class.SubClass[1], 3);
            SetAttribute(property, 0, name);
            SetAttribute(property, 1, title);
            SetAttribute(property, 2, dataType);
            SetAttribute(property, 3, @default);
            SetAttribute(property, 4, lookupType);
            SetAttribute(property, 5, lookupDependence);
            SetAttribute(property, 6, lookupTable);
            @class.SubClass[1].Add(property);
        }
        
        private static void SetAttribute(GenObject genObject, int index, string value)
        {
            if (genObject.Attributes.Count == index)
                genObject.Attributes.Add(value);
            else genObject.Attributes[index] = value;
        }
    }
}
