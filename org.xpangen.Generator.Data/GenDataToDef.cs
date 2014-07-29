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
        private readonly int _nSubClass;
        private readonly int _nProperty;
        private readonly int _xSubClass;
        private readonly int _xProperty;
        private readonly int _xClassName;
        private readonly int _xSubClassName;
        private readonly int _xSubClassRelationship;
        private readonly int _xSubClassReference;
        private readonly int _xPropertyName;

        private GenDataBase GenDataBase { get; set; }
        private IGenDataDef GenDataDef { get; set; }

        public GenDataToDef(GenDataBase genData)
        {
            GenDataBase = genData;
            GenDataDef = GenDataBase.GenDataDef;
            _nClass = GenDataDef.Classes.IndexOf("Class");
            _nSubClass = GenDataDef.Classes.IndexOf("SubClass");
            _nProperty = GenDataDef.Classes.IndexOf("Property");
            _xClassName = GenDataDef.Classes[_nClass].Properties.IndexOf("Name");
            _xSubClassName = GenDataDef.Classes[_nSubClass].Properties.IndexOf("Name");
            _xSubClassRelationship = GenDataDef.Classes[_nSubClass].Properties.IndexOf("Relationship");
            _xSubClassReference = GenDataDef.Classes[_nSubClass].Properties.IndexOf("Reference");
            _xPropertyName = GenDataDef.Classes[_nProperty].Properties.IndexOf("Name");

            if (_nClass == -1 || _nSubClass == -1 || _nProperty == -1)
                return;

            _xSubClass = GenDataDef.IndexOfSubClass(_nClass, _nSubClass);
            _xProperty = GenDataDef.IndexOfSubClass(_nClass, _nProperty);
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
                for (var i = 0; i < sc.Count; i++)
                {
                    var className = sc[i].Attributes[_xClassName];
                    if (!f.Classes.Contains(className))
                    {
                        if (f.Classes.Count <= 1) f.AddSubClass("", className);
                        else f.AddClass(className);
                    }
                }

                for (var i = 0; i < sc.Count; i++)
                {
                    var className = sc[i].Attributes[_xClassName];
                    for (var j = 0; j < sc[i].SubClass[_xSubClass].Count; j++)
                    {
                        var subClassName = sc[i].SubClass[_xSubClass][j].Attributes[_xSubClassName];
                        var relationship = _xSubClassRelationship != -1
                                               ? sc[i].SubClass[_xSubClass][j].Attributes[_xSubClassRelationship]
                                               : "";
                        var reference = _xSubClassReference != -1
                                            ? sc[i].SubClass[_xSubClass][j].Attributes[_xSubClassReference]
                                            : "";
                        if (relationship.Equals("Extends", StringComparison.InvariantCultureIgnoreCase))
                            f.AddInheritor(className, subClassName);
                        else
                            f.AddSubClass(className, subClassName, reference);
                    }
                }

                for (var i = 0; i < sc.Count; i++)
                {
                    var className = sc[i].Attributes[_xClassName];
                    var iClass = f.Classes.IndexOf(className);
                    var @class = f.Classes[iClass];
                    for (var j = 0; j < sc[i].SubClass[_xProperty].Count; j++)
                    {
                        var name = sc[i].SubClass[_xProperty][j].Attributes[_xPropertyName];
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
