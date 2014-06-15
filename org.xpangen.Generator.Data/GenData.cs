// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    /// <summary>
    /// The container for navigating generator data.
    /// </summary>
    public class GenData
    {
        /// <summary>
        /// The class context for automated navigation of the data.
        /// </summary>
        public GenContext Context { get; private set; }
        
        /// <summary>
        /// The container of the data.
        /// </summary>
        public GenDataBase GenDataBase { get; private set; }
        
        /// <summary>
        /// The definition of the data.
        /// </summary>
        public GenDataDef GenDataDef { get { return GenDataBase.GenDataDef; } }
        
        /// <summary>
        /// The name of the data file.
        /// </summary>
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
        
        /// <summary>
        /// Has the data been changed?
        /// </summary>
        public bool Changed
        {
            get { return GenDataBase.Changed; }
        }

        /// <summary>
        /// The root object of the data, is also accessible as Context[0].
        /// </summary>
        public GenObject Root
        {
            get { return GenDataBase.Root; }
        }

        /// <summary>
        /// The cached data references.
        /// </summary>
        public GenDataReferenceCache Cache { get { return Context.Cache; } }

        /// <summary>
        /// The data loader for reference data.
        /// </summary>
        public static IGenDataLoader DataLoader { get; set; }

        /// <summary>
        /// Create a new <see cref="GenData"/> container for the specified definition.
        /// </summary>
        /// <param name="genDataDef">The data definition.</param>
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
                GenDataDefSubClass defSubClass = null;
                if (parent != null)
                {
                    foreach (var subClass in parent.SubClasses)
                    {
                        if (subClass.SubClass != GenDataDef.Classes[i]) continue;
                        
                        reference = subClass.Reference ?? "";
                        referenceDefinition = subClass.ReferenceDefinition ?? "";
                        defSubClass = subClass;
                        break;
                    }
                }
                Context.Add(null, GenDataDef.Classes[i], GenDataBase, reference, referenceDefinition, defSubClass);
            }

            Context[0].SubClassBase.Add(GenDataBase.Root);
            First(0);
        }
        
        /// <summary>
        /// Create a new object for the named class, and add to the end of the list
        /// of classes belonging to the current parent class specified.
        /// </summary>
        /// <param name="parentClassName">The name of the parent class.</param>
        /// <param name="className">The name of the class object being created.</param>
        /// <returns>The new object.</returns>
        public GenObject CreateObject(string parentClassName, string className)
        {
            var parent = Context[GenDataDef.Classes.IndexOf(parentClassName)].GenObject;
            var o = GenDataBase.CreateGenObject(className, parent);
            Context[o.ClassId].SubClassBase = o.ParentSubClass;
            Context[o.ClassId].Index = Context[o.ClassId].IndexOf(o);
            return o;
        }

        /// <summary>
        /// Establish the first item of the class as the current item for its context, 
        /// then set all of its subclasses to their first classes.
        /// </summary>
        /// <param name="classId">The class whose context is being established.</param>
        public void First(int classId)
        {
            if (!Eol(classId))
                ResetSubClasses(classId);
            Context[classId].First();
            if (!Eol(classId))
                SetSubClasses(classId);
        }

        /// <summary>
        /// Establish the next item after the current item of the class as the current item for its context, 
        /// then set all of its subclasses to their first classes.
        /// </summary>
        /// <param name="classId">The class whose context is being established.</param>
        public void Next(int classId)
        {
            if (!Eol(classId))
                ResetSubClasses(classId);
            Context[classId].Next();
            if (!Eol(classId))
                SetSubClasses(classId);
        }

        /// <summary>
        /// Establish the last item of the class as the current item for its context, 
        /// then set all of its subclasses to their first classes.
        /// </summary>
        /// <param name="classId">The class whose context is being established.</param>
        public void Last(int classId)
        {
            if (!Eol(classId))
                ResetSubClasses(classId);
            Context[classId].Last();
            if (!Eol(classId))
                SetSubClasses(classId);
        }

        /// <summary>
        /// Establish the item before the current item of the class as the current item for its context, 
        /// then set all of its subclasses to their first classes.
        /// </summary>
        /// <param name="classId">The class whose context is being established.</param>
        public void Prior(int classId)
        {
            if (!Eol(classId))
                ResetSubClasses(classId);
            Context[classId].Prior();
            if (!Eol(classId))
                SetSubClasses(classId);
        }

        /// <summary>
        /// Are we at the end of the list of objects of the specified class?
        /// </summary>
        /// <param name="classId">The class being checked.</param>
        /// <returns>The context is at the end of the list.</returns>
        public bool Eol(int classId)
        {
            return Context[classId].Eol;
        }

        /// <summary>
        /// Ensure that no object is current for the specified class.
        /// </summary>
        /// <param name="classId">The class being reset.</param>
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
            if (Context[classId].ReferenceData != null && Context[classId].ReferenceData != this)
            {
                Context[classId].ReferenceData.Context[Context[classId].RefClassId].Index = Context[classId].Index;
                Context[classId].ReferenceData.SetSubClasses(Context[classId].RefClassId);
            }

            var classDef = Context.Classes[classId];
            if (Context[classId].GenObject == null)
                First(classId);
            var genObject = Context[classId].GenObject;
            if (genObject != null)
                for (var i = 0; i < classDef.SubClasses.Count; i++)
                {
                    var subClass = classDef.SubClasses[i].SubClass;
                    var subClassId = subClass.ClassId;
                    if (!string.IsNullOrEmpty(classDef.SubClasses[i].Reference) &&
                        classDef.Reference != classDef.SubClasses[i].Reference)
                    {
                        if (genObject.SubClass[i].Reference != null)
                            SetReferenceSubClasses(classId, i,
                                                   Cache[
                                                       Context.Classes[subClassId].ReferenceDefinition,
                                                       genObject.SubClass[i].Reference],
                                                   genObject.SubClass[i].Reference);
                    }
                    else if (Context[subClassId].ReferenceData != null && Context[subClassId].ReferenceData != this)
                    {
                        var refContext = Context[subClassId].ReferenceData.Context[Context[subClassId].RefClassId];
                        //refContext.Index = Context[subClassId].Index;
                        Context[subClassId].SubClassBase = refContext.SubClassBase;
                        First(subClassId);
                    }
                    else
                    {
                        Context[subClassId].SubClassBase = genObject.SubClass[i];
                        First(subClassId);
                    }
                }
        }

        private void SetReferenceSubClasses(int classId, int subClassIndex, GenData data, string reference)
        {
            var subClass = Context.Classes[classId].SubClasses[subClassIndex].SubClass;
            var subClassId = subClass.ClassId;
            var subRefClassId = subClass.RefClassId;
            Context[subClassId].ClassId = subClass.ClassId;
            Context[subClassId].RefClassId = subRefClassId;
            Context[subClassId].Reference = reference;
            Context[subClassId].ReferenceData = data;
            Context[subClassId].SubClassBase = data.Context[subRefClassId].SubClassBase;
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
        /// Restore the context to that saved before.
        /// </summary>
        /// <param name="savedContext">The context being restored.</param>
        public void EstablishContext(GenSavedContext savedContext)
        {
            if (savedContext.ParentContext != null)
                savedContext.ParentContext.EstablishContext();
            Context[savedContext.ClassId].SubClassBase = savedContext.SubClassBase;
            Context[savedContext.ClassId].RefClassId = savedContext.RefClassId;
            Context[savedContext.ClassId].Reference = savedContext.Reference;
            Context[savedContext.ClassId].ReferenceData = savedContext.ReferenceData;
            Context[savedContext.ClassId].Index = savedContext.SubClassBase.IndexOf(savedContext.GenObject);

            SetSubClasses(savedContext.ClassId);
        }

        /// <summary>
        /// Save the specified context, in the context of the saved parent context.
        /// </summary>
        /// <param name="classId">The class ID being saved.</param>
        /// <param name="parentContext">The saved parent context.</param>
        /// <returns>The saved context.</returns>
        public GenSavedContext SaveContext(int classId, GenSavedContext parentContext)
        {
            var sc = new GenSavedContext
                         {
                             ParentContext = parentContext,
                             GenData = this,
                             ClassId = classId,
                             GenObject = Context[classId].GenObject,
                             SubClassBase = Context[classId].SubClassBase,
                             Reference = Context[classId].Reference,
                             RefClassId = Context[classId].RefClassId,
                             ReferenceData = Context[classId].ReferenceData ?? this
                         };
            return sc;
        }

        /// <summary>
        /// Save the specified context.
        /// </summary>
        /// <param name="classId">The class ID being saved.</param>
        /// <returns>The saved context.</returns>
        public GenSavedContext SaveContext(int classId)
        {
            if (classId == 0)
                return null;
            if (Context[classId].DefClass.Parent.ClassId == 0)
                return SaveContext(classId, null);
            return SaveContext(classId, SaveContext(Context[classId].DefClass.Parent.ClassId));
        }
        
        public override string ToString()
        {
            return !string.IsNullOrEmpty(DataName) ? DataName : base.ToString();
        }

        public void SetInheritance(int classId)
        {
            if (!GenDataDef.Classes[classId].IsInherited)
                throw new GeneratorException("Cannot set inheritance for a non-extension class", GenErrorType.Assertion);
            var superClassId = GenDataDef.Classes[classId].Parent.ClassId;
            if (Context[superClassId].Eol || Context[superClassId].GenObject.ClassId != classId)
            {
                Context[classId].SubClassBase = null;
                Context[classId].Index = -1;
                return;
            }
            Context[classId].SubClassBase = Context[superClassId].SubClassBase;
            Context[classId].Index = Context[superClassId].Index;
        }
    }
}
