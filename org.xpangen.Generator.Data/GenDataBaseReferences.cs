using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenDataBaseReferences: Dictionary<string, string>
    {
        public new void Add(string key, string value)
        {
            var keyLower = key.ToLowerInvariant();
            var valueLower = value.ToLowerInvariant();
            if (!ContainsKey(keyLower))
                base.Add(keyLower, valueLower);
            if (!ContainsKey(valueLower))
                Add(valueLower, "Minimal");
        }

        public List<GenDataBaseReference> ReferenceList
        {
            get
            {
                var references = new List<GenDataBaseReference>();
                foreach (var key in Keys)
                    references.Add(new GenDataBaseReference(key, this[key]));
                return references;
            }
        }
    }
}