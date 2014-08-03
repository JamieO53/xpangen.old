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

        protected GenFragmentGenerator(GenData genData, GenWriter writer, Fragment fragment, GenFragment genFragment)
        {
            _genData = genData;
            _writer = writer;
            _fragment = fragment;
            GenFragment = genFragment;
            GenObject = GenFragment.GenObject;
        }

        protected GenFragmentGenerator()
        {
        }

        protected GenObject GenObject { get; set; }
        
        protected GenData GenData
        {
            get { return _genData; }
        }

        protected GenWriter Writer
        {
            get { return _writer; }
        }

        protected GenFragment GenFragment { get; set; }

        protected Fragment Fragment
        {
            get { return _fragment; }
        }

        protected virtual bool Generate()
        {
            var expanded = GenFragmentExpander.Expand(GenFragment, GenData);
            Writer.Write(expanded);
            return expanded != "";
        }

        private static GenFragmentGenerator Create(GenFragment genFragment, GenData genData, GenWriter genWriter)
        {
            if (genFragment.FragmentType != FragmentType.Null && genFragment.FragmentType != FragmentType.Text)
                if (genFragment.FragmentType != FragmentType.Lookup || !((GenLookup)genFragment).NoMatch)
                    Assert(genFragment.GenObject != null, "The genObject must be set");
            FragmentType fragmentType;
            Enum.TryParse(genFragment.Fragment.GetType().Name, out fragmentType);
            switch (fragmentType)
            {
                case FragmentType.Profile:
                    return new GenContainerGenerator(genFragment, genData, genWriter);
                case FragmentType.Segment:
                    return new GenSegmentGenerator(genFragment, genData, genWriter);
                case FragmentType.Block:
                    return new GenContainerGenerator(genFragment, genData, genWriter);
                case FragmentType.Lookup:
                    return new GenLookupGenerator(genFragment, genData, genWriter);
                case FragmentType.Condition:
                    return new GenConditionGenerator(genFragment, genData, genWriter);
                case FragmentType.Function:
                    return new GenFunctionGenerator(genFragment, genData, genWriter);
                case FragmentType.TextBlock:
                    return new GenContainerGenerator(genFragment, genData, genWriter);
                default:
                    return new GenFragmentGenerator(genData, genWriter, genFragment.Fragment, genFragment);
            }
        }

        public static bool Generate(GenFragment genFragment, GenData genData, GenWriter genWriter)
        {
            return Create(genFragment, genData, genWriter).Generate();
        }
    }

    public class GenConditionGenerator : GenContainerGenerator
    {
        internal GenConditionGenerator(GenFragment genFragment, GenData genData, GenWriter writer) 
            : base(genFragment, genData, writer)
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
            value2 = UseLit
                         ? Lit
                         : (GenComparison == GenComparison.Exists || GenComparison == GenComparison.NotExists
                                ? ""
                                : GenObject.GetValue(Var2));

            if (GenComparison != GenComparison.Exists && GenComparison != GenComparison.NotExists &&
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
        public GenContainerGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter) 
            : base(genData, genWriter, genFragment.Fragment, genFragment)
        {
        }

        protected GenObject OverrideGenObject { get; set; }
        
        protected override bool Generate()
        {
            var generated = false;
            var container = (GenContainerFragmentBase) GenFragment;
            var genObject = OverrideGenObject ?? container.GenObject;
            foreach (var fragment in container.Body.Fragment)
            {
                fragment.GenObject = genObject;
                generated |= Generate(fragment, GenData, Writer);
            }
            return generated;
        }
    }

    public class GenLookupGenerator : GenContainerGenerator
    {
        private GenDataId _var1;
        private GenDataId _var2;

        public GenLookupGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter) 
            : base(genFragment, genData, genWriter)
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
            var generated = false;
            if (NoMatch)
            {
                if (GenData.Eol(Var2.ClassId))
                    generated |= base.Generate();
                else
                {
                    if (GenObject == null || SearchFor(Var1, GenObject.GetValue(Var2)) == null)
                        generated |= base.Generate();
                }
            }
            else
            {
                if (GenData.Eol(Var2.ClassId)) return false;
                var v = GenData.GetValue(Var2);
                var o = SearchFor(Var1, v);
                if (o == null) return false;
                OverrideGenObject = o;
                generated |= base.Generate();
            }
            return generated;
        }

        private bool NoMatch { get { return Lookup.NoMatch != ""; } }

        private GenObject SearchFor(GenDataId id, string value)
        {
            var searchObjects = FindSearchObjects(GenObject, id.ClassName);
            if (searchObjects == null) return null;
           foreach (var searchObject in searchObjects)
                if (searchObject.GetValue(id) == value) return searchObject;
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
        public GenFunctionGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter) 
            : base(genData, genWriter, genFragment.Fragment, genFragment)
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

    public class GenSegmentGenerator : GenContainerGenerator
    {
        public GenSegmentGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter) 
            : base(genFragment, genData, genWriter)
        {
            GenFragment = genFragment;
        }

        private GenBlock ItemBody { get; set; }
        private GenFragment Separator { get; set; }
        private Segment Segment {get { return (Segment) Fragment; }}

        protected override bool Generate()
        {
            var generated = false;
            var seg = (GenSegment) GenFragment;
            string sepText;
            //var segmentObjects = GetSegmentObjects();

            switch (seg.GenCardinality)
            {
                case GenCardinality.All:
                    GenData.First(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        OverrideGenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= base.Generate();
                        GenData.Next(seg.ClassId);
                    }
                    break;
                case GenCardinality.AllDlm:
                    CheckDelimiter(seg);
                    sepText = GenFragmentExpander.Expand(Separator, GenData);
                    GenData.First(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        ItemBody.GenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= Generate(ItemBody, GenData, Writer);
                        if (generated) Writer.ProvisionalWrite(sepText);
                        GenData.Next(seg.ClassId);
                    }
                    Writer.ClearProvisionalText();
                    break;
                case GenCardinality.Back:
                    GenData.Last(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        OverrideGenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= base.Generate();
                        GenData.Prior(seg.ClassId);
                    }
                    break;
                case GenCardinality.BackDlm:
                    CheckDelimiter(seg);
                    sepText = GenFragmentExpander.Expand(Separator, GenData);
                    GenData.Last(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        ItemBody.GenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= Generate(ItemBody, GenData, Writer);
                        if (generated) Writer.ProvisionalWrite(sepText);
                        GenData.Prior(seg.ClassId);
                    }
                    Writer.ClearProvisionalText();
                    break;
                case GenCardinality.First:
                    GenData.First(seg.ClassId);
                    OverrideGenObject = GenData.Context[seg.ClassId].GenObject;
                    if (!GenData.Eol(seg.ClassId))
                        generated |= base.Generate();
                    break;
                case GenCardinality.Tail:
                    GenData.First(seg.ClassId);
                    GenData.Next(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        OverrideGenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= base.Generate();
                        GenData.Next(seg.ClassId);
                    }
                    break;
                case GenCardinality.Last:
                    GenData.Last(seg.ClassId);
                    OverrideGenObject = GenData.Context[seg.ClassId].GenObject;
                    if (!GenData.Eol(seg.ClassId))
                        generated |= base.Generate();
                    break;
                case GenCardinality.Trunk:
                    GenData.Last(seg.ClassId);
                    GenData.Prior(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        OverrideGenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= base.Generate();
                        GenData.Prior(seg.ClassId);
                    }
                    break;
                case GenCardinality.Reference:
                    Writer.Write(GenData.GenDataDef.Classes[seg.ClassId].Name);
                    Writer.Write("[Reference='");
                    Writer.Write(GenData.Context[seg.ClassId].Reference);
                    Writer.Write("']\r\n");
                    break;
                case GenCardinality.Inheritance:
                    GenData.SetInheritance(seg.ClassId);
                    OverrideGenObject = GenData.Context[seg.ClassId].GenObject;
                    if (!GenData.Eol(seg.ClassId))
                        generated |= base.Generate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return generated;
        }

        private ISubClassBase GetSegmentObjects()
        {
            var classes = GenData.GenDataDef.Classes;
            var classId = classes.IndexOf(GenObject.ClassName);
            while (classes[classId].IsInherited && GetSubClassIndex(classes, classId) == -1)
                classId = classes[classId].Parent.ClassId;
            
            Assert(classId != -1, "Segment parent class not found: " + GenObject.ClassName);
            
            var subClassIdx = GetSubClassIndex(classes, classId);
            Assert(subClassIdx != -1,
                "Segment class is not a subclass of its parent: " + GenObject.ClassName + " -> " + Segment.Class);
            var segmentObjects = GenObject.SubClass[subClassIdx];
            return segmentObjects;
        }

        private int GetSubClassIndex(GenDataDefClassList classes, int classId)
        {
            var subClassId = classes.IndexOf(Segment.Class);
            while (classes[subClassId].IsInherited && classes[classId].SubClasses.IndexOf(subClassId) == -1)
                subClassId = classes[subClassId].Parent.ClassId;
            var subClassIdx = classes[classId].SubClasses.IndexOf(subClassId);
            return subClassIdx;
        }

        private void CheckDelimiter(GenSegment seg)
        {
            if (Separator != null) return;

            // Optimization: This is done once when this method is first called
            ItemBody = new GenBlock(new GenFragmentParams(seg.GenDataDef, seg, seg.ParentContainer));

            for (var i = 0; i < seg.Body.Count - 1; i++)
                ItemBody.Body.Add(seg.Body.Fragment[i]);

            var last = seg.Body.Count > 0 ? seg.Body.Fragment[seg.Body.Count - 1] : null;
            var lastText = last as GenTextBlock;
            if (last is GenTextBlock && lastText.Body.Count > 1)
            {
                var newText = new GenTextBlock(new GenFragmentParams(seg.GenDataDef, seg, seg));
                for (var i = 0; i < lastText.Body.Count - 1; i++)
                    newText.Body.Add(lastText.Body.Fragment[i]);
                ItemBody.Body.Add(newText);
                Separator = lastText.Body.Fragment[lastText.Body.Count - 1];
            }
            else
                Separator = seg.Body.Count > 0
                    ? last
                    : GenFragment.NullFragment;
            if (Separator != null) Separator.GenObject = GenObject;
            else Separator = GenFragment.NullFragment;
        }
    }
}