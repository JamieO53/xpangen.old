﻿// This Source Code Form is subject to the terms of the Mozilla Public
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

        private GenDataBase GenDataBase { get; set; }
        private GenDataDef GenDataDef { get; set; }

        public GenDataToDef(GenDataBase genData)
        {
            GenDataBase = genData;
            GenDataDef = GenDataBase.GenDataDef;
            _nClass = GenDataDef.Classes.IndexOf("Class");
            _nSubClass = GenDataDef.Classes.IndexOf("SubClass");
            _nProperty = GenDataDef.Classes.IndexOf("Property");

            if (_nClass == -1 || _nSubClass == -1 || _nProperty == -1)
                return;

            _xSubClass = GenDataDef.IndexOfSubClass(_nClass, _nSubClass);
            _xProperty = GenDataDef.IndexOfSubClass(_nClass, _nProperty);
        }
    
        public GenDataDef AsDef()
        {
            var f = new GenDataDef {Definition = GenDataBase.DataName};
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
                    AsDefSubClass(f, sc[i], i == 0);
            }
        }

        private void AsDefSubClass(GenDataDef data, GenObject classObject, bool first)
        {
            var attributes = new GenAttributes(GenDataDef) { GenObject = classObject };
            var sName = attributes.AsString("Name");
            if (first)
                data.AddSubClass("", sName);
            var iClass = data.AddClass(sName);

            // todo: properties not to be added for an extension class if it belongs to the superclass
            var pAttributes = new GenAttributes(GenDataDef);
            for (var i = 0; i < classObject.SubClass[_xProperty].Count; i++)
            {
                pAttributes.GenObject = classObject.SubClass[_xProperty][i];
                data.Classes[iClass].Properties.Add(pAttributes.AsString("Name"));
            }

            var scAttributes = new GenAttributes(GenDataDef);
            for (var i = 0; i < classObject.SubClass[_xSubClass].Count; i++)
            {
                scAttributes.GenObject = classObject.SubClass[_xSubClass][i];
                if (scAttributes.AsString("Relationship").Equals("Extends", StringComparison.InvariantCultureIgnoreCase))
                    data.AddInheritor(sName, scAttributes.AsString("Name"));
                else
                    data.AddSubClass(sName, scAttributes.AsString("Name"), scAttributes.AsString("Reference"));
            }
        }
    }
}
