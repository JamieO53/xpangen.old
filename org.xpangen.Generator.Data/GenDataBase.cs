// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenDataBase : BindableObject
    {
        private GenDataBaseReferences _references;
        public Dictionary<string, GenDataBase> Cache { get; private set; }

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

        public GenDataBase CheckReference(string defFile, string dataFile)
        {
            var fn = dataFile.ToLowerInvariant().Replace('\\', '/');
            GenDataBase d;
            if (Cache.ContainsKey(fn))
                d = Cache[fn];
            else
            {
                var df = defFile.ToLowerInvariant();
                var f = DataLoader.LoadData(df);

                d = DataLoader.LoadData(f.AsDef(), fn);
                Cache.Add(fn, d);
            }

            foreach (var key in d.Cache.Keys)
                if (!Cache.ContainsKey(key)) Cache.Add(key, d.Cache[key]);
            return d;
        }

        public void CheckReference(string dataFile, GenDataBase genData)
        {
            var fn = dataFile.ToLowerInvariant().Replace('\\', '/');
            if (!Cache.ContainsKey(fn))
                Cache.Add(fn, genData);
            foreach (var key in genData.Cache.Keys)
                if (!Cache.ContainsKey(key)) Cache.Add(key, genData.Cache[key]);
        }

        public override string ToString()
        {
            return DataName;
        }

        /// <summary>
        ///     The data loader for reference data.
        /// </summary>
        public static IGenDataLoader DataLoader { get; set; }

        public int GetClassId(string className)
        {
            return GenDataDef.GetClassId(className);
        }
    }
}