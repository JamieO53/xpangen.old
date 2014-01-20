using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenContext : List<GenObjectList>
    {
        public GenContext()
        {
            Classes = new GenDataDefClassList();
        }

        public GenDataDefClassList Classes { get; private set; }

        /// <summary>
        /// Copy the contents of a context to this context.
        /// </summary>
        /// <param name="context">The context being copied.</param>
        public void Duplicate(GenContext context)
        {
            Classes.AddRange(context.Classes);
            for (var i = 0; i < context.Count; i++)
                //if (context[i] != null)
                    this[i] = new GenObjectList(context[i].GenObjectListBase) { Index = context[i].Index };
                //else this[i] = null;
        }

        public void Add(GenObjectList item, GenDataDefClass defClass, GenDataBase genDataBase)
        {
            Add(item ??
                new GenObjectList(new GenObjectListBase(genDataBase, null, 0,
                                                        new GenDataDefSubClass {SubClass = defClass}))
                    {
                        ClassIdOffset = 0
                    });
            Classes.Add(defClass);
        }

        /// <summary>
        /// Get the text value for the id.
        /// </summary>
        /// <param name="id">The identifier being looked up.</param>
        /// <returns>The text corresponding to the id.</returns>
        public string GetValue(GenDataId id)
        {
            try
            {
                if (
                    String.Compare(Classes[id.ClassId].Properties[id.PropertyId], "First",
                                   StringComparison.OrdinalIgnoreCase) == 0)
                    return this[id.ClassId].IsFirst() ? "True" : "";
                var o = this[id.ClassId].GenObject;
                return id.PropertyId >= o.Attributes.Count ? "" : o.Attributes[id.PropertyId];
            }
            catch (Exception)
            {
                string c;
                string p;

                try
                {
                    c = id.ClassName;
                }
                catch (Exception)
                {
                    c = "Unknown class";
                }
                try
                {
                    p = id.PropertyName;
                }
                catch (Exception)
                {
                    p = "Unknown property";
                }
                return string.Format("<<<< Invalid Value Lookup: {0}.{1} >>>>", c, p);
            }
        }
    }
}