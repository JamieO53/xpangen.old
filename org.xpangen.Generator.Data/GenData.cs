// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace org.xpangen.Generator.Data
{
    public class GenData
    {
        public GenContext Context { get; private set; }
        public GenDataBase GenDataBase { get; private set; }
        public GenDataDef GenDataDef { get { return GenDataBase.GenDataDef; } }
        public string DataName
        {
            get
            {
                return GenDataBase.DataName;
            } 
            set
            {
                GenDataBase.DataName = value;
            }
        }
        public bool Changed
        {
            get { return GenDataBase.Changed; }
        }

        public GenObject Root
        {
            get { return GenDataBase.Root; }
        }

        public GenDataReferenceCache Cache { get { return Context.Cache; } }

        public static IGenDataLoader DataLoader { get; set; }

        public GenData(GenDataDef genDataDef) : this(new GenDataBase(genDataDef))
        {
        }

        private GenData(GenDataBase genDataBase)
        {
            GenDataBase = genDataBase;
            Context = new GenContext(this);
            for (var i = 0; i < GenDataDef.Classes.Count; i++)
            {
                var parent = GenDataDef.Classes[i].Parent;
                var reference = "";
                var referenceDefinition = "";
                if (parent != null)
                {
                    foreach (var subClass in parent.SubClasses)
                    {
                        if (subClass.SubClass != GenDataDef.Classes[i]) continue;
                        
                        reference = subClass.Reference ?? "";
                        referenceDefinition = subClass.ReferenceDefinition ?? "";
                        break;
                    }
                }
                Context.Add(null, GenDataDef.Classes[i], GenDataBase, reference, referenceDefinition);
            }

            Context[0].GenObjectListBase.Add(GenDataBase.Root);
            First(0);
            GenDataBase.Changed = false;
        }
        
        public GenObject CreateObject(string parentClassName, string className)
        {
            var parent = Context[GenDataDef.Classes.IndexOf(parentClassName)].GenObject;
            var o = GenDataBase.CreateGenObject(className, parent);
            Context[o.ClassId].GenObjectListBase = o.ParentSubClass;
            Context[o.ClassId].Index = Context[o.ClassId].IndexOf(o);
            return o;
        }

        public void EstablishContext(GenObject genObject)
        {
            if (genObject.ParentSubClass != null)
                EstablishContext(genObject.ClassId, genObject.ParentSubClass, genObject, "", 0, null);
        }

        public void EstablishContext(GenSavedContext savedContext)
        {
            EstablishContext(savedContext.ClassId, savedContext.GenObjectListBase, savedContext.GenObject,
                             savedContext.Reference, savedContext.RefClassId, savedContext.ReferenceData);
        }

        private void EstablishContext(int classId, IGenObjectListBase genObjectListBase, GenObject genObject,
                                      string reference, int refClassId, GenData referenceData)
        {
            if (string.IsNullOrEmpty(reference))
            {
                if (genObject.Parent != null && genObject.Parent.ParentSubClass != null)
                    EstablishContext(genObject.Parent.ClassId, genObject.Parent.ParentSubClass, genObject.Parent, reference, 0,
                                     this);
            }
            else if (genObject.ClassId != 0)
            {
                var parentClassId = GenDataDef.Classes[classId].Parent.ClassId;
                var parentRefClassId = referenceData.GenDataDef.Classes.IndexOf(GenDataDef.Classes[parentClassId].Name);
                EstablishContext(parentClassId, genObject.ParentSubClass, genObject.Parent, reference, parentRefClassId,
                                 referenceData);
            }

            Context[classId].GenObjectListBase = genObjectListBase;
            Context[classId].RefClassId = refClassId;
            Context[classId].ReferenceData = referenceData;
            if (referenceData == null ||
                GenDataDef.Classes[classId].Reference.Equals(referenceData.GenDataDef.Definition,
                                                             StringComparison.InvariantCultureIgnoreCase))
            {
                Context[classId].Reference = reference;
                Context[classId].Index = Context[classId].IndexOf(genObject);
            }
            else if (classId != 0)
            {
                for (var i = 0; i < GenDataDef.Classes[classId].Parent.SubClasses.Count; i++)
                {
                    if (GenDataDef.Classes[classId].Parent.SubClasses[i].Reference !=
                        referenceData.GenDataDef.Definition) continue;
                    for (var j = 0;
                         j < Context[GenDataDef.Classes[classId].Parent.ClassId].GenObject.SubClass.Count;
                         j++)
                    {
                        if (reference !=
                            Context[GenDataDef.Classes[classId].Parent.ClassId].GenObject.SubClass[j].Reference)
                            continue;
                        Context[GenDataDef.Classes[classId].Parent.ClassId].Index = j;
                        break;
                    }
                }
            }

            for (var i = 0; i < GenDataDef.Classes[classId].SubClasses.Count; i++)
            {
                var subDef = GenDataDef.Classes[classId].SubClasses[i].SubClass;
                if (string.IsNullOrEmpty(subDef.Reference)) continue;
                var sub = Context[subDef.ClassId];
// ReSharper disable PossibleNullReferenceException
                sub.GenObjectListBase = referenceData.Context[subDef.RefClassId].GenObjectListBase;
// ReSharper restore PossibleNullReferenceException
                sub.Reference = reference;
                sub.ReferenceData = referenceData;
                sub.First();
            }
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
            return Context[classId].Eol;
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

        private void SetSubClasses(int classId)
        {
            var classDef = Context.Classes[classId];
            for (var i = 0; i < classDef.SubClasses.Count; i++)
            {
                var subClass = classDef.SubClasses[i].SubClass;
                var subClassId = subClass.ClassId;
                if (Context[classId].GenObject == null)
                    First(classId);
                var genObject = Context[classId].GenObject;
                if (genObject != null)
                    if (!string.IsNullOrEmpty(classDef.SubClasses[i].Reference))
                    {
                        var referenceData = genObject.SubClass[i] as GenObjectListReference;
                        if (referenceData != null)
                            SetReferenceSubClasses(classId, i,
                                                   Cache[
                                                       referenceData.Definition.ReferenceDefinition,
                                                       referenceData.Reference],
                                                   referenceData.Reference);
                        else
                            SetReferenceSubClasses(classId, i,
                                                   Cache[
                                                       Context.Classes[subClassId].ReferenceDefinition,
                                                       Context[subClassId].Reference],
                                                   Context[subClassId].Reference);
                    }
                    else
                    {
                        Context[subClassId].GenObjectListBase = genObject.SubClass[i];
                        First(subClassId);
                    }
            }
        }

        private void SetReferenceSubClasses(int classId, int subClassIndex, GenData data, string reference)
        {
            var subClass = Context.Classes[classId].SubClasses[subClassIndex].SubClass;
            var subClassId = subClass.ClassId;
            var subRefClassId = subClass.RefClassId;
            Context[subClassId].GenObjectListBase = data.Context[subRefClassId].GenObjectListBase;
            Context[subClassId].ClassId = subClass.ClassId;
            Context[subClassId].RefClassId = subRefClassId;
            Context[subClassId].Reference = reference;
            Context[subClassId].ReferenceData = data;
            data.First(subRefClassId);
            Context[subClassId].First();
            if (!data.Eol(subRefClassId))
                for (var i = 0; i < Context.Classes[subClassId].SubClasses.Count; i++)
                    SetReferenceSubClasses(subClassId, i, data, reference);
        }

        /// <summary>
        /// Get the text value for the id.
        /// </summary>
        /// <param name="id">The identifier being looked up.</param>
        /// <returns>The text corresponding to the id.</returns>
        public string GetValue(GenDataId id)
        {
            return Context.GetValue(id);
        }

        // The result is only defined if the Class, SubClass and Property classes are
        // defined, and SubClass and Property are sub-classes of Class.
        // If defined, the definition is built up by walking the structure and finding
        // all the classes and summarising the result.
        public GenDataDef AsDef()
        {
            return GenDataBase.AsDef();
        }

        /// <summary>
        /// Create a new generator data object and duplicate the context.
        /// </summary>
        /// <returns></returns>
        public GenData DuplicateContext()
        {
            var d = new GenData(GenDataBase);
            d.Context.Duplicate(Context, GenDataBase);

            return d;
        }

        /// <summary>
        /// Cache the references in GenDataBase
        /// </summary>
        private void CacheReferences()
        {
            foreach (var reference in GenDataBase.References.ReferenceList)
            {
#pragma warning disable 168
                // Side effect: Cache saves the reference if it does not already exist
                var dummy = Cache[reference.Definition, reference.Data];
#pragma warning restore 168
            }
        }

        /// <summary>
        /// Load the cache from the data references and loaded data references
        /// </summary>
        public void LoadCache()
        {
            CacheReferences();
            Cache.Merge();
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(DataName) ? DataName : base.ToString();
        }

        public GenSavedContext SaveContext(int classId)
        {
            var sc = new GenSavedContext
                         {
                             GenData = this,
                             ClassId = classId,
                             GenObject = Context[classId].GenObject,
                             GenObjectListBase = Context[classId].GenObjectListBase,
                             Reference = Context[classId].Reference,
                             RefClassId = Context[classId].RefClassId,
                             ReferenceData = Context[classId].ReferenceData ?? this
                         };
            return sc;
        }
    }
}
