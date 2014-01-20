// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    public class GenData
    {
        public GenContext Context { get; private set; }
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
            Context = new GenContext();
            for (var i = 0; i < GenDataDef.Classes.Count; i++)
                Context.Add(null, GenDataDef.Classes[i], GenDataBase);

            Context[0].GenObjectListBase.Add(GenDataBase.Root);
            //Context[0] =
            //    new GenObjectList(new GenObjectListBase(GenDataBase, null, 0,
            //                                            new GenDataDefSubClass {SubClass = GenDataDef.Classes[0]})
            //                          {GenDataBase.Root});
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
            for (var i = 0; i < GenDataDef.Classes[classId].SubClasses.Count; i++)
            {
                var subClassId = GenDataDef.Classes[classId].SubClasses[i].SubClass.ClassId;
                if (Context[classId].GenObject == null)
                    First(classId);
                var genObject = Context[classId].GenObject;
                if (genObject != null)
                    if (Context[subClassId] == null)
                        Context[subClassId] = new GenObjectList(genObject.SubClass[i]);
                    else if (GenDataDef.Classes[classId].SubClasses[i].Reference != "")
                        ExpandReferences();
                    else
                    {
                        Context[subClassId].GenObjectListBase = genObject.SubClass[i];
                        Context[subClassId].First();
                    }
                Context[subClassId].First();
            }
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
            d.Context.Duplicate(Context);

            return d;
        }

        /// <summary>
        /// Expand subclass references
        /// </summary>
        public void ExpandReferences()
        {
            bool goOn;
            do
            {
                goOn = false;
                var n = Context.Count;
                for (var i = 0; i < n; i++)
                {
                    if (Eol(i)) continue;
                    for (var j = 0; j < Context[i].GenObject.SubClass.Count; j++)
                    {
                        var subclass = Context[i].GenObject.SubClass[j];
                        if (subclass is GenObjectListReference)
                        {
                            var offset = Context.Count - 2;
                            var refClassId = subclass.ClassId;
                            var defName = subclass.Definition.Reference;
                            var dataName = (subclass as GenObjectListReference).Reference;
                            var data = Cache[defName, dataName];
                            Context[refClassId] = new GenObjectList(data.Context[1].GenObjectListBase)
                                                      {
                                                          ClassIdOffset = refClassId - 1
                                                      };
                            for (var k = 2; k < data.GenDataDef.Classes.Count; k++)
                            {
                                Context.Add(new GenObjectList(data.Context[2].GenObjectListBase)
                                                {
                                                    ClassIdOffset = offset
                                                });
                                Context[Context.Count - 1].First();
                                if (goOn || Eol(Context.Count - 1) ||
                                    Context[Context.Count - 1].GenObject.SubClass.Count == 0) continue;
                                for (var l = 0; l < Context[Context.Count - 1].GenObject.SubClass.Count; l++)
                                {
                                    if (!(Context[Context.Count - 1].GenObject.SubClass[l] is GenObjectListReference))
                                        continue;
                                    goOn = true;
                                    break;
                                }
                            }
                        }
                        else
                            Context[j].First();
                    }
                }
            } while (goOn);
        }
    }
}
