using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenDataDefReferenceCache
    {
        private Dictionary<string, IGenDataDef> _localCache;
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
        public IGenDataDef this[string defPath]
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

        private Dictionary<string, IGenDataDef> LocalCache
        {
            get { return _localCache ?? (_localCache = new Dictionary<string, IGenDataDef>()); }
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
        public IGenDataDef Internal(string name, IGenDataDef genDataDef)
        {
            var n = name.ToLowerInvariant().Replace('/', '\\');
            if (n.Equals("self"))
                throw new ArgumentException("The 'self' generator data cannot be added explicitly to the cache", "name");
            if (!LocalCache.ContainsKey(n))
                LocalCache.Add(n, genDataDef);
            return genDataDef;
        }

        private IGenDataDef AddDef(string def)
        {
            var f = def.ToLowerInvariant().Replace('/', '\\');
            IGenDataDef d;
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
            else if (f.Equals("definition"))
            {
                if (!LocalCache.ContainsKey(f))
                {
                    d = GenDataDef.CreateDefinition();
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
        public GenDataDefReferenceCacheItem(string path, IGenDataDef genDataDef) : this()
        {
            GenDataDef = genDataDef;
            Path = path;
        }

        public string Path { get; private set; }
        public IGenDataDef GenDataDef { get; private set; }
    }
}