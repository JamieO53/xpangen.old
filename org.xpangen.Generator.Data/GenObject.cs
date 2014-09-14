// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace org.xpangen.Generator.Data
{
    public class GenObject : GenBase, IGenObject
    {
        private GenDataBase _genDataBase;

        public GenObject(GenObject parent, ISubClassBase parentSubClass, int classId)
        {
            Parent = parent;
            RefParent = null;
            ParentSubClass = parentSubClass;
            ClassId = classId;
            Attributes = new TextList();

            if (Parent == null) return;
            GenDataBase = Parent.GenDataBase;
            if (Definition != null && Definition.IsInherited)
            {
                Assert(Definition.Parent.Inheritors.Contains(Definition),
                       "The new object is inherited, but not from its parent");
                Assert(!Definition.Parent.IsInherited && parentSubClass.ClassId == Definition.Parent.ClassId ||
                    Definition.Parent.IsInherited && parentSubClass.ClassId == Definition.Parent.Parent.ClassId,
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
        public GenObject RefParent { private get; set; }
        public GenSubClasses SubClass { get; private set; }

        public GenDataDefClass Definition
        {
            get
            {
                if (GenDataBase != null && GenDataBase.GenDataDef != null)
                    return GenDataBase.GenDataDef.GetClassDef(ClassId);
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
            bool notFound;
            return GetValue(id, out notFound);
        }

        public string GetValue(GenDataId id, out bool notFound)
        {
            string value = null;
            if (id.ClassName == ClassName)
            {
                var indexOfClass = GenDataBase.GenDataDef.GetClassId(id.ClassName);
                var indexOfProperty = GenDataBase.GenDataDef.GetClassProperties(indexOfClass).IndexOf(id.PropertyName);
                if (GenDataBase.GenDataDef.GetClassDef(indexOfClass).IsPseudo(indexOfProperty))
                {
                    notFound = true;
                    return "";
                }
                var idx = Definition.Properties.IndexOf(id.PropertyName);
                if (idx == -1)
                    value = "<<<< Invalid Lookup: " + id + " Property not found >>>>";
                else
                {
                    notFound = false;
                    return idx >= Attributes.Count ? "" : Attributes[idx];
                }
            }
            if (Parent != null)
            {
                value = Parent.GetValue(id, out notFound);
                if (!notFound)
                    return value;
            }
            if (RefParent != null)
            {
                value = RefParent.GetValue(id, out notFound);
                if (!notFound)
                    return value;
            }
            notFound = true;
            return value ?? "<<<< Invalid Lookup: " + id + " Class not found >>>>";
        }

        public GenSubClass GetSubClass(string subClassName)
        {
            var subClassDef = GetClassDef(subClassName);
            var subClassId = subClassDef.ClassId;
            var idx = Definition.IndexOfSubClass(subClassId);
            if (idx == -1)
                throw new GeneratorException("Cannot find subclass " + subClassName + " of " + ClassName,
                    GenErrorType.Assertion);
            var subClass = (GenSubClass)SubClass[idx];
            if (subClass.ClassId == subClassId) return subClass;
            
            var newSubClass = new GenSubClass(GenDataBase, Parent, subClassId, subClass.Definition);
            foreach (var o in subClass)
                if (subClassDef.IsInheritor(o.ClassId))
                    newSubClass.Add(o);
            return newSubClass;
        }

        public GenObject CreateGenObject(string className)
        {
            var classId = GetClassId(className);
            var k = Definition.IndexOfSubClass(classId);
            var l = SubClass[k] as GenSubClass;
            return l != null ? l.CreateObject(classId) : null;
        }

        private GenDataDefClass GetClassDef(string className)
        {
            return GenDataBase.GenDataDef.GetClassDef(className);
        }

        private int GetClassId(string className)
        {
            return GenDataBase.GenDataDef.GetClassId(className);
        }
    }
}