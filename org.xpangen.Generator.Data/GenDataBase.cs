// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenDataBase : BindableObject
    {
        private GenDataBaseReferences _references;
        internal Dictionary<string, GenDataBase> Cache { get; set; }

        public GenDataBase(GenDataDef genDataDef) : this(genDataDef, null)
        {
        }

        private GenDataBase(GenDataDef genDataDef, Dictionary<string, GenDataBase> cache)
        {
            Cache = cache ?? new Dictionary<string, GenDataBase>();
            GenDataDef = genDataDef;
            IgnorePropertyValidation = true;
            Root = new GenObject(null, null, 0) { GenDataBase = this };
        }

        public GenDataDef GenDataDef { get; private set; }
        public GenObject Root { get; private set; }
        public GenDataBaseReferences References
        {
            get
            {
                if (HasReferences)
                    _references = new GenDataBaseReferences();
                return _references;
            }
        }

        private bool HasReferences
        {
            get { return _references == null; }
        }

        public bool Changed { get; set; }

        public string DataName { get; set; }

        public void RaiseDataChanged(string className, string propertyName)
        {
            RaisePropertyChanged(className + '.' + propertyName);
        }

        public GenDataDef AsDef()
        {
            var x = new GenDataToDef(this);
            return x.AsDef();
        }

        public GenObject CreateGenObject(GenObject parent, string className)
        {
            return parent.CreateGenObject(className);
        }

        internal GenDataBase CheckReference(string defFile, string dataFile)
        {
            var fn = dataFile.ToLowerInvariant();
            if (Cache.ContainsKey(fn)) return Cache[fn];
            var df = defFile.ToLowerInvariant();
            var f = GenData.DataLoader.LoadData(df);

            var d = GenData.DataLoader.LoadData(f.AsDef(), fn);
            Cache.Add(fn, d.GenDataBase);
            return d.GenDataBase;
        }

        public override string ToString()
        {
            return DataName;
        }
    }
}