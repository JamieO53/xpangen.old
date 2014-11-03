// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace org.xpangen.Generator.Data
{
    public class GenObject : IGenObject
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
                Contract.Assert(Definition.Parent.Inheritors.Contains(Definition),
                       "The new object is inherited, but not from its parent");
                Contract.Assert(!Definition.Parent.IsInherited && parentSubClass.ClassId == Definition.Parent.ClassId ||
                    Definition.Parent.IsInherited && parentSubClass.ClassId == Definition.Parent.Parent.ClassId,
                       "The new object is being added to the incorrect subclass");
            }
            else
            {
                Contract.Assert(parentSubClass.Definition == null || parentSubClass.Definition.SubClass.ClassId == ClassId,
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
        public int ClassId { get; private set; }

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
                    if (id.PropertyName.Equals("First", StringComparison.InvariantCultureIgnoreCase))
                    {
                        notFound = false;
                        return ParentSubClass.IndexOf(this) == 0 ? "True" : "";
                    }
                    if (id.PropertyName.Equals("Reference", StringComparison.InvariantCultureIgnoreCase))
                    {
                        notFound = false;
                        return ParentSubClass.Reference ?? "";
                    }
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
            if (ClassNameIs(subClassName))
            {
                var subClass = new GenSubClass(GenDataBase, Parent, ClassId, ParentSubClass.Definition);
                subClass.Add(this);
                return subClass;
            }
            var subClassId = GenDataDef.GetClassId(subClassName);
            return subClassId == -1 ? null : GetSubClass(subClassId);
        }

        private GenSubClass GetSubClass(int subClassId)
        {
            Contract.Requires(subClassId >= 0 && subClassId < GenDataDef.Classes.Count);
            var subClassDef = GenDataDef.Classes[subClassId];
            var subClassName = subClassDef.Name;
            var idx = Definition.IndexOfSubClass(subClassName);
            return idx == -1 ? null : GetSubClassByIndex(subClassId, idx, subClassDef);
        }

        private GenSubClass GetSubClassByIndex(int subClassId, int idx, GenDataDefClass subClassDef)
        {
            var subClassRef = SubClass[idx] as SubClassReference;
            if (subClassRef != null)
            {
                if (String.IsNullOrEmpty(subClassRef.Reference)) return GenSubClass.Empty;
                var d = GenDataBase.CheckReference(subClassRef.Definition.Reference, subClassRef.Reference);
                foreach (var o in d.Root.SubClass[0])
                    o.RefParent = this;
                return (GenSubClass) d.Root.SubClass[0];
            }

            var subClass = (GenSubClass) SubClass[idx];
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
            var k = Definition.IndexOfSubClass(className);
            var l = SubClass[k] as GenSubClass;
            return l != null ? l.CreateObject(classId) : null;
        }

        private int GetClassId(string className)
        {
            return GenDataDef.GetClassId(className);
        }

        private GenDataDef GenDataDef
        {
            get { return GenDataBase.GenDataDef; }
        }

        public GenObject SearchFor(GenDataId id, string value)
        {
            var searchObjects = FindSearchObjects(id.ClassName);
            if (searchObjects == null) return null;
            foreach (var searchObject in searchObjects)
            {
                bool notFound;
                var s = searchObject.GetValue(id, out notFound);
                if (!notFound && s == value) return searchObject;
            }
            return null;
        }

        private IEnumerable<GenObject> FindSearchObjects(string className)
        {
            var genObject = this;
            while (genObject != null)
            {
                if (genObject.ClassName.Equals(className, StringComparison.InvariantCultureIgnoreCase))
                    return genObject.ParentSubClass;
                var classId = GenDataDef.GetClassId(genObject.ClassName);
                var idx = GenDataDef.GetClassSubClasses(classId).IndexOf(className);
                if (idx != -1) return genObject.SubClass[idx];
                
                genObject = genObject.Parent;
            }
            return null;
        }

        private bool ClassNameIs(string className)
        {
            return ClassName.Equals(className, StringComparison.InvariantCultureIgnoreCase);
        }

        public static GenObject GetContext(GenObject genObject, string className)
        {
            if (genObject == null || genObject.ClassNameIs(className)) return genObject;
            var ancestorContext = GetAncestorContext(genObject, className);
            if (ancestorContext != null && ancestorContext.ClassNameIs(className))
                return ancestorContext;
            var descendentContext = GetDescendentContext(genObject, className);
            if (descendentContext != null &&
                descendentContext.ClassNameIs(className))
                return descendentContext;
            if (descendentContext != null && descendentContext.Definition.IsInherited &&
                descendentContext.Definition.Parent.ClassNameIs(className))
                return descendentContext;
            return null;
        }

        private static GenObject GetAncestorContext(GenObject genObject, string className)
        {
            while (true)
            {
                if (genObject.Parent == null) return genObject;
                if (genObject.Parent.ClassName.Equals(className, StringComparison.InvariantCultureIgnoreCase))
                    return genObject.Parent;
                var descendentContext = GetDescendentContext(genObject.Parent, className, genObject.ParentSubClass);
                if (descendentContext != null &&
                    descendentContext.ClassName.Equals(className, StringComparison.InvariantCultureIgnoreCase))
                    return descendentContext;
                genObject = genObject.RefParent ?? genObject.Parent;
            }
        }

        private static GenObject GetDescendentContext(GenObject genObject, string className, ISubClassBase exclude = null)
        {
            var subClass = genObject.GetSubClass(className);
            if (subClass != null)
            {
                if (subClass.Definition.SubClass.IsInheritor(className))
                    return subClass.Count == 0 ? null : subClass[0];
                return null;
            }

            foreach (var s in genObject.SubClass)
            {
                var sc = genObject.GetSubClass(s.ClassId);
                if (sc == exclude) continue;
                if (sc.Definition.SubClass.IsInheritor(className))
                    return sc.Count == 0 ? null : sc[0];
                var descendentContext = sc.Count == 0 ? null : GetDescendentContext(sc[0], className);
                if (descendentContext != null &&
                    descendentContext.ClassName.Equals(className, StringComparison.InvariantCultureIgnoreCase))
                    return descendentContext;
            }
            return genObject;
        }
    }
}