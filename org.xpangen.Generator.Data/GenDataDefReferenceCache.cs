// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenDataDefReferenceCache
    {
        private Dictionary<string, GenDataDef> _localCache;
        private GenDataDef Self { get; set; }

        public GenDataDefReferenceCache(GenDataDef genData)
        {
            Self = genData;
        }

        /// <summary>
        /// Check if the cache does not contain the data, and adds it then returns the cached data.
        /// </summary>
        /// <param name="defPath">The data's definition location.</param>
        /// <returns></returns>
        public GenDataDef this[string defPath]
        {
            get
            {
                if (defPath.Equals("self", StringComparison.InvariantCultureIgnoreCase))
                    return Self;
                var f = AddDef(defPath);
                return f;
            }
            set
            {
                var path = defPath.ToLowerInvariant();
                if (!defPath.Equals("self", StringComparison.InvariantCultureIgnoreCase) && !LocalCache.ContainsKey(path))
                    LocalCache.Add(path, value);
            }
        }

        public List<GenDataDefReferenceCacheItem> References
        {
            get
            {
                var references = new List<GenDataDefReferenceCacheItem>();
                if (_localCache != null)
                    foreach (var item in _localCache)
                        references.Add(new GenDataDefReferenceCacheItem(item.Key, item.Value));
                return references;
            }
        }

        private Dictionary<string, GenDataDef> LocalCache
        {
            get { return _localCache ?? (_localCache = new Dictionary<string, GenDataDef>()); }
        }

        public int Count
        {
            get { return _localCache == null ? 0 : LocalCache.Count; }
        }

        /// <summary>
        /// Cache a data file programatically.
        /// </summary>
        /// <param name="name">The name of the cached data.</param>
        /// <param name="genDataDef">The data being cached.</param>
        /// <returns></returns>
        public void Internal(string name, GenDataDef genDataDef)
        {
            var n = name.ToLowerInvariant().Replace('/', '\\');
            if (n.Equals("self"))
                throw new ArgumentException("The 'self' generator data cannot be added explicitly to the cache", "name");
            if (!LocalCache.ContainsKey(n))
                LocalCache.Add(n, genDataDef);
        }

        private GenDataDef AddDef(string def)
        {
            var f = def.ToLowerInvariant().Replace('/', '\\');
            GenDataDef d;
            if (f.Equals("minimal"))
            {
                if (!LocalCache.ContainsKey(f))
                {
                    d = GenDataDef.CreateMinimal();
                    LocalCache.Add(f, d);
                }
                else
                    d = LocalCache[f];
            }
            else if (!LocalCache.ContainsKey(f))
            {
                d = GenData.DataLoader.LoadData(f).AsDef();
                LocalCache.Add(f, d);
            }
            else
                d = LocalCache[f];
            return d;
        }

        /// <summary>
        /// Does the cache contain the referenced data
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public bool Contains(string reference)
        {
            var d = reference.ToLowerInvariant().Replace('/', '\\');
            return d == "self" || _localCache != null && LocalCache.ContainsKey(d);
        }
    }

    public struct GenDataDefReferenceCacheItem
    {
        public GenDataDefReferenceCacheItem(string path, GenDataDef genDataDef) : this()
        {
            GenDataDef = genDataDef;
            Path = path;
        }

        public string Path { get; private set; }
        public GenDataDef GenDataDef { get; private set; }
    }
}