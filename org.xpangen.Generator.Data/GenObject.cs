// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    public class GenObject : GenBase, IGenObject
    {
        private GenDataBase _genDataBase;

        public GenObject(GenObject parent, ISubClassBase parentSubClass, int classId)
        {
            Parent = parent;
            ParentSubClass = parentSubClass;
            ClassId = classId;
            Attributes = new TextList();

            if (Parent == null) return;
            GenDataBase = Parent.GenDataBase;
            if (Definition != null && Definition.IsInherited)
            {
                Assert(Definition.Parent.Inheritors.Contains(Definition),
                       "The new object is inherited, but not from its parent");
                Assert(parentSubClass.ClassId == Definition.Parent.ClassId,
                       "The new object is being added to the incorrect subclass");
            }
            else
            {
                Assert(parentSubClass.Definition == null || parentSubClass.Definition.SubClass.ClassId == ClassId,
                       "The new object is being assigned to the wrong subclass");
            }
        }

        public GenDataBase GenDataBase
        {
            get { return _genDataBase; }
            set
            {
                if (_genDataBase == value) return;
                _genDataBase = value;
                if (Definition != null)
                    for (var i = 0; i < Definition.Properties.Count; i++)
                        Attributes.Add("");
                SubClass = new GenSubClasses(this);
            }
        }

        public TextList Attributes { get; private set; }

        public ISubClassBase ParentSubClass { get; private set; }
        public int ClassId { get; set; }

        public GenObject Parent { get; private set; }
        public GenSubClasses SubClass { get; private set; }

        public GenDataDefClass Definition
        {
            get
            {
                if (GenDataBase != null && GenDataBase.GenDataDef != null)
                    return GenDataBase.GenDataDef.Classes[ClassId];
                return null;
            }
        }

        public NameList Properties
        {
            get { return Definition.Properties; }
        }

        public string ClassName
        {
            get { return Definition.Name; }
        }

        public override string ToString()
        {
            return GenDataBase + "." + Definition.Name + "." + Attributes[0];
        }

        public string GetValue(GenDataId id)
        {
            if (id.ClassName == ClassName)
            {
                var idx = Definition.Properties.IndexOf(id.PropertyName);
                if (idx == -1) return "<<<< Invalid Lookup: " + id + " Property not found >>>>";
                if (idx >= Attributes.Count) return "";
                return Attributes[idx];
            }
            if (Parent != null) return Parent.GetValue(id);
            return "<<<< Invalid Lookup: " + id + " Class not found >>>>";
        }

        public string GetValue(GenDataId id, out bool notFound)
        {
            if (id.ClassName == ClassName)
            {
                var idx = Definition.Properties.IndexOf(id.PropertyName);
                if (idx == -1)
                {
                    notFound = true;
                    return "<<<< Invalid Lookup: " + id + " Property not found >>>>";
                }
                notFound = false;
                if (idx >= Attributes.Count) return "";
                return Attributes[idx];
            }
            if (Parent != null) return Parent.GetValue(id, out notFound);
            notFound = true;
            return "<<<< Invalid Lookup: " + id + " Class not found >>>>";
        }
    }
}