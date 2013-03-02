// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenData: BindableObject
    {
        public GenDataDef GenDataDef { get; private set; }
        public List<GenObjectList> Context { get; private set; }

        public GenObject Root { get { return Context[0].Context; } }

        public bool Changed { get; set; }

        public GenData(GenDataDef genDataDef)
        {
            IgnorePropertyValidation = true;
            GenDataDef = genDataDef;
            Context = new List<GenObjectList>();
            for (var i = 0; i < genDataDef.Classes.Count; i++)
                Context.Add(null);

            var root = new GenObject(null, null, 0) {GenData = this};
            Context[0] = new GenObjectList(this, null, 0) {root};
            First(0);
            Changed = false;
        }

        public GenObject CreateObject(string parentClassName, string className)
        {
            var i = GenDataDef.Classes.IndexOf(className);
            var j = GenDataDef.Classes.IndexOf(parentClassName);
            var k = GenDataDef.IndexOfSubClass(j, i);
            var l = Context[j].Context.SubClass[k];
            var o = new GenObject(l.Parent, l, i);
            l.Add(o);
            EstablishContext(o);
            return o;
        }

        public void RaiseDataChanged(string className, string propertyName)
        {
            RaisePropertyChanged(className + '.' + propertyName);
        }
        
        public void EstablishContext(GenObject genObject)
        {
            if (genObject == null || genObject.ParentSubClass == null) return;
            
            var i = 0;
            while (i < genObject.Parent.SubClass.Count && genObject.Parent.SubClass[i].ClassId != genObject.ClassId)
                i++;
            if (i < genObject.Parent.SubClass.Count)
            {
                Context[genObject.ClassId] = genObject.Parent.SubClass[i];
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
            for (var i = 0; i < GenDataDef.SubClasses[classId].Count; i++)
            {
                var subClassId = GenDataDef.SubClasses[classId][i];
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
            for (var i = 0; i < GenDataDef.SubClasses[classId].Count; i++)
            {
                var subClassId = GenDataDef.SubClasses[classId][i];
                if (Context[classId].Context == null)
                    First(classId);
                Context[subClassId] = Context[classId].Context.SubClass[i];
                Context[subClassId].First();
            }
        }

        public string GetValue(GenDataId id)
        {
            try
            {
                if (
                    String.Compare(GenDataDef.Properties[id.ClassId][id.PropertyId], "First",
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
                return string.Format("<<<< Invalid Value Lookup: {0}.{1}", c, p);
            }
        }

        private string GetValueForId(GenDataId id)
        {
            var context = Context[id.ClassId].Context;
            return id.PropertyId >= context.Attributes.Count ? "" : context.Attributes[id.PropertyId];
        }

        private string IdClass(GenDataId id)
        {
            return GenDataDef.Classes[id.ClassId];
        }

        private string IdProperty(GenDataId id)
        {
            return GenDataDef.Properties[id.ClassId][id.PropertyId];
        }

        // The result is only defined if the Class, SubClass and Property classes are
        // defined, and SubClass and Property are sub-classes of Class.
        // If defined, the definition is built up by walking the structure and finding
        // all the classes and summarising the result.
        public GenDataDef AsDef()
        {
            var x = new GenDataToDef(this);
            return x.AsDef();
        }
    }
}
