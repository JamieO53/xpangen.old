// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

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

        private GenData GenData { get; set; }
        private GenDataDef GenDataDef { get; set; }

        public GenDataToDef(GenData genData)
        {
            GenData = genData;
            GenDataDef = GenData.GenDataDef;
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
            var f = new GenDataDef();
            Navigate(GenData.Root, f);

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
                GenData.EstablishContext(c);
                GenData.Context[_nClass].First();
                while (!GenData.Context[_nClass].Eol)
                {
                    AsDefSubClass(f, GenData.Context[_nClass].Context, GenData.Context[_nClass].IsFirst());
                    GenData.Context[_nClass].Next();
                }
            }
        }

        private void AsDefSubClass(GenDataDef data, GenObject classObject, bool first)
        {
            var attributes = new GenAttributes(GenDataDef) { GenObject = classObject };
            var sName = attributes.AsString("Name");
            if (first)
                data.AddSubClass("", sName);
            var iClass = data.AddClass(sName);
            for (var i = 0; i < classObject.SubClass[_xProperty].Count; i++)
            {
                attributes.GenObject = classObject.SubClass[_xProperty][i];
                data.Properties[iClass].Add(attributes.AsString("Name"));
            }
            for (var i = 0; i < classObject.SubClass[_xSubClass].Count; i++)
            {
                attributes.GenObject = classObject.SubClass[_xSubClass][i];
                data.AddSubClass(sName, attributes.AsString("Name"));
            }
        }
    }
}
