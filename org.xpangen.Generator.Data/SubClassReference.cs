// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class SubClassReference : List<GenObject>, ISubClassBase
    {
        private string _reference;

        public SubClassReference(GenDataBase genDataBase, GenObject parent, int classId, GenDataDefSubClass subClassDef)
        {
            GenDataBase = genDataBase;
            Definition = subClassDef;
            ClassId = classId;
            Parent = parent;
        }

        public GenDataBase GenDataBase { get; private set; }
        public GenObject Parent { get; private set; }
        public int ClassId { get; private set; }
        public GenDataDefSubClass Definition { get; private set; }

        public void Reset()
        {
            if (Definition.ReferenceDefinition != Definition.Reference)
                Clear();
        }

        public string Reference
        {
            get { return _reference; }
            set
            {
                _reference = value;
                GenDataBase.References.Add(value, Definition.Reference);
                GenDataBase.Changed = true;
                GenDataBase.RaiseDataChanged(Parent.ParentSubClass.Definition.SubClass.Name, "Reference");
            }
        }
    }
}