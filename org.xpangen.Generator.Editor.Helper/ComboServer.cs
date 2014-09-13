using System;
using System.Collections.Generic;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Codes;

namespace org.xpangen.Generator.Editor.Helper
{
    /// <summary>
    /// Caches combo items
    /// </summary>
    public class ComboServer
    {
        private readonly CodesDefinition _codes;
        private readonly Dictionary<string, List<GeComboItem>> _cache;
        
        /// <summary>
        /// Create a new <see cref="ComboServer"/> instance.
        /// </summary>
        public ComboServer(GenData data)
        {
            _codes = new CodesDefinition(data);
            _cache = new Dictionary<string, List<GeComboItem>>();
        }

        /// <summary>
        /// Returns the named combo list.
        /// </summary>
        /// <param name="name">The name of the list.</param>
        /// <returns>The combo list.</returns>
        public List<GeComboItem> GetComboItems(string name)
        {
            if (_cache.ContainsKey(name)) return _cache[name];

            CodesTable table;
            if (name.Equals("lookuptables", StringComparison.InvariantCultureIgnoreCase))
            {
                var list = new List<GeComboItem>();
                foreach (var t in _codes.CodesTableList)
                {
                    table = t;
                    var item = new GeComboItem(table.Title, table.Name);
                    list.Add(item);
                }
                return list;
            }
            
            table = _codes.CodesTableList.Find(name);
            if (table != null)
            {
                var list = new List<GeComboItem>();
                for (var i = 0; i < table.CodeList.Count; i++)
                {
                    var item = new GeComboItem(table.CodeList[i].Description, table.CodeList[i].Value);
                    list.Add(item);
                }
                _cache.Add(name, list);
                return list;
            }
            return null;
        }
    }
}