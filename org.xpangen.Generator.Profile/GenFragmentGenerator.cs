// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFragmentGenerator : GenBase
    {
        private readonly GenData _genData;
        private readonly GenWriter _writer;
        private readonly Fragment _fragment;

        protected GenFragmentGenerator(GenData genData, GenWriter writer, Fragment fragment, GenFragment genFragment, GenObject genObject)
        {
            _genData = genData;
            _writer = writer;
            _fragment = fragment;
            GenFragment = genFragment;
            GenObject = genObject;
        }

        protected GenFragmentGenerator()
        {
        }

        public GenObject GenObject { get; private set; }

        public GenData GenData
        {
            get { return _genData; }
        }

        protected GenWriter Writer
        {
            get { return _writer; }
        }

        protected internal GenFragment GenFragment { get; protected set; }

        protected Fragment Fragment
        {
            get { return _fragment; }
        }

        protected virtual bool Generate()
        {
            GenFragment genFragment = GenFragment;
            var expanded = GenFragmentExpander.Expand(genFragment, GenData, genFragment.GenObject, genFragment.Fragment);
            Writer.Write(expanded);
            return expanded != "";
        }

        private static GenFragmentGenerator Create(GenFragment genFragment, GenData genData, GenWriter genWriter, GenObject genObject, Fragment fragment)
        {
            if (genFragment.FragmentType != FragmentType.Null && genFragment.FragmentType != FragmentType.Text)
                if (genFragment.FragmentType != FragmentType.Lookup || !((GenLookup)genFragment).NoMatch)
                    Assert(genObject != null, "The genObject must be set");
            FragmentType fragmentType;
            Enum.TryParse(fragment.GetType().Name, out fragmentType);
            switch (fragmentType)
            {
                case FragmentType.Profile:
                    return new GenContainerGenerator(genFragment, genData, genWriter, genObject);
                case FragmentType.Segment:
                    return new GenSegmentGenerator(genFragment, genData, genWriter, genObject);
                case FragmentType.Block:
                    return new GenContainerGenerator(genFragment, genData, genWriter, genObject);
                case FragmentType.Lookup:
                    return new GenLookupGenerator(genFragment, genData, genWriter, genObject);
                case FragmentType.Condition:
                    return new GenConditionGenerator(genFragment, genData, genWriter, genObject);
                case FragmentType.Function:
                    return new GenFunctionGenerator(genFragment, genData, genWriter, genObject);
                case FragmentType.TextBlock:
                    return new GenContainerGenerator(genFragment, genData, genWriter, genObject);
                default:
                    return new GenFragmentGenerator(genData, genWriter, fragment, genFragment, genObject);
            }
        }

        public static bool Generate(GenFragment genFragment, GenData genData, GenWriter genWriter, GenObject genObject, Fragment fragment)
        {
            return Create(genFragment, genData, genWriter, genObject, fragment).Generate();
        }
    }

    public class GenConditionGenerator : GenContainerGenerator
    {
        internal GenConditionGenerator(GenFragment genFragment, GenData genData, GenWriter writer, GenObject genObject) 
            : base(genFragment, genData, writer, genObject)
        {
        }

        private GenDataId Var1
        {
            get
            {
                return new GenDataId {ClassName = Condition.Class1, PropertyName = Condition.Property1};
            }
        }

        private GenDataId Var2
        {
            get
            {
                return new GenDataId {ClassName = Condition.Class2, PropertyName = Condition.Property2};
            }
        }

        private string Lit { get { return Condition.Lit; }
        }

        private GenComparison GenComparison
        {
            get
            {
                GenComparison c;
                Enum.TryParse(Condition.Comparison, out c);
                return c;
            }
        }

        private bool UseLit
        {
            get { return Condition.UseLit != ""; }
        }

        private bool Test()
        {
            string s1;
            string s2;
            GetOperands(out s1, out s2);

            var i = String.Compare(s1, s2, StringComparison.Ordinal);

            switch (GenComparison)
            {
                case GenComparison.Exists:
                    return i != 0;
                case GenComparison.NotExists:
                    return i == 0;
                case GenComparison.Eq:
                    return i == 0;
                case GenComparison.Ne:
                    return i != 0;
                case GenComparison.Lt:
                    return i < 0;
                case GenComparison.Le:
                    return i <= 0;
                case GenComparison.Gt:
                    return i > 0;
                case GenComparison.Ge:
                    return i >= 0;
                default:
                    throw new Exception("<<<<Invalid condition comparison type>>>>");
            }
        }

        private void GetOperands(out string value1, out string value2)
        {
            value1 = GenObject.GetValue(Var1);
            var comparison = GenComparison;
            value2 = UseLit
                         ? Lit
                         : (comparison == GenComparison.Exists || comparison == GenComparison.NotExists
                                ? ""
                                : GenObject.GetValue(Var2));

            if (comparison != GenComparison.Exists && comparison != GenComparison.NotExists &&
                value1.Length != value2.Length &&
                value1 != "" && value2 != "" &&
                GenUtilities.IsNumeric(value1) && GenUtilities.IsNumeric(value2))
                GenUtilities.PadShortNumericOperand(ref value1, ref value2);
        }

        private Condition Condition
        {
            get { return (Condition)Fragment; }
        }

        protected override bool Generate()
        {
            var cond = (GenCondition) GenFragment;

            cond.Body.GenObject = cond.GenObject;
            if (Test()) return base.Generate();
            return false;
        }
    }

    public class GenContainerGenerator : GenFragmentGenerator
    {
        public GenContainerGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter, GenObject genObject) 
            : base(genData, genWriter, genFragment.Fragment, genFragment, genObject)
        {
            ContainerFragment = (ContainerFragment) Fragment;
        }

        protected GenObject OverrideGenObject { get; set; }
        protected ContainerFragment ContainerFragment { get; set; }
        
        protected override bool Generate()
        {
            var generated = false;
            var container = (GenContainerFragmentBase) GenFragment;
            var genObject = OverrideGenObject ?? container.GenObject;
            foreach (var fragment in container.Body.Fragment)
            {
                fragment.GenObject = genObject;
                generated |= Generate(fragment, GenData, Writer, fragment.GenObject, fragment.Fragment);
            }
            return generated;
        }
    }

    public class GenLookupGenerator : GenContainerGenerator
    {
        private GenDataId _var1;
        private GenDataId _var2;

        public GenLookupGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter, GenObject genObject) 
            : base(genFragment, genData, genWriter, genObject)
        {
        }

        private GenDataId Var1
        {
            get
            {
                if (!string.IsNullOrEmpty(_var1.ClassName)) return _var1;
                _var1.ClassName = Lookup.Class1;
                _var1.PropertyName = Lookup.Property1;
                _var1.ClassId = GenData.GenDataDef.Classes.IndexOf(_var1.ClassName);
                _var1.PropertyId = GenData.GenDataDef.Classes[_var1.ClassId].Properties.IndexOf(_var1.PropertyName);
                return _var1;
            }
        }

        private GenDataId Var2
        {
            get
            {
                if (!string.IsNullOrEmpty(_var2.ClassName)) return _var2;
                _var2.ClassName = Lookup.Class2;
                _var2.PropertyName = Lookup.Property2;
                _var2.ClassId = GenData.GenDataDef.Classes.IndexOf(_var2.ClassName);
                _var2.PropertyId = GenData.GenDataDef.Classes[_var2.ClassId].Properties.IndexOf(_var2.PropertyName);
                return _var2;
            }
        }

        private Lookup Lookup { get { return (Lookup) Fragment; }}
        protected override bool Generate()
        {
            var notFound = GenObject == null;
            var value2 = "";
            if (!notFound)
                value2 = GenObject.GetValue(Var2, out notFound);
            
            if (NoMatch)
                return (notFound || SearchFor(Var1, value2) == null) && base.Generate();
            
            if (notFound) return false;
            OverrideGenObject = SearchFor(Var1, value2);
            return OverrideGenObject != null && base.Generate();
        }

        private bool NoMatch { get { return Lookup.NoMatch != ""; } }

        private GenObject SearchFor(GenDataId id, string value)
        {
            var searchObjects = FindSearchObjects(GenObject, id.ClassName);
            if (searchObjects == null) return null;
           foreach (var searchObject in searchObjects)
           {
               bool notFound;
               var s = searchObject.GetValue(id, out notFound);
               if (!notFound && s == value) return searchObject;
           }
            return null;
        }

        private IEnumerable<GenObject> FindSearchObjects(GenObject genObject, string className)
        {
            while (genObject != null)
            {
                var idx = GenData.GenDataDef.Classes[genObject.ClassId].SubClasses.IndexOf(className);
                if (idx != -1) return genObject.SubClass[idx];
                
                genObject = genObject.Parent;
            }
            return null;
        }
    }

    public class GenFunctionGenerator : GenFragmentGenerator
    {
        public GenFunctionGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter, GenObject genObject) 
            : base(genData, genWriter, genFragment.Fragment, genFragment, genObject)
        {
            GenFragment = genFragment;
        }

        protected override bool Generate()
        {
            var fn = (GenFunction) GenFragment;
            fn.Body.GenObject = fn.GenObject;
            if (String.Compare(fn.FunctionName, "File", StringComparison.OrdinalIgnoreCase) == 0 &&
                (Writer.Stream == null || Writer.Stream is FileStream))
                return (Writer.FileName = fn.Body.Expand(GenData)) != "";
            return base.Generate();
        }
    }

    public class SegmentNavigator : GenBase
    {
        private readonly GenSegmentGenerator _generator;

        public SegmentNavigator(GenSegmentGenerator generator)
        {
            GenSegment = (GenSegment) generator.GenFragment;
            _generator = generator;
            GenObject = GenSegment.GenObject;
            var objects = GenObjectList.SubClassBase;
            var subClassBase = GetSubClassBase();
        }

        public ISubClassBase GetSubClassBase()
        {
            ISubClassBase subClassBase;
            if (GenData.GenDataDef.Classes[ClassId].IsReference &&
                !GenData.GenDataDef.Classes[ClassId].Parent.IsReference)
            {
                var f = GenData.Cache[GenData.GenDataDef.Classes[ClassId].ReferenceDefinition,
                    GenObject.SubClass[IndexOfSubClass()].Reference];
                subClassBase = f.Root.SubClass[0];
            }
            else if (SubClassIsInheritor())
            {
                int classRootId;
                int subClassRootId;
                GetRootClassIds(out subClassRootId, out classRootId);
                ISubClassBase rootSubClassBase;
                if (classRootId != subClassRootId)
                {
                    rootSubClassBase = GenObject.SubClass[GenObject.Definition.SubClasses.IndexOf(subClassRootId)];
                    subClassBase = new GenSubClass(GenData.GenDataBase, GenObject, ClassId,
                        GenData.GenDataDef.Classes[classRootId].SubClasses[
                            GenData.GenDataDef.Classes[classRootId].SubClasses.IndexOf(subClassRootId)]);
                }
                else
                {
                    rootSubClassBase = GenObject.ParentSubClass;
                    Assert(0 <= classRootId & classRootId < GenData.GenDataDef.Classes.Count, "Root ClassId invalid");
                    var idx = GenData.GenDataDef.Classes[classRootId].Parent.SubClasses.IndexOf(classRootId);
                    Assert(0 <= idx & idx < GenData.GenDataDef.Classes[classRootId].Parent.SubClasses.Count,
                        "SubClass index out of range");
                    subClassBase = new GenSubClass(GenData.GenDataBase, GenObject, ClassId,
                        GenData.GenDataDef.Classes[classRootId].Parent.SubClasses[idx]);
                }
                foreach (var o in rootSubClassBase)
                {
                    if (GenObjectClassId(o) == ClassId)
                        subClassBase.Add(o);
                }
            }
            else
            {
                var classIdx = IndexOfSubClass();
                subClassBase = GenObject.SubClass[classIdx];
            }
            return subClassBase;
        }

        private int SegmentClassId()
        {
            return GenSegment.ClassId;
        }

        private bool SubClassIsInheritor()
        {
            int classId;
            int subClassId;
            GetRootClassIds(out subClassId, out classId);
            return subClassId == classId;
        }

        public GenObject GenObject { get; private set; }

        private int IndexOfSubClass()
        {
            int classId;
            int subClassId;
            GetRootClassIds(out subClassId, out classId);
            var idx = GenData.GenDataDef.Classes[classId].SubClasses.IndexOf(subClassId);
            Assert(idx != -1, "SubClass not found");
            return idx;
        }

        private void GetRootClassIds(out int subClassId, out int classId)
        {
            classId = GenObjectClassId(GenObject);
            subClassId = ClassId;
            var classes = GenData.GenDataDef.Classes;
            while (classes[subClassId].IsInherited &&
                   classes[classId].SubClasses.IndexOf(subClassId) == -1)
                subClassId = classes[subClassId].Parent.ClassId;
            while (classes[classId].IsInherited &&
                   classes[classId].SubClasses.IndexOf(subClassId) == -1)
                classId = classes[classId].Parent.ClassId;
            Assert(subClassId != -1, "Invalid root subclass");
            Assert(classId == subClassId || classes[classId].SubClasses.IndexOf(subClassId) != -1,
                "Subclass is not a subclass of the class");
        }

        private int GenObjectClassId(GenObject genObject)
        {
            return GenData.GenDataDef.Classes.IndexOf(genObject.Definition.Name);
        }

        private GenSegment GenSegment { get; set; }

        public string Reference()
        {
            return GenObjectList.Reference;
        }

        private GenObjectList GenObjectList
        {
            get { return GenData.Context[ClassId]; }
        }

        public string ClassName()
        {
            return ClassDef.Name;
        }

        private GenDataDefClass ClassDef
        {
            get { return GenData.GenDataDef.Classes[ClassId]; }
        }

        public GenObject GetGenObject()
        {
            return GenObjectList.GenObject;
        }

        public void SetInheritance()
        {
            GenData.SetInheritance(ClassId);
        }

        private GenData GenData
        {
            get { return _generator.GenData; }
        }

        public void Prior()
        {
            CheckInheritance();
            GenObjectList.Prior();
            CheckInheritance();
            if (!GenObjectList.Eol)
                SetSubClasses();
        }

        private void SetSubClasses()
        {
            int classId = ClassId;
            var inheritedClassId = ClassId;
            if (GenObjectList.DefClass != null && GenData.Context[classId].DefClass.IsInherited)
            {
                classId = GenObjectList.DefClass.Parent.ClassId;
                GenData.Context[classId].Index = GenData.Context[inheritedClassId].Index;
                GenData.Context[classId].ReferenceData = GenData.Context[inheritedClassId].ReferenceData;
            }

            if (GenData.Context[classId].ReferenceData != null && GenData.Context[classId].ReferenceData != GenData)
            {
                GenData.Context[classId].ReferenceData.Context[GenData.Context[classId].RefClassId].Index = GenData.Context[classId].Index;
                GenData.Context[classId].ReferenceData.SetSubClasses(GenData.Context[classId].RefClassId);
            }

            var classDef = GenData.Context.Classes[classId];
            if (GenData.Context[classId].GenObject == null)
                GenData.First(classId);
            var genObject = GenData.Context[classId].GenObject;
            if (classDef != null && genObject != null)
            {
                for (var i = 0; i < classDef.SubClasses.Count; i++)
                {
                    var subClass = classDef.SubClasses[i].SubClass;
                    var subClassId = subClass.ClassId;
                    if (!string.IsNullOrEmpty(classDef.SubClasses[i].Reference) &&
                        classDef.Reference != classDef.SubClasses[i].Reference)
                    {
                        if (genObject.SubClass[i].Reference != null)
                            SetReferenceSubClasses(classId, i, subClassId, genObject);
                    }
                    else if (GenData.Context[subClassId].ReferenceData != null && GenData.Context[subClassId].ReferenceData != GenData)
                    {
                        var refContext = GenData.Context[subClassId].ReferenceData.Context[GenData.Context[subClassId].RefClassId];
                        GenData.Context[subClassId].SubClassBase = refContext.SubClassBase;
                        GenData.First(subClassId);
                    }
                    else
                    {
                        Assert(i < genObject.SubClass.Count, "The object does not have a subclass to set");
                        GenData.Context[subClassId].SubClassBase = genObject.SubClass[i];
                        GenData.First(subClassId);
                    }
                }
            }
        }

        private void SetReferenceSubClasses(int classId, int i, int subClassId, GenObject genObject)
        {
            var data = GenData.Cache[GenData.Context.Classes[subClassId].ReferenceDefinition,
                genObject.SubClass[i].Reference];
            var reference = genObject.SubClass[i].Reference;
            var subClass = GenData.Context.Classes[classId].SubClasses[i].SubClass;
            var subClassId1 = subClass.ClassId;
            var subRefClassId = subClass.RefClassId;
            GenData.Context[subClassId1].ClassId = subClass.ClassId;
            GenData.Context[subClassId1].RefClassId = subRefClassId;
            GenData.Context[subClassId1].Reference = reference;
            GenData.Context[subClassId1].ReferenceData = data;
            GenData.Context[subClassId1].SubClassBase = data.Context[subRefClassId].SubClassBase;
            data.First(subRefClassId);
            GenData.Context[subClassId1].First();
            if (!data.Eol(subRefClassId))
                for (var i1 = 0; i1 < GenData.Context.Classes[subClassId1].SubClasses.Count; i1++)
                    GenData.SetReferenceSubClasses(subClassId1, i1, data, reference);
        }

        private void CheckInheritance()
        {
            if (GenData.GenDataDef == null || !ClassDef.IsInherited) return;
            if (GenObjectList.SubClassBase == ParentGenObjectList.SubClassBase) return;
            GenObjectList.SubClassBase = ParentGenObjectList.SubClassBase;
            GenObjectList.Index = ParentGenObjectList.Index;
            GenObjectList.ReferenceData = ParentGenObjectList.ReferenceData;
        }

        private GenObjectList ParentGenObjectList
        {
            get
            {
                var superClassId = ClassDef.Parent.ClassId;
                var genObjectList = GenData.Context[superClassId];
                return genObjectList;
            }
        }

        public void Last()
        {
            CheckInheritance();
            GenObjectList.Last();
            CheckInheritance();
            if (!GenObjectList.Eol)
                SetSubClasses();
        }

        public void Next()
        {
            CheckInheritance();
            GenObjectList.Next();
            CheckInheritance();
            if (!GenObjectList.Eol)
                SetSubClasses();
        }

        public bool Eol()
        {
            CheckInheritance();
            return GenObjectList.Eol;
        }

        public void First()
        {
            CheckInheritance();
            GenObjectList.First();
            if (!GenData.Eol(ClassId))
                SetSubClasses();
        }

        private int ClassId
        {
            get { return SegmentClassId(); }
        }
    }

    public class GenSegmentGenerator : GenContainerGenerator
    {
        private readonly SegmentNavigator _navigator;

        public GenSegmentGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter, GenObject genObject) 
            : base(genFragment, genData, genWriter, genObject)
        {
            GenFragment = genFragment;
            GenSegment =(GenSegment) GenFragment;
            _navigator = new SegmentNavigator(this);
        }

        private GenSegment GenSegment { get; set; }
        private GenBlock ItemBody { get; set; }
        private GenFragment Separator { get; set; }

        protected override bool Generate()
        {
            var generated = false;
            var genSegment = (GenSegment) GenFragment;
            string sepText;

            switch (genSegment.GenCardinality)
            {
                case GenCardinality.All:
                    _navigator.First();
                    while (!_navigator.Eol())
                    {
                        OverrideGenObject = _navigator.GetGenObject();
                        generated |= base.Generate();
                        _navigator.Next();
                    }
                    break;
                case GenCardinality.AllDlm:
                    CheckDelimiter();
                    sepText = GenFragmentExpander.Expand(Separator, GenData, Separator.GenObject, Separator.Fragment);
                    _navigator.First();
                    while (!_navigator.Eol())
                    {
                        ItemBody.GenObject = _navigator.GetGenObject();
                        generated |= Generate(ItemBody, GenData, Writer, ((GenFragment) ItemBody).GenObject, ItemBody.Fragment);
                        if (generated) Writer.ProvisionalWrite(sepText);
                        _navigator.Next();
                    }
                    Writer.ClearProvisionalText();
                    break;
                case GenCardinality.Back:
                    _navigator.Last();
                    while (!_navigator.Eol())
                    {
                        OverrideGenObject = _navigator.GetGenObject();
                        generated |= base.Generate();
                        _navigator.Prior();
                    }
                    break;
                case GenCardinality.BackDlm:
                    CheckDelimiter();
                    GenFragment genFragment2 = Separator;
                    sepText = GenFragmentExpander.Expand(genFragment2, GenData, genFragment2.GenObject, genFragment2.Fragment);
                    _navigator.Last();
                    while (!_navigator.Eol())
                    {
                        ItemBody.GenObject = _navigator.GetGenObject();
                        GenFragment genFragment = ItemBody;
                        generated |= Generate(genFragment, GenData, Writer, genFragment.GenObject, genFragment.Fragment);
                        if (generated) Writer.ProvisionalWrite(sepText);
                        _navigator.Prior();
                    }
                    Writer.ClearProvisionalText();
                    break;
                case GenCardinality.First:
                    _navigator.First();
                    OverrideGenObject = _navigator.GetGenObject();
                    if (!_navigator.Eol())
                        generated |= base.Generate();
                    break;
                case GenCardinality.Tail:
                    _navigator.First();
                    _navigator.Next();
                    while (!_navigator.Eol())
                    {
                        OverrideGenObject = _navigator.GetGenObject();
                        generated |= base.Generate();
                        _navigator.Next();
                    }
                    break;
                case GenCardinality.Last:
                    _navigator.Last();
                    OverrideGenObject = _navigator.GetGenObject();
                    if (!_navigator.Eol())
                        generated |= base.Generate();
                    break;
                case GenCardinality.Trunk:
                    _navigator.Last();
                    _navigator.Prior();
                    while (!_navigator.Eol())
                    {
                        OverrideGenObject = _navigator.GetGenObject();
                        generated |= base.Generate();
                        _navigator.Prior();
                    }
                    break;
                case GenCardinality.Reference:
                    Writer.Write(_navigator.ClassName());
                    Writer.Write("[Reference='");
                    Writer.Write(_navigator.Reference());
                    Writer.Write("']\r\n");
                    break;
                case GenCardinality.Inheritance:
                    _navigator.SetInheritance();
                    OverrideGenObject = _navigator.GetGenObject();
                    if (!_navigator.Eol())
                        generated |= base.Generate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return generated;
        }

        public void CheckDelimiter()
        {
            if (Separator != null) return;

            // Optimization: This is done once when this method is first called
            ItemBody = new GenBlock(new GenFragmentParams(GenSegment.GenDataDef, GenSegment, GenSegment.ParentContainer));

            for (var i = 0; i < GenSegment.Body.Count - 1; i++)
                ItemBody.Body.Add(GenSegment.Body.Fragment[i]);

            var last = GenSegment.Body.Count > 0 ? GenSegment.Body.Fragment[GenSegment.Body.Count - 1] : null;
            var lastText = last as GenTextBlock;
            if (last is GenTextBlock && lastText.Body.Count > 1)
            {
                var newText = new GenTextBlock(new GenFragmentParams(GenSegment.GenDataDef, GenSegment, GenSegment));
                for (var i = 0; i < lastText.Body.Count - 1; i++)
                    newText.Body.Add(lastText.Body.Fragment[i]);
                ItemBody.Body.Add(newText);
                Separator = lastText.Body.Fragment[lastText.Body.Count - 1];
                var body = ContainerFragment.Body().FragmentList;
                var dlm = ContainerFragment.SecondaryBody().FragmentList;
                dlm.Add(body[body.Count-1]);
                body.RemoveAt(body.Count - 1);
            }
            else
                Separator = GenSegment.Body.Count > 0
                    ? last
                    : GenFragment.NullFragment;
            if (Separator != null) Separator.GenObject = GenObject;
            else Separator = GenFragment.NullFragment;
        }
    }
}