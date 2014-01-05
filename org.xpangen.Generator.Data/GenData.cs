// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace org.xpangen.Generator.Data
{
    public class GenData
    {
        public GenDataContext Context { get; private set; }
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

        public GenData(GenDataDef genDataDef) : this(new GenDataBase(genDataDef))
        {
        }

        public GenData(GenDataBase genDataBase)
        {
            GenDataBase = genDataBase;
            Context = new GenDataContext(GenDataBase);
            for (var i = 0; i < GenDataDef.Classes.Count; i++)
                Context.Add(null);

            Context[0] = new GenObjectList(new GenObjectListBase(GenDataBase, null, 0) { GenDataBase.Root });
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
                Context[genObject.ClassId] = new GenObjectList(genObject.Parent.SubClass[i]);
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
                        Context[subClassId] = null;
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
                Context[subClassId] = new GenObjectList(Context[classId].GenObject.SubClass[i]);
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
    }
}
