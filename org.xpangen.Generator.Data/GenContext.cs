// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
        /// <param name="genDataBase">The underlying data.</param>
        public void Duplicate(GenContext context, GenDataBase genDataBase)
        {
            Classes.AddRange(context.Classes);
            foreach (var reference in Cache.References)
                context.Cache.Internal(reference.Path, reference.GenData);
            for (var i = 0; i < context.Count; i++)
            {
                var classDef = Classes[i];
                var x = context[i];
                this[i] = new GenObjectList(x.SubClassBase, genDataBase,
                    x.ParentList == null ? null : this[x.ParentList.ClassId],
                    x.DefSubClass)
                          {
                              Index = x.Index,
                              ClassId = classDef.ClassId,
                              RefClassId = classDef.RefClassId,
                              Reference = x.Reference,
                              ReferenceData = x.ReferenceData
                          };
            }
        }

        public void Add(GenObjectList item, GenDataDefClass defClass, GenDataBase genDataBase, string reference,
                        string referenceDefinition, GenDataDefSubClass defSubClass)
        {
            if (defClass != null && defClass.ClassId != 0 && this[0].Count > 0 && this[0].Index == -1) this[0].Index = 0;
            GenObjectList myList;
            if (item != null) myList = item;
            else
            {
                var genObjectList = Count == 0
                    ? null
                    : this[defClass == null || defClass.Parent == null ? 0 : defClass.Parent.ClassId];
                if (reference == "")
                {
                    var subClassBase = new GenSubClass(genDataBase, null, defClass == null ? 0 : defClass.ClassId,
                        new GenDataDefSubClass
                        {
                            SubClass = defClass,
                            Reference = reference,
                            ReferenceDefinition =
                                referenceDefinition
                        });
                    myList = new GenObjectList(subClassBase, genDataBase,
                        genObjectList,
                        defSubClass)
                             {
                                 RefClassId = defClass == null ? 0 : defClass.RefClassId,
                                 ClassId = defClass == null ? 0 : defClass.ClassId,
                                 Reference = reference
                             };
                }
                else
                    myList = new GenObjectList(new SubClassReference(genDataBase, null, defClass == null ? 0 : defClass.ClassId,
                        new GenDataDefSubClass
                        {
                            SubClass = defClass,
                            Reference = reference,
                            ReferenceDefinition =
                                referenceDefinition
                        }), genDataBase,
                        genObjectList,
                        defSubClass)
                             {
                                 RefClassId = defClass == null ? 0 : defClass.RefClassId,
                                 ClassId = defClass == null ? 0 : defClass.ClassId,
                                 Reference = reference
                             };
            }
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
            bool notFound;
            var value = this[id.ClassId].GenObject.GetValue(id, out notFound);
            if (!notFound) return value;
            if (String.Compare(id.PropertyName, "Reference", StringComparison.OrdinalIgnoreCase) == 0)
                return this[id.ClassId].Reference ?? "";
            if (String.Compare(id.PropertyName, "First", StringComparison.OrdinalIgnoreCase) == 0)
                return this[id.ClassId].IsFirst() ? "True" : "";
            return value;
        }
    }
}