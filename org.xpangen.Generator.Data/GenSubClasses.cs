﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenSubClasses : List<ISubClassBase>
    {
        private GenObject Parent { get; set; }

        public GenSubClasses(GenObject parent)
        {
            Parent = parent;
            var parentDef = Parent.Definition;
            AddInheritedSubClasses(parentDef, parent.GenDataBase);
            AddSubClasses(parentDef, parent.GenDataBase);
        }

        private void AddSubClasses(GenDataDefClass parentDef, GenDataBase genDataBase)
        {
            if (parentDef != null)
                foreach (var t in parentDef.SubClasses)
                    AddSubClass(t, genDataBase);
        }

        private void AddInheritedSubClasses(GenDataDefClass parentDef, GenDataBase genDataBase)
        {
            if (parentDef != null && parentDef.IsInherited)
            {
                AddInheritedSubClasses(parentDef.Parent, genDataBase);
                AddSubClasses(parentDef.Parent, genDataBase);
            }
        }

        private void AddSubClass(GenDataDefSubClass subClassDef, GenDataBase genDataBase)
        {
            var subClassClassDef = subClassDef.SubClass;
            if (string.IsNullOrEmpty(subClassDef.ReferenceDefinition))
                Add(new GenSubClass(genDataBase, Parent, subClassClassDef.ClassId, subClassDef));
            else
                Add(new SubClassReference(genDataBase, Parent, subClassClassDef.ClassId, subClassDef));
        }
    }
}