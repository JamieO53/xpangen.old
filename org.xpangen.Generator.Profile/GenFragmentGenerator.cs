// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
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

        protected GenFragmentGenerator(GenData genData, GenWriter writer, Fragment fragment)
        {
            _genData = genData;
            _writer = writer;
            _fragment = fragment;
        }

        protected GenFragmentGenerator()
        {
        }

        public GenData GenData
        {
            get { return _genData; }
        }

        public GenWriter Writer
        {
            get { return _writer; }
        }

        protected GenFragment GenFragment { get; set; }

        public Fragment Fragment
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
                    return new GenFragmentGenerator(genData, genWriter, genFragment.Fragment) {GenFragment = genFragment};
            }
        }

        public static bool Generate(GenFragment genFragment, GenData genData, GenWriter genWriter)
        {
            return Create(genFragment, genData, genWriter).Generate();
        }
    }

    public class GenConditionGenerator : GenFragmentGenerator
    {
        internal GenConditionGenerator(GenFragment genFragment, GenData genData, GenWriter writer) : base(genData, writer, genFragment.Fragment)
        {
            GenFragment = genFragment;
        }

        protected override bool Generate()
        {
            var cond = (GenCondition) GenFragment;

            cond.Body.GenObject = cond.GenObject;
            if (cond.Test(GenData)) return cond.Body.Generate(GenData, Writer);
            return false;
        }
    }

    public class GenContainerGenerator : GenFragmentGenerator
    {
        public GenContainerGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter) 
            : base(genData, genWriter, genFragment.Fragment)
        {
            GenFragment = genFragment;
        }

        protected override bool Generate()
        {
            var generated = false;
            var container = (GenContainerFragmentBase) GenFragment;
            foreach (var fragment in container.Body.Fragment)
            {
                fragment.GenObject = container.GenObject;
                generated |= Generate(fragment, GenData, Writer);
            }
            return generated;
        }
    }

    public class GenLookupGenerator : GenFragmentGenerator
    {
        public GenLookupGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter) 
            : base(genData, genWriter, genFragment.Fragment)
        {
            GenFragment = genFragment;
        }

        protected override bool Generate()
        {
            var generated = false;
            var lkp = (GenLookup)GenFragment;
            if (lkp.NoMatch)
            {
                var contextData = GenData.DuplicateContext();

                if (GenData.Eol(lkp.Var2.ClassId))
                {
                    contextData.Reset(lkp.Var1.ClassId);
                    lkp.GenObject = contextData.Context[lkp.ClassId].GenObject;
                    generated |= lkp.Body.Generate(GenData, Writer);
                }
                else
                {
                    var v = contextData.GetValue(lkp.Var2);
                    SearchFor(contextData, lkp.ClassId, lkp.Var1, v);
                    if (contextData.Eol(lkp.ClassId))
                    {
                        lkp.GenObject = contextData.Context[lkp.ClassId].GenObject;
                        generated |= lkp.Body.Generate(GenData, Writer);
                    }
                }
            }
            else
            {
                if (!GenData.Eol(lkp.Var2.ClassId))
                {
                    var contextData = GenData.DuplicateContext();
                    var v = contextData.GetValue(lkp.Var2);
                    SearchFor(contextData, lkp.ClassId, lkp.Var1, v);
                    if (!contextData.Eol(lkp.ClassId))
                    {
                        lkp.GenObject = contextData.Context[lkp.ClassId].GenObject;
                        generated |= lkp.Body.Generate(GenData, Writer);
                    }
                }
            }
            return generated;
        }

        private static void SearchFor(GenData genData, int classId, GenDataId id, string value)
        {
            genData.First(classId);
            while (!genData.Eol(classId) && genData.GetValue(id) != value)
            {
                genData.Next(classId);
            }
        }
    }

    public class GenFunctionGenerator : GenFragmentGenerator
    {
        public GenFunctionGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter) 
            : base(genData, genWriter, genFragment.Fragment)
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

    public class GenSegmentGenerator : GenFragmentGenerator
    {
        private GenObject _genObject;

        public GenSegmentGenerator(GenFragment genFragment, GenData genData, GenWriter genWriter) 
            : base(genData, genWriter, genFragment.Fragment)
        {
            GenFragment = genFragment;
        }


        private GenObject GenObject
        {
            get { return _genObject; }
            set
            {
                ((GenSegment)GenFragment).Body.GenObject = value;
                _genObject = value;
            }
        }

        private GenBlock ItemBody { get; set; }
        private GenFragment Separator { get; set; }

        protected override bool Generate()
        {
            var generated = false;
            var seg = (GenSegment) GenFragment;
            string sepText;
            switch (seg.GenCardinality)
            {
                case GenCardinality.All:
                    GenData.First(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= seg.Body.Generate(GenData, Writer);
                        GenData.Next(seg.ClassId);
                    }
                    break;
                case GenCardinality.AllDlm:
                    CheckDelimiter(seg);
                    sepText = GenFragmentExpander.Expand(Separator, GenData);
                    GenData.First(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        ItemBody.GenObject = GenObject;
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
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= seg.Body.Generate(GenData, Writer);
                        GenData.Prior(seg.ClassId);
                    }
                    break;
                case GenCardinality.BackDlm:
                    CheckDelimiter(seg);
                    sepText = GenFragmentExpander.Expand(Separator, GenData);
                    GenData.Last(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        ItemBody.GenObject = GenObject;
                        generated |= Generate(ItemBody, GenData, Writer);
                        if (generated) Writer.ProvisionalWrite(sepText);
                        GenData.Prior(seg.ClassId);
                    }
                    Writer.ClearProvisionalText();
                    break;
                case GenCardinality.First:
                    GenData.First(seg.ClassId);
                    GenObject = GenData.Context[seg.ClassId].GenObject;
                    if (!GenData.Eol(seg.ClassId))
                        generated |= seg.Body.Generate(GenData, Writer);
                    break;
                case GenCardinality.Tail:
                    GenData.First(seg.ClassId);
                    GenData.Next(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= seg.Body.Generate(GenData, Writer);
                        GenData.Next(seg.ClassId);
                    }
                    break;
                case GenCardinality.Last:
                    GenData.Last(seg.ClassId);
                    GenObject = GenData.Context[seg.ClassId].GenObject;
                    if (!GenData.Eol(seg.ClassId))
                        generated |= seg.Body.Generate(GenData, Writer);
                    break;
                case GenCardinality.Trunk:
                    GenData.Last(seg.ClassId);
                    GenData.Prior(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= seg.Body.Generate(GenData, Writer);
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
                    GenObject = GenData.Context[seg.ClassId].GenObject;
                    if (!GenData.Eol(seg.ClassId))
                        generated |= seg.Body.Generate(GenData, Writer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return generated;
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
                Separator = lastText.Body.Fragment[lastText.Body.Count - 1];
                ItemBody.Body.Add(newText);
            }
            else
                Separator = seg.Body.Count > 0
                    ? last
                    : GenFragment.NullFragment;
        }
    }
}