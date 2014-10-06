// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Diagnostics.Contracts;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.FunctionLibrary;
using org.xpangen.Generator.Profile.Profile;
using Function = org.xpangen.Generator.Profile.Profile.Function;

namespace org.xpangen.Generator.Profile
{
    public class GenFragmentGenerator
    {
        private readonly GenDataDef _genDataDef;
        private readonly GenWriter _writer;
        private readonly Fragment _fragment;

        protected GenFragmentGenerator(GenDataDef genDataDef, GenWriter writer, Fragment fragment, GenObject genObject)
        {
            _genDataDef = genDataDef;
            _writer = writer;
            _fragment = fragment;
            GenObject = genObject;
        }

        protected GenFragmentGenerator()
        {
        }

        public GenObject GenObject { get; private set; }

        public GenDataDef GenDataDef
        {
            get { return _genDataDef; }
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
            var expanded = GenFragmentExpander.Expand(GenDataDef, GenObject, Fragment);
            Writer.Write(expanded);
            return expanded != "";
        }

        private static GenFragmentGenerator Create(GenDataDef genDataDef, GenWriter genWriter, GenObject genObject, Fragment fragment)
        {
            FragmentType fragmentType;
            Enum.TryParse(fragment.GetType().Name, out fragmentType);
            switch (fragmentType)
            {
                case FragmentType.Profile:
                    return new GenContainerGenerator(genDataDef, genWriter, genObject, fragment);
                case FragmentType.Segment:
                    return new GenSegmentGenerator(genDataDef, genWriter, genObject, fragment);
                case FragmentType.Block:
                    return new GenContainerGenerator(genDataDef, genWriter, genObject, fragment);
                case FragmentType.Lookup:
                    return new GenLookupGenerator(genDataDef, genWriter, genObject, fragment);
                case FragmentType.Condition:
                    return new GenConditionGenerator(genDataDef, genWriter, genObject, fragment);
                case FragmentType.Function:
                    return new GenFunctionGenerator(genDataDef, genWriter, genObject, fragment);
                case FragmentType.TextBlock:
                    return new GenContainerGenerator(genDataDef, genWriter, genObject, fragment);
                case FragmentType.Annotation:
                    return new GenAnnotationGenerator(genDataDef, genWriter, genObject, fragment);
                default:
                    return new GenFragmentGenerator(genDataDef, genWriter, fragment, genObject);
            }
        }

        public static bool Generate(GenDataDef genDataDef, GenWriter genWriter, GenObject genObject, Fragment fragment)
        {
            return Create(genDataDef, genWriter, genObject, fragment).Generate();
        }

        public static bool GenerateSecondary(GenDataDef genDataDef, GenWriter genWriter, GenObject genObject, Fragment fragment)
        {
            return Create(genDataDef, genWriter, genObject, fragment).GenerateSecondary();
        }

        protected virtual bool GenerateSecondary()
        {
            var expanded = GenFragmentExpander.ExpandSecondary(GenDataDef, GenObject, Fragment);
            Writer.Write(expanded);
            return expanded != "";
        }
    }

    public class GenAnnotationGenerator : GenContainerGenerator
    {
        internal GenAnnotationGenerator(GenDataDef genDataDef, GenWriter genWriter, GenObject genObject, Fragment fragment)
            : base(genDataDef, new GenWriter(new NullStream()), genObject, fragment)
        {
        }
    }
    
    public class GenConditionGenerator : GenContainerGenerator
    {
        internal GenConditionGenerator(GenDataDef genDataDef, GenWriter writer, GenObject genObject, Fragment fragment) 
            : base(genDataDef, writer, genObject, fragment)
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

            var i = String.Compare(s1, s2, StringComparison.OrdinalIgnoreCase);

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
        public GenContainerGenerator(GenDataDef genDataDef, GenWriter genWriter, GenObject genObject, Fragment fragment) 
            : base(genDataDef, genWriter, fragment, genObject)
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
                generated |= Generate(GenDataDef, Writer, genObject, fragment);
            }
            return generated;
        }

        protected override bool GenerateSecondary()
        {
            var generated = false;
            var genObject = OverrideGenObject ?? GenObject;
            foreach (var fragment in ContainerFragment.SecondaryBody().FragmentList)
            {
                generated |= Generate(GenDataDef, Writer, genObject, fragment);
            }
            return generated;
        }
    }

    public class GenLookupGenerator : GenContainerGenerator
    {
        private GenDataId _var1;
        private GenDataId _var2;

        public GenLookupGenerator(GenDataDef genDataDef, GenWriter genWriter, GenObject genObject, Fragment fragment) 
            : base(genDataDef, genWriter, genObject, fragment)
        {
        }

        private GenDataId Var1
        {
            get
            {
                if (!string.IsNullOrEmpty(_var1.ClassName)) return _var1;
                _var1.ClassName = Lookup.Class1;
                _var1.PropertyName = Lookup.Property1;
                _var1.ClassId = GenDataDef.GetClassId(_var1.ClassName);
                _var1.PropertyId = GenDataDef.GetClassProperties(_var1.ClassId).IndexOf(_var1.PropertyName);
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
                _var2.ClassId = GenDataDef.GetClassId(_var2.ClassName);
                _var2.PropertyId = GenDataDef.GetClassProperties(_var2.ClassId).IndexOf(_var2.PropertyName);
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
        public GenFunctionGenerator(GenDataDef genDataDef, GenWriter genWriter, GenObject genObject, Fragment fragment) 
            : base(genDataDef, genWriter, fragment, genObject)
        {
        }

        protected override bool Generate()
        {
            var fn = (Function) Fragment;
            if (String.Compare(fn.FunctionName, "File", StringComparison.OrdinalIgnoreCase) == 0 &&
                (Writer.Stream == null || Writer.Stream is FileStream))
            {
                return (Writer.FileName = GenFragmentExpander.Expand(GenDataDef, GenObject, fn.Body().FragmentList[0])) != "";
            }
            return base.Generate();
        }
    }

    public class SegmentNavigator
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
            Contract.Ensures(Contract.Result<ISubClassBase>() != null);
            if (GenCardinality != GenCardinality.Inheritance)
                return GenObject.GetSubClass(ClassName);
            
            ISubClassBase subClassBase = new GenSubClass(GenObject.GenDataBase, GenObject, ClassId, GenObject.ParentSubClass.Definition);
            if (GenObjectClassId(GenObject) == ClassId)
                subClassBase.Add(GenObject);
            return subClassBase;
        }

        private GenCardinality GenCardinality
        {
            get { return _generator.GenCardinality; }
        }

        private int SegmentClassId
        {
            get {return GenDataDef.GetClassId(ClassName);}
        }

        private GenObject GenObject { get; set; }

        private int IndexOfSubClass()
        {
            var idx = GenObject.Definition.IndexOfSubClass(ClassName);
            Contract.Assert(idx != -1, "SubClass not found");
            return idx;
        }

        private int GenObjectClassId(GenObject genObject)
        {
            return GenDataDef.GetClassId(genObject.Definition.Name);
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

        private GenDataDef GenDataDef
        {
            get { return _generator.GenDataDef; }
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
            Index = Count - 1;
            if (ClassDef.IsAbstract || !ClassDef.IsInherited) return;
            while (!Eol() && ClassName != SubClassBase[Index].ClassName) Index--;
        }

        private int Count { get { return SubClassBase == null ? 0 : SubClassBase.Count; } }

        public bool Eol()
        {
            if (Index < 0 || SubClassBase == null || Index >= Count) return true;
            if (ClassDef.IsAbstract || !ClassDef.IsInherited) return false;
            if (Inheritance && ClassName != SubClassBase[Index].ClassName) return false;
            return false;
        }

        private GenDataDefClass ClassDef
        {
            get { return GenDataDef.GetClassDef(ClassId); }
        }

        private int ClassId
        {
            get { return SegmentClassId; }
        }
    }

    public class GenSegmentGenerator : GenContainerGenerator
    {
        public GenSegmentGenerator(GenDataDef genDataDef, GenWriter genWriter, GenObject genObject, Fragment fragment) 
            : base(genDataDef, genWriter, genObject, fragment)
        {
            Segment = (Segment) fragment;
            GenCardinality cardinality;
            Contract.Assert(Enum.TryParse(Segment.Cardinality, out cardinality), "Invalid segment cardinality: " + Segment.Cardinality);
            GenCardinality = cardinality;
            Navigator = new SegmentNavigator(this);
        }

        internal GenCardinality GenCardinality { get; private set; }
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
                                GenObject.GetValue(GenDataDef.GetId(placeholder.Class,
                                    placeholder.Property));
                    }
                }
            }
            return sepText;
        }
    }
}