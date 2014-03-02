using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenContext : List<GenObjectList>
    {
        public GenContext(GenData genData)
        {
            Cache = new GenDataReferenceCache(genData);
            Classes = new GenDataDefClassList();
        }

        public GenDataDefClassList Classes { get; private set; }

        public GenDataReferenceCache Cache { get; private set; }

        /// <summary>
        /// Copy the contents of a context to this context.
        /// </summary>
        /// <param name="context">The context being copied.</param>
        public void Duplicate(GenContext context, GenDataBase genDataBase)
        {
            Classes.AddRange(context.Classes);
            for (var i = 0; i < Cache.References.Count; i++)
                context.Cache.Internal(Cache.References[i].Path, Cache.References[i].GenData);
            for (var i = 0; i < context.Count; i++)
                this[i] = new GenObjectList(context[i].GenObjectListBase, genDataBase)
                              {
                                  Index = context[i].Index,
                                  ClassId = Classes[i].ClassId,
                                  RefClassId = Classes[i].RefClassId,
                                  Reference = context[i].Reference,
                                  ReferenceData = context[i].ReferenceData
                              };
        }

        public void Add(GenObjectList item, GenDataDefClass defClass, GenDataBase genDataBase, string reference, string referenceDefinition)
        {
            GenObjectList myList;
            if (item != null) myList = item;
            else if (reference == "")
                myList = new GenObjectList(new GenObjectListBase(genDataBase, null, defClass.ClassId,
                                                                 new GenDataDefSubClass
                                                                     {
                                                                         SubClass = defClass,
                                                                         Reference = reference,
                                                                         ReferenceDefinition =
                                                                             referenceDefinition
                                                                     }), genDataBase)
                             {
                                 RefClassId = defClass.RefClassId,
                                 ClassId = defClass.ClassId,
                                 Reference = reference
                             };
            else
                myList = new GenObjectList(new GenObjectListReference(genDataBase, null, defClass.ClassId,
                                                                 new GenDataDefSubClass
                                                                     {
                                                                         SubClass = defClass,
                                                                         Reference = reference,
                                                                         ReferenceDefinition =
                                                                             referenceDefinition
                                                                     }), genDataBase)
                             {
                                 RefClassId = defClass.RefClassId,
                                 ClassId = defClass.ClassId,
                                 Reference = reference
                             };
            Add(myList);
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
                if (Classes[id.ClassId].IsPseudo(id.PropertyId))
                {
                    if (String.Compare(Classes[id.ClassId].Properties[id.PropertyId], "First", StringComparison.OrdinalIgnoreCase) == 0)
                        return this[id.ClassId].IsFirst() ? "True" : "";
                    if (String.Compare(Classes[id.ClassId].Properties[id.PropertyId], "Reference", StringComparison.OrdinalIgnoreCase) == 0)
                        return this[id.ClassId].Reference ?? "";
                }
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