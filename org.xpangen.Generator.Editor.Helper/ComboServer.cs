using System.Collections.Generic;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Codes;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Editor.Helper
{
    /// <summary>
    /// Caches combo items
    /// </summary>
    public class ComboServer
    {
        private readonly Root _codes;
        private readonly Dictionary<string, List<GeComboItem>> _cache;
        
        /// <summary>
        /// Create a new <see cref="ComboServer"/> instance.
        /// </summary>
        public ComboServer()
        {
            var data = GenData.DataLoader.LoadData(GenData.DataLoader.LoadData("CodesDefinition").AsDef(),
                                       "Data/Standard Editor Codes.dcb");

            _codes = new Root(data); // { GenObject = data.Root };
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

            var table = _codes.CodesTableList.Find(name);
            if (table != null)
            {
                var list = new List<GeComboItem>();
                for (var i = 0; i < table.CodeList.Count; i++)
                {
                    var item = new GeComboItem(table.CodeList[i].Name, table.CodeList[i].Value);
                    list.Add(item);
                }
                _cache.Add(name, list);
                return list;
            }
            return null;
        }
    }
}