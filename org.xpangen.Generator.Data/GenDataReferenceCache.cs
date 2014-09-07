using System;
using System.IO;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenDataReferenceCache
    {
        private Dictionary<string, GenData> _localCache;
        private GenData Self { get; set; }

        public GenDataReferenceCache(GenData genData)
        {
            Self = genData;
        }

        public void Merge()
        {
            if (_localCache == null) return;
            foreach (var pair in References)
            {
                pair.GenData.LoadCache();
                if (pair.GenData.Cache._localCache == null)
                    continue;
                pair.GenData.Cache.Merge();
                foreach (var item in pair.GenData.Cache.References)
                    if (!LocalCache.ContainsKey(item.Path))
                        LocalCache.Add(item.Path, item.GenData);
            }
        }

        /// <summary>
        /// Returns the cached data.
        /// </summary>
        /// <param name="path">The data's locations.</param>
        /// <returns></returns>
        public GenData this[string path]
        {
            get
            {
                if (path.Equals("self", StringComparison.InvariantCultureIgnoreCase))
                    return Self;
                var p = path.ToLowerInvariant().Replace('/', '\\');
                return LocalCache.ContainsKey(p) ? LocalCache[p] : null;
            }
        }

        public void Check(string defPath, string path)
        {
            if (path.Equals("self", StringComparison.InvariantCultureIgnoreCase))
                return;
            var f = AddDef(defPath);
            var p = path.ToLowerInvariant().Replace('/', '\\');
            if (!LocalCache.ContainsKey(p))
            {
                var fullPath = Path.GetExtension(path) == "" ? p + ".dcb" : path;
                var d = GenData.DataLoader.LoadData(f.AsDef(), fullPath);
                LocalCache.Add(p, d);
                foreach (var reference in d.Cache.References)
                    if (!LocalCache.ContainsKey(reference.Path))
                        LocalCache.Add(reference.Path, reference.GenData);
            }
        }

        public List<GenDataReferenceCacheItem> References
        {
            get
            {
                var references = new List<GenDataReferenceCacheItem>();
                if (_localCache != null)
                    foreach (var item in _localCache)
                        references.Add(new GenDataReferenceCacheItem(item.Key, item.Value));
                return references;
            }
        }

        private Dictionary<string, GenData> LocalCache
        {
            get { return _localCache ?? (_localCache = new Dictionary<string, GenData>()); }
        }

        /// <summary>
        /// Cache a data file programatically.
        /// </summary>
        /// <param name="def">The definition of the definition data.</param>
        /// <param name="name">The name of the cached data.</param>
        /// <param name="genData">The data being cached.</param>
        /// <returns></returns>
        public GenData Internal(string def, string name, GenData genData)
        {
            var n = name.ToLowerInvariant().Replace('/', '\\');
            if (n.Equals("self"))
                throw new ArgumentException("The 'self' generator data cannot be added explicitly to the cache", "name");
            if (!string.IsNullOrEmpty(def)) AddDef(def);
            if (!LocalCache.ContainsKey(n))
                LocalCache.Add(n, genData);
            else return LocalCache[n];
            return genData;
        }

        /// <summary>
        /// Cache a data file programatically.
        /// </summary>
        /// <param name="name">The name of the cached data.</param>
        /// <param name="genData">The data being cached.</param>
        /// <returns></returns>
        public void Internal(string name, GenData genData)
        {
            Internal(null, name, genData);
        }

        private GenData AddDef(string def)
        {
            var f = def.ToLowerInvariant().Replace('/', '\\');
            GenData d;
            if (LocalCache.ContainsKey(f))
                d = LocalCache[f];
            else
            {
                d = f.Equals("minimal") ? GenDataDef.CreateMinimal().AsGenData() : GenData.DataLoader.LoadData(f);
                LocalCache.Add(f, d);
            }
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

    public struct GenDataReferenceCacheItem
    {
        public GenDataReferenceCacheItem(string path, GenData genData) : this()
        {
            GenData = genData;
            Path = path;
        }

        public string Path { get; private set; }
        public GenData GenData { get; private set; }
    }
}