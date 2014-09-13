// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace org.xpangen.Generator.Data
{
    /// <summary>
    /// Crawl througn the GenData to create the GenDataDef defined by its data
    /// </summary>
    internal class GenDataToDef
    {
        private readonly int _nClass;
        private readonly int _xSubClass;
        private readonly int _xProperty;
        private readonly int _xClassName;
        private readonly int _xSubClassName;
        private readonly int _xSubClassRelationship;
        private readonly int _xSubClassReference;
        private readonly int _xPropertyName;

        private GenDataBase GenDataBase { get; set; }
        private GenDataDef GenDataDef { get; set; }

        public GenDataToDef(GenDataBase genData)
        {
            GenDataBase = genData;
            GenDataDef = GenDataBase.GenDataDef;
            _nClass = GenDataDef.GetClassId("Class");
            var nSubClass = GenDataDef.GetClassId("SubClass");
            var nProperty = GenDataDef.GetClassId("Property");
            _xClassName = GenDataDef.GetClassProperties(_nClass).IndexOf("Name");
            _xSubClassName = GenDataDef.GetClassProperties(nSubClass).IndexOf("Name");
            _xSubClassRelationship = GenDataDef.GetClassProperties(nSubClass).IndexOf("Relationship");
            _xSubClassReference = GenDataDef.GetClassProperties(nSubClass).IndexOf("Reference");
            _xPropertyName = GenDataDef.GetClassProperties(nProperty).IndexOf("Name");

            if (_nClass == -1 || nSubClass == -1 || nProperty == -1)
                return;

            _xSubClass = GenDataDef.IndexOfSubClass(_nClass, nSubClass);
            _xProperty = GenDataDef.IndexOfSubClass(_nClass, nProperty);
        }
    
        public GenDataDef AsDef()
        {
            var f = new GenDataDef {DefinitionName = GenDataBase.DataName};
            Navigate(GenDataBase.Root, f);

            return f;
        }

        private void Navigate(GenObject c, GenDataDef f)
        {
            if (c.ClassId != _nClass)
                foreach (var sc in c.SubClass)
                    foreach (var s in sc)
                        Navigate(s, f);
            else
            {
                var sc = c.ParentSubClass;
                foreach (var pc in sc)
                {
                    var className = pc.Attributes[_xClassName];
                    if (!f.Classes.Contains(className))
                    {
                        if (f.Classes.Count <= 1) f.AddSubClass("", className);
                        else f.AddClass(className);
                    }
                }

                foreach (var pc in sc)
                {
                    var className = pc.Attributes[_xClassName];
                    for (var j = 0; j < pc.SubClass[_xSubClass].Count; j++)
                    {
                        var subClassName = pc.SubClass[_xSubClass][j].Attributes[_xSubClassName];
                        var relationship = _xSubClassRelationship != -1
                            ? pc.SubClass[_xSubClass][j].Attributes[_xSubClassRelationship]
                            : "";
                        var reference = _xSubClassReference != -1
                            ? pc.SubClass[_xSubClass][j].Attributes[_xSubClassReference]
                            : "";
                        if (relationship.Equals("Extends", StringComparison.InvariantCultureIgnoreCase))
                            f.AddInheritor(className, subClassName);
                        else
                            f.AddSubClass(className, subClassName, reference);
                    }
                }

                foreach (var pc in sc)
                {
                    var className = pc.Attributes[_xClassName];
                    var iClass = f.GetClassId(className);
                    var @class = f.GetClassDef(iClass);
                    for (var j = 0; j < pc.SubClass[_xProperty].Count; j++)
                    {
                        var name = pc.SubClass[_xProperty][j].Attributes[_xPropertyName];
                        if (!@class.IsInherited || !@class.Parent.Properties.Contains(name))
                        {
                            @class.AddInstanceProperty(name);
                            @class.Properties.Add(name);
                        }
                    }
                }
            }
        }
    }
}
