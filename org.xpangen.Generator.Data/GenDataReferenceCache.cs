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

        public GenData this[string defPath, string path]
        {
            get
            {
                if (path.Equals("self", StringComparison.InvariantCultureIgnoreCase))
                    return Self;
                var f = AddDef(defPath);
                if (!LocalCache.ContainsKey(path))
                {
                    var fullPath = Path.GetExtension(path) == "" ? path + ".dcb" : path;
                    if (!File.Exists(fullPath))
                        throw new ArgumentException("The lookup path is undefined: " + path, "path");
                    var d = GenData.DataLoader.LoadData(f.AsDef(), fullPath);
                    LocalCache.Add(path, d);
                }
                return LocalCache[path];
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
            var n = name.ToLowerInvariant();
            if (n.Equals("self"))
                throw new ArgumentException("The 'self' generator data cannot be added explicitly to the cache", "name");
            AddDef(def);
            if (!LocalCache.ContainsKey(n))
                LocalCache.Add(n, genData);
            return genData;
        }

        private GenData AddDef(string def)
        {
            var f = def.ToLowerInvariant().Replace('/', '\\');
            GenData d;
            if (f.Equals("minimal"))
            {
                if (!LocalCache.ContainsKey(f))
                {
                    d = GenDataDef.CreateMinimal().AsGenData();
                    LocalCache.Add(f, d);
                }
                else
                    d = LocalCache[f];
            }
            else if (!LocalCache.ContainsKey(f))
            {
                d = GenData.DataLoader.LoadData(Path.GetExtension(f) == "" ? f + ".dcb" : f);
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
}