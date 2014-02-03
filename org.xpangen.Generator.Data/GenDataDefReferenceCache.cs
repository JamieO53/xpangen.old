using System;
using System.Collections;
using System.IO;
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
                if (!defPath.Equals("self", StringComparison.InvariantCultureIgnoreCase))
                    LocalCache.Add(defPath, value);
            }
        }

        protected internal List<GenDataDefReferenceCacheItem> References
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

        /// <summary>
        /// Cache a data file programatically.
        /// </summary>
        /// <param name="def">The definition of the definition data.</param>
        /// <param name="name">The name of the cached data.</param>
        /// <param name="genData">The data being cached.</param>
        /// <returns></returns>
        public GenDataDef Internal(string name, GenDataDef genDataDef)
        {
            var n = name.ToLowerInvariant().Replace('/', '\\');
            if (n.Equals("self"))
                throw new ArgumentException("The 'self' generator data cannot be added explicitly to the cache", "name");
            if (!LocalCache.ContainsKey(n))
                LocalCache.Add(n, genDataDef);
            return genDataDef;
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
                d = GenData.DataLoader.LoadData(Path.GetExtension(f) == "" ? f + ".dcb" : f).AsDef();
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