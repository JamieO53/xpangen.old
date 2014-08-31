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

        protected GenFragmentGenerator(GenData genData, GenWriter writer, Fragment fragment, GenObject genObject)
        {
            _genData = genData;
            _writer = writer;
            _fragment = fragment;
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

        protected Fragment Fragment
        {
            get { return _fragment; }
        }

        protected virtual bool Generate()
        {
            var expanded = GenFragmentExpander.Expand(GenData, GenObject, Fragment);
            Writer.Write(expanded);
            return expanded != "";
        }

        private static GenFragmentGenerator Create(GenData genData, GenWriter genWriter, GenObject genObject, Fragment fragment)
        {
            FragmentType fragmentType;
            Enum.TryParse(fragment.GetType().Name, out fragmentType);
            switch (fragmentType)
            {
                case FragmentType.Profile:
                    return new GenContainerGenerator(genData, genWriter, genObject, fragment);
                case FragmentType.Segment:
                    return new GenSegmentGenerator(genData, genWriter, genObject, fragment);
                case FragmentType.Block:
                    return new GenContainerGenerator(genData, genWriter, genObject, fragment);
                case FragmentType.Lookup:
                    return new GenLookupGenerator(genData, genWriter, genObject, fragment);
                case FragmentType.Condition:
                    return new GenConditionGenerator(genData, genWriter, genObject, fragment);
                case FragmentType.Function:
                    return new GenFunctionGenerator(genData, genWriter, genObject, fragment);
                case FragmentType.TextBlock:
                    return new GenContainerGenerator(genData, genWriter, genObject, fragment);
                default:
                    return new GenFragmentGenerator(genData, genWriter, fragment, genObject);
            }
        }

        public static bool Generate(GenData genData, GenWriter genWriter, GenObject genObject, Fragment fragment)
        {
            return Create(genData, genWriter, genObject, fragment).Generate();
        }

        public static bool GenerateSecondary(GenData genData, GenWriter genWriter, GenObject genObject, Fragment fragment)
        {
            return Create(genData, genWriter, genObject, fragment).GenerateSecondary();
        }

        protected virtual bool GenerateSecondary()
        {
            var expanded = GenFragmentExpander.ExpandSecondary(GenData, GenObject, Fragment);
            Writer.Write(expanded);
            return expanded != "";
        }
    }

    public class GenConditionGenerator : GenContainerGenerator
    {
        internal GenConditionGenerator(GenData genData, GenWriter writer, GenObject genObject, Fragment fragment) 
            : base(genData, writer, genObject, fragment: fragment)
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
            return Test() && base.Generate();
        }
    }

    public class GenContainerGenerator : GenFragmentGenerator
    {
        public GenContainerGenerator(GenData genData, GenWriter genWriter, GenObject genObject, Fragment fragment) 
            : base(genData, genWriter, fragment, genObject)
        {
            ContainerFragment = (ContainerFragment) Fragment;
        }

        protected GenObject OverrideGenObject { get; set; }
        private ContainerFragment ContainerFragment { get; set; }
        
        protected override bool Generate()
        {
            var generated = false;
            var genObject = OverrideGenObject ?? GenObject;
            foreach (var fragment in ContainerFragment.Body().FragmentList)
            {
                generated |= Generate(GenData, Writer, genObject, fragment);
            }
            return generated;
        }

        protected override bool GenerateSecondary()
        {
            var generated = false;
            var genObject = OverrideGenObject ?? GenObject;
            foreach (var fragment in ContainerFragment.SecondaryBody().FragmentList)
            {
                generated |= Generate(GenData, Writer, genObject, fragment);
            }
            return generated;
        }
    }

    public class GenLookupGenerator : GenContainerGenerator
    {
        private GenDataId _var1;
        private GenDataId _var2;

        public GenLookupGenerator(GenData genData, GenWriter genWriter, GenObject genObject, Fragment fragment) 
            : base(genData, genWriter, genObject, fragment: fragment)
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
                return (notFound || SearchFor(Var1, value2) == null) && base.GenerateSecondary();
            
            if (notFound) return false;
            OverrideGenObject = SearchFor(Var1, value2);
            return OverrideGenObject != null && base.Generate();
        }

        private bool NoMatch { get { return Lookup.SecondaryBody().FragmentList.Count > 0; } }

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
        public GenFunctionGenerator(GenData genData, GenWriter genWriter, GenObject genObject, Fragment fragment) 
            : base(genData, genWriter, fragment, genObject)
        {
        }

        protected override bool Generate()
        {
            var fn = (Function) Fragment;
            if (String.Compare(fn.FunctionName, "File", StringComparison.OrdinalIgnoreCase) == 0 &&
                (Writer.Stream == null || Writer.Stream is FileStream))
                return (Writer.FileName = GenFragmentExpander.Expand(GenData, GenObject, fn.Body().FragmentList[0])) != "";
            return base.Generate();
        }
    }

    public class SegmentNavigator : GenBase
    {
        private readonly GenSegmentGenerator _generator;

        public SegmentNavigator(GenSegmentGenerator generator)
        {
            _generator = generator;
            GenObject = generator.GenObject;
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
            return GenData.GenDataDef.Classes.IndexOf(ClassName());
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
            return _generator.Segment.Class;
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

        public GenSegmentGenerator(GenData genData, GenWriter genWriter, GenObject genObject, Fragment fragment) 
            : base(genData, genWriter, genObject, fragment)
        {
            Segment = (Segment) fragment;
            GenCardinality cardinality;
            Assert(Enum.TryParse(Segment.Cardinality, out cardinality), "Invalid segment cardinality: " + Segment.Cardinality);
            GenCardinality = cardinality;
            _navigator = new SegmentNavigator(this);
        }

        public GenCardinality GenCardinality { get; set; }
        public Segment Segment { get; set; }
        protected override bool Generate()
        {
            var generated = false;
            string sepText;

            switch (GenCardinality)
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
                    sepText = GetSeparatorText();
                    _navigator.First();
                    while (!_navigator.Eol())
                    {
                        OverrideGenObject = _navigator.GetGenObject();
                        generated |= base.Generate();
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
                    sepText = GetSeparatorText();
                    _navigator.Last();
                    while (!_navigator.Eol())
                    {
                        OverrideGenObject = _navigator.GetGenObject();
                        generated |= base.Generate();
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

        private string GetSeparatorText()
        {
            var segment = ((Segment) Fragment);
            var sb = segment.SecondaryBody();
            var sepText = "";
            foreach (var f in sb.FragmentList)
            {
                var text = f as Text;
                var textBlock = f as TextBlock;
                if (text != null)
                    sepText += text.TextValue;
                else if (textBlock != null)
                {
                    sepText = "";
                    foreach (var f0 in textBlock.Body().FragmentList)
                    {
                        var text0 = f0 as Text;
                        var placeholder = f0 as Placeholder;
                        if (text0 != null)
                            sepText += text0.TextValue;
                        else if (placeholder != null)
                            sepText +=
                                GenObject.GetValue(GenData.GenDataDef.GetId(placeholder.Class,
                                    placeholder.Property));
                    }
                }
            }
            return sepText;
        }
    }
}