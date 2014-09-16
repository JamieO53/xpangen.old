// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;
using Function = org.xpangen.Generator.Profile.Profile.Function;

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
            : base(genData, writer, genObject, fragment)
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
            : base(genData, genWriter, genObject, fragment)
        {
        }

        private GenDataId Var1
        {
            get
            {
                if (!string.IsNullOrEmpty(_var1.ClassName)) return _var1;
                _var1.ClassName = Lookup.Class1;
                _var1.PropertyName = Lookup.Property1;
                _var1.ClassId = GenData.GenDataDef.GetClassId(_var1.ClassName);
                _var1.PropertyId = GenData.GenDataDef.GetClassProperties(_var1.ClassId).IndexOf(_var1.PropertyName);
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
                _var2.ClassId = GenData.GenDataDef.GetClassId(_var2.ClassName);
                _var2.PropertyId = GenData.GenDataDef.GetClassProperties(_var2.ClassId).IndexOf(_var2.PropertyName);
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
                return (notFound || GenObject.SearchFor(Var1, value2) == null) && base.GenerateSecondary();
            
            if (notFound) return false;
            OverrideGenObject = GenObject.SearchFor(Var1, value2);
            return OverrideGenObject != null && base.Generate();
        }

        private bool NoMatch { get { return Lookup.SecondaryBody().FragmentList.Count > 0; } }
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
            SubClassBase = GetSubClassBase();
            Index = -1;
        }

        private int Index { get; set; }

        private ISubClassBase SubClassBase { get; set; }

        private ISubClassBase GetSubClassBase()
        {
            ISubClassBase subClassBase;
            if (GenDataDef.GetClassIsReference(ClassId) &&
                !GenDataDef.GetClassParent(ClassId).IsReference)
            {
                //return GenObject.GetSubClass(ClassName);
                GenData.Cache.Check(GenDataDef.GetClassDef(ClassId).ReferenceDefinition,
                    GenObject.SubClass[IndexOfSubClass()].Reference);
                var f = GenData.Cache[GenObject.SubClass[IndexOfSubClass()].Reference];
                subClassBase = f.Root.SubClass[0];
                foreach (var o in subClassBase)
                    o.RefParent = GenObject;
            }
            else if (GenObject.Definition.IsInherited || GenObject.ClassName == ClassName)//(SubClassIsInheritor())
            {
                //subClassBase = GenObject.GetSubClass(ClassName);
                int classRootId;
                int subClassRootId;
                GetRootClassIds(out subClassRootId, out classRootId);
                if (classRootId != subClassRootId)
                    return GenObject.GetSubClass(ClassName);
                var rootSubClassBase = GenObject.ParentSubClass;
                Assert(0 <= classRootId & classRootId < GenDataDef.Classes.Count, "Root ClassId invalid");
                var parentRootSubClasses = GenDataDef.GetClassParent(classRootId).SubClasses;
                var idx = parentRootSubClasses.IndexOf(classRootId);
                Assert(0 <= idx & idx < parentRootSubClasses.Count, "SubClass index out of range");
                subClassBase = new GenSubClass(GenData.GenDataBase, GenObject, ClassId, parentRootSubClasses[idx]);
                foreach (var o in rootSubClassBase)
                {
                    if (GenObjectClassId(o) == ClassId)
                        subClassBase.Add(o);
                }
            }
            else
                return GenObject.GetSubClass(ClassName);
            return subClassBase;
        }

        private int SegmentClassId
        {
            get {return GenData.GenDataDef.GetClassId(ClassName);}
        }

        private bool SubClassIsInheritor()
        {
            int classId;
            int subClassId;
            GetRootClassIds(out subClassId, out classId);
            return subClassId == classId;
        }

        private GenObject GenObject { get; set; }

        private int IndexOfSubClass()
        {
            var idx = GenObject.Definition.IndexOfSubClass(ClassName);
            Assert(idx != -1, "SubClass not found");
            return idx;
        }

        private void GetRootClassIds(out int subClassId, out int classId)
        {
            classId = GenObjectClassId(GenObject);
            subClassId = ClassId;
            while (GenData.GenDataDef.GetClassIsInherited(subClassId) &&
                   GenData.GenDataDef.GetClassSubClasses(classId).IndexOf(subClassId) == -1)
                subClassId = GenData.GenDataDef.GetClassParent(subClassId).ClassId;
            while (GenData.GenDataDef.GetClassIsInherited(classId) &&
                   GenData.GenDataDef.GetClassSubClasses(classId).IndexOf(subClassId) == -1)
                classId = GenData.GenDataDef.GetClassParent(classId).ClassId;
            Assert(subClassId != -1, "Invalid root subclass");
            Assert(classId == subClassId || GenData.GenDataDef.GetClassSubClasses(classId).IndexOf(subClassId) != -1,
                "Subclass is not a subclass of the class");
        }

        private int GenObjectClassId(GenObject genObject)
        {
            return GenData.GenDataDef.GetClassId(genObject.Definition.Name);
        }

        public string Reference
        {
            get
            {
                if (GenDataDef.GetClassDef(ClassName).IsReference)
                    return GenObject.SubClass[IndexOfSubClass()].Reference;
                return "";
            }
        }

        public string ClassName
        { get { return _generator.Segment.Class; }}

        public GenObject GetGenObject()
        {
            if (Eol()) return null;
            if (!Inheritance) return SubClassBase[Index];
            if (ClassName == SubClassBase[Index].ClassName) return SubClassBase[Index];
            return null;
        }

        private bool Inheritance { get; set; }

        public void SetInheritance()
        {
            Inheritance = true;
            Index = SubClassBase.IndexOf(GenObject);
        }

        private GenData GenData
        {
            get { return _generator.GenData; }
        }

        public void First()
        {
            Index = 0;
            if (ClassDef.IsAbstract || !ClassDef.IsInherited) return;
            while (!Eol() && ClassName != SubClassBase[Index].ClassName) Index++;
        }

        public void Next()
        {
            if (!Eol()) Index++;
            if (ClassDef.IsAbstract || !ClassDef.IsInherited) return;
            while (!Eol() && ClassName != SubClassBase[Index].ClassName) Index++;
        }

        public void Prior()
        {
            if (!Eol()) Index--;
            if (ClassDef.IsAbstract || !ClassDef.IsInherited) return;
            while (!Eol() && ClassName != SubClassBase[Index].ClassName) Index--;
        }

        public void Last()
        {
            Index = SubClassBase.Count - 1;
            if (ClassDef.IsAbstract || !ClassDef.IsInherited) return;
            while (!Eol() && ClassName != SubClassBase[Index].ClassName) Index--;
        }

        public bool Eol()
        {
            if (Index < 0 || Index >= SubClassBase.Count) return true;
            if (ClassDef.IsAbstract || !ClassDef.IsInherited) return false;
            if (Inheritance && ClassName != SubClassBase[Index].ClassName) return false;
            return false;
        }

        private GenDataDefClass ClassDef
        {
            get { return GenDataDef.GetClassDef(ClassId); }
        }

        private GenDataDef GenDataDef
        {
            get { return GenData.GenDataDef; }
        }

        private int ClassId
        {
            get { return SegmentClassId; }
        }
    }

    public class GenSegmentGenerator : GenContainerGenerator
    {
        public GenSegmentGenerator(GenData genData, GenWriter genWriter, GenObject genObject, Fragment fragment) 
            : base(genData, genWriter, genObject, fragment)
        {
            Segment = (Segment) fragment;
            GenCardinality cardinality;
            Assert(Enum.TryParse(Segment.Cardinality, out cardinality), "Invalid segment cardinality: " + Segment.Cardinality);
            GenCardinality = cardinality;
            Navigator = new SegmentNavigator(this);
        }

        private GenCardinality GenCardinality { get; set; }
        public Segment Segment { get; private set; }

        private SegmentNavigator Navigator { get; set; }

        protected override bool Generate()
        {
            var generated = false;
            string sepText;

            switch (GenCardinality)
            {
                case GenCardinality.All:
                    Navigator.First();
                    while (!Navigator.Eol())
                    {
                        OverrideGenObject = Navigator.GetGenObject();
                        generated |= base.Generate();
                        Navigator.Next();
                    }
                    break;
                case GenCardinality.AllDlm:
                    sepText = GetSeparatorText();
                    Navigator.First();
                    while (!Navigator.Eol())
                    {
                        OverrideGenObject = Navigator.GetGenObject();
                        generated |= base.Generate();
                        if (generated) Writer.ProvisionalWrite(sepText);
                        Navigator.Next();
                    }
                    Writer.ClearProvisionalText();
                    break;
                case GenCardinality.Back:
                    Navigator.Last();
                    while (!Navigator.Eol())
                    {
                        OverrideGenObject = Navigator.GetGenObject();
                        generated |= base.Generate();
                        Navigator.Prior();
                    }
                    break;
                case GenCardinality.BackDlm:
                    sepText = GetSeparatorText();
                    Navigator.Last();
                    while (!Navigator.Eol())
                    {
                        OverrideGenObject = Navigator.GetGenObject();
                        generated |= base.Generate();
                        if (generated) Writer.ProvisionalWrite(sepText);
                        Navigator.Prior();
                    }
                    Writer.ClearProvisionalText();
                    break;
                case GenCardinality.First:
                    Navigator.First();
                    OverrideGenObject = Navigator.GetGenObject();
                    if (!Navigator.Eol())
                        generated |= base.Generate();
                    break;
                case GenCardinality.Tail:
                    Navigator.First();
                    Navigator.Next();
                    while (!Navigator.Eol())
                    {
                        OverrideGenObject = Navigator.GetGenObject();
                        generated |= base.Generate();
                        Navigator.Next();
                    }
                    break;
                case GenCardinality.Last:
                    Navigator.Last();
                    OverrideGenObject = Navigator.GetGenObject();
                    if (!Navigator.Eol())
                        generated |= base.Generate();
                    break;
                case GenCardinality.Trunk:
                    Navigator.Last();
                    Navigator.Prior();
                    while (!Navigator.Eol())
                    {
                        OverrideGenObject = Navigator.GetGenObject();
                        generated |= base.Generate();
                        Navigator.Prior();
                    }
                    break;
                case GenCardinality.Reference:
                    Writer.Write(Navigator.ClassName);
                    Writer.Write("[Reference='");
                    Writer.Write(Navigator.Reference);
                    Writer.Write("']\r\n");
                    break;
                case GenCardinality.Inheritance:
                    Navigator.SetInheritance();
                    OverrideGenObject = Navigator.GetGenObject();
                    if (!Navigator.Eol())
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