// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenData
    {
        public List<GenObjectList> Context { get; private set; }
        public GenDataBase GenDataBase { get; private set; }
        public GenDataDef GenDataDef { get { return GenDataBase.GenDataDef; } }

        public bool Changed
        {
            get { return GenDataBase.Changed; }
        }

        public GenObject Root
        {
            get { return GenDataBase.Root; }
        }

        public GenDataReferenceCache Cache { get; private set; }

        public static IGenDataLoader DataLoader { get; set; }

        public GenData(GenDataDef genDataDef) : this(new GenDataBase(genDataDef, false))
        {
        }

        private GenData(GenDataBase genDataBase)
        {
            Cache = new GenDataReferenceCache(this);
            GenDataBase = genDataBase;
            Context = new List<GenObjectList>();
            for (var i = 0; i < GenDataDef.Classes.Count; i++)
                Context.Add(null);

            Context[0] =
                new GenObjectList(new GenObjectListBase(GenDataBase, null, 0,
                                                        new GenDataDefSubClass {SubClass = GenDataDef.Classes[0]})
                                      {GenDataBase.Root});
            First(0);
            GenDataBase.Changed = false;
        }
        
        public GenObject CreateObject(string parentClassName, string className)
        {
            var parent = Context[GenDataDef.Classes.IndexOf(parentClassName)].GenObject;
            var o = GenDataBase.CreateGenObject(className, parent);
            EstablishContext(o);
            return o;
        }

        public void EstablishContext(GenObject genObject)
        {
            if (genObject == null || genObject.ParentSubClass == null) return;
            
            var i = 0;
            while (i < genObject.Parent.SubClass.Count && genObject.Parent.SubClass[i].ClassId != genObject.ClassId)
                i++;
            if (i < genObject.Parent.SubClass.Count)
            {
                if (Context[genObject.ClassId] == null)
                    Context[genObject.ClassId] = new GenObjectList(genObject.Parent.SubClass[i]);
                else
                    Context[genObject.ClassId].GenObjectListBase = genObject.Parent.SubClass[i];
                Context[genObject.ClassId].Index = Context[genObject.ClassId].IndexOf(genObject);
            }
            EstablishContext(genObject.Parent);
        }

        public void First(int classId)
        {
            if (!Eol(classId))
                ResetSubClasses(classId);
            Context[classId].First();
            if (!Eol(classId))
                SetSubClasses(classId);
        }

        public void Next(int classId)
        {
            if (!Eol(classId))
                ResetSubClasses(classId);
            Context[classId].Next();
            if (!Eol(classId))
                SetSubClasses(classId);
        }

        public void Last(int classId)
        {
            if (!Eol(classId))
                ResetSubClasses(classId);
            Context[classId].Last();
            if (!Eol(classId))
                SetSubClasses(classId);
        }

        public void Prior(int classId)
        {
            if (!Eol(classId))
                ResetSubClasses(classId);
            Context[classId].Prior();
            if (!Eol(classId))
                SetSubClasses(classId);
        }

        public bool Eol(int classId)
        {
            return Context[classId] == null || Context[classId].Eol;
        }

        public void Reset(int classId)
        {
            if (!Eol(classId))
                ResetSubClasses(classId);
        }

        private void ResetSubClasses(int classId)
        {
            for (var i = 0; i < GenDataDef.Classes[classId].SubClasses.Count; i++)
            {
                var subClassId = GenDataDef.Classes[classId].SubClasses[i].SubClass.ClassId;
                if (Context[subClassId] != null)
                    if (!Eol(subClassId))
                    {
                        Reset(subClassId);
                        Context[subClassId].Reset();
                    }
            }
        }

        internal void SetSubClasses(int classId)
        {
            for (var i = 0; i < GenDataDef.Classes[classId].SubClasses.Count; i++)
            {
                var subClassId = GenDataDef.Classes[classId].SubClasses[i].SubClass.ClassId;
                if (Context[classId].GenObject == null)
                    First(classId);
                var genObject = Context[classId].GenObject;
                if (genObject != null)
                    if (Context[subClassId] == null)
                        Context[subClassId] = new GenObjectList(genObject.SubClass[i]);
                    else
                        Context[subClassId].GenObjectListBase = genObject.SubClass[i];
                Context[subClassId].First();
            }
        }

        public string GetValue(GenDataId id)
        {
            try
            {
                if (
                    String.Compare(GenDataDef.Classes[id.ClassId].Properties[id.PropertyId], "First",
                                   StringComparison.OrdinalIgnoreCase) == 0)
                    return Context[id.ClassId].IsFirst() ? "True" : "";
                return GetValueForId(id);
            }
            catch (Exception)
            {
                string c;
                string p;
                
                try
                {
                    c = IdClass(id);
                }
                catch(Exception)
                {
                    c = "Unknown class";
                }
                try
                {
                    p = IdProperty(id);
                }
                catch (Exception)
                {
                    p = "Unknown property";
                }
                return string.Format("<<<< Invalid Value Lookup: {0}.{1} >>>>", c, p);
            }
        }

        private string GetValueForId(GenDataId id)
        {
            var context = Context[id.ClassId].GenObject;
            return id.PropertyId >= context.Attributes.Count ? "" : context.Attributes[id.PropertyId];
        }

        private string IdClass(GenDataId id)
        {
            return GenDataDef.Classes[id.ClassId].Name;
        }

        private string IdProperty(GenDataId id)
        {
            return GenDataDef.Classes[id.ClassId].Properties[id.PropertyId];
        }

        // The result is only defined if the Class, SubClass and Property classes are
        // defined, and SubClass and Property are sub-classes of Class.
        // If defined, the definition is built up by walking the structure and finding
        // all the classes and summarising the result.
        public GenDataDef AsDef()
        {
            return GenDataBase.AsDef();
        }

        public GenData DuplicateContext()
        {
            var d = new GenData(GenDataBase);
            for (var i = 0; i < Context.Count; i++)
                if (Context[i] != null)
                    d.Context[i] = new GenObjectList(Context[i].GenObjectListBase) {Index = Context[i].Index};
                else d.Context[i] = null;

            return d;
        }

        /// <summary>
        /// Find an item with the specified value.
        /// </summary>
        /// <param name="id">The Class / Property identity being sought.</param>
        /// <param name="val">The value being sought.</param>
        /// <returns>The value has been found.</returns>
        public bool Find(GenDataId id, string val)
        {
            if (Eol(id.ClassId))
                return false;
            if (Context[id.ClassId].GenObject.Attributes[id.PropertyId] == val)
                return true;

            First(id.ClassId);
            while (!Eol(id.ClassId) && Context[id.ClassId].GenObject.Attributes[id.PropertyId] != val)
                Next(id.ClassId);
            return !Eol(id.ClassId);
        }

        /// <summary>
        /// Find all items with the specified value.
        /// </summary>
        /// <param name="id">The Class / Property identity being sought.</param>
        /// <param name="val">The value being sought.</param>
        /// <returns>The value has been found.</returns>
        public List<GenObject> FindMatches(GenDataId id, string val)
        {
            var list = new List<GenObject>();
            First(id.ClassId);
            while (!Eol(id.ClassId))
            {
                if (Context[id.ClassId].GenObject.Attributes[id.PropertyId] == val)
                    list.Add(Context[id.ClassId].GenObject);
            }
            return list;
        }
    }
}
