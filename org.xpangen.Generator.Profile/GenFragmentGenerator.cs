using System;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFragmentGenerator : GenBase
    {
        private readonly GenFragment _prefix;
        private readonly GenData _genData;
        private readonly GenWriter _writer;
        private readonly Fragment _fragment;

        protected GenFragmentGenerator(GenFragment prefix, GenData genData, GenWriter writer, Fragment fragment)
        {
            _prefix = prefix;
            _genData = genData;
            _writer = writer;
            _fragment = fragment;
        }

        protected GenFragmentGenerator()
        {
        }

        public GenFragment Prefix
        {
            get { return _prefix; }
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
            var expanded = GenFragment.Expand(GenData);
            if (expanded != "" && Prefix != null)
                Create(Prefix, null, GenData, Writer).Generate();
            Writer.Write(expanded);
            return expanded != "";
        }

        private static GenFragmentGenerator Create(GenFragment genFragment, GenFragment prefix, GenData genData, GenWriter genWriter)
        {
            FragmentType fragmentType;
            Enum.TryParse(genFragment.Fragment.GetType().Name, out fragmentType);
            switch (fragmentType)
            {
                case FragmentType.Profile:
                    return new GenContainerGenerator(genFragment, prefix, genData, genWriter);
                case FragmentType.Null:
                    return new GenNullGenerator();
                case FragmentType.Text:
                    return new GenTextGenerator(genFragment, prefix, genData, genWriter);
                case FragmentType.Placeholder:
                    return new GenPlaceholderGenerator(genFragment, prefix, genData, genWriter);
                //case FragmentType.Body:
                //    break;
                case FragmentType.Segment:
                    return new GenSegmentGenerator(genFragment, prefix, genData, genWriter);
                case FragmentType.Block:
                    return new GenContainerGenerator(genFragment, prefix, genData, genWriter);
                case FragmentType.Lookup:
                    return new GenLookupGenerator(genFragment, prefix, genData, genWriter);
                case FragmentType.Condition:
                    return new GenConditionGenerator(genFragment, prefix, genData, genWriter);
                case FragmentType.Function:
                    return new GenFunctionGenerator(genFragment, prefix, genData, genWriter);
                case FragmentType.TextBlock:
                    return new GenContainerGenerator(genFragment, prefix, genData, genWriter);
                default:
                    return new GenFragmentGenerator(prefix, genData, genWriter, genFragment.Fragment) {GenFragment = genFragment};
            }
        }

        public static bool Generate(GenFragment genFragment, GenFragment prefix, GenData genData, GenWriter genWriter)
        {
            return Create(genFragment, prefix, genData, genWriter).Generate();
        }
    }

    public class GenConditionGenerator : GenFragmentGenerator
    {
        internal GenConditionGenerator(GenFragment genFragment, GenFragment prefix, GenData genData, GenWriter writer) : base(prefix, genData, writer, genFragment.Fragment)
        {
            GenFragment = genFragment;
        }

        protected override bool Generate()
        {
            var cond = (GenCondition) GenFragment;

            cond.Body.GenObject = cond.GenObject;
            if (cond.Test(GenData)) return cond.Body.Generate(Prefix, GenData, Writer);
            return false;
        }
    }

    public class GenContainerGenerator : GenFragmentGenerator
    {
        public GenContainerGenerator(GenFragment genFragment, GenFragment prefix, GenData genData, GenWriter genWriter) 
            : base(prefix, genData, genWriter, genFragment.Fragment)
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
                generated |= Generate(fragment, Prefix, GenData, Writer);
            }
            return generated;
        }
    }

    public class GenLookupGenerator : GenFragmentGenerator
    {
        public GenLookupGenerator(GenFragment genFragment, GenFragment prefix, GenData genData, GenWriter genWriter) 
            : base(prefix, genData, genWriter, genFragment.Fragment)
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
                    generated |= lkp.Body.Generate(Prefix, GenData, Writer);
                }
                else
                {
                    var v = contextData.GetValue(lkp.Var2);
                    SearchFor(contextData, lkp.ClassId, lkp.Var1, v);
                    if (contextData.Eol(lkp.ClassId))
                    {
                        lkp.GenObject = contextData.Context[lkp.ClassId].GenObject;
                        generated |= lkp.Body.Generate(Prefix, GenData, Writer);
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
                        generated |= lkp.Body.Generate(Prefix, GenData, Writer);
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
        public GenFunctionGenerator(GenFragment genFragment, GenFragment prefix, GenData genData, GenWriter genWriter) 
            : base(prefix, genData, genWriter, genFragment.Fragment)
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

        public GenSegmentGenerator(GenFragment genFragment, GenFragment prefix, GenData genData, GenWriter genWriter) 
            : base(prefix, genData, genWriter, genFragment.Fragment)
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
            GenBlock myPrefix;
            var sepText = "";
            switch (seg.GenCardinality)
            {
                case GenCardinality.All:
                    GenData.First(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= seg.Body.Generate(Prefix, GenData, Writer);
                        GenData.Next(seg.ClassId);
                    }
                    break;
                case GenCardinality.AllDlm:
                    CheckDelimiter(seg);
                    sepText = Separator.Expand(GenData);
                    myPrefix = new GenBlock(new GenFragmentParams(seg.GenDataDef, seg, seg.ParentContainer));
                    if (Prefix != null)
                        myPrefix.Body.Add(Prefix);
                    GenData.First(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        seg.ItemBody.GenObject = GenObject;
                        generated |= Generate(seg.ItemBody, myPrefix, GenData, Writer);
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
                        generated |= seg.Body.Generate(Prefix, GenData, Writer);
                        GenData.Prior(seg.ClassId);
                    }
                    break;
                case GenCardinality.BackDlm:
                    CheckDelimiter(seg);
                    sepText = Separator.Expand(GenData);
                    myPrefix = new GenBlock(new GenFragmentParams(seg.GenDataDef, seg, seg.ParentContainer));
                    GenData.Last(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        seg.ItemBody.GenObject = GenObject;
                        generated |= Generate(seg.ItemBody, myPrefix, GenData, Writer);
                        if (generated) Writer.ProvisionalWrite(sepText);
                        GenData.Prior(seg.ClassId);
                    }
                    break;
                case GenCardinality.First:
                    GenData.First(seg.ClassId);
                    GenObject = GenData.Context[seg.ClassId].GenObject;
                    if (!GenData.Eol(seg.ClassId))
                        generated |= seg.Body.Generate(Prefix, GenData, Writer);
                    break;
                case GenCardinality.Tail:
                    GenData.First(seg.ClassId);
                    GenData.Next(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= seg.Body.Generate(Prefix, GenData, Writer);
                        GenData.Next(seg.ClassId);
                    }
                    break;
                case GenCardinality.Last:
                    GenData.Last(seg.ClassId);
                    GenObject = GenData.Context[seg.ClassId].GenObject;
                    if (!GenData.Eol(seg.ClassId))
                        generated |= seg.Body.Generate(Prefix, GenData, Writer);
                    break;
                case GenCardinality.Trunk:
                    GenData.Last(seg.ClassId);
                    GenData.Prior(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        generated |= seg.Body.Generate(Prefix, GenData, Writer);
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
                        generated |= seg.Body.Generate(Prefix, GenData, Writer);
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

    public class GenTextGenerator : GenFragmentGenerator
    {
        public GenTextGenerator(GenFragment genFragment, GenFragment prefix, GenData genData, GenWriter genWriter) 
            : base(prefix, genData, genWriter, genFragment.Fragment)
        {
            GenFragment = genFragment;
        }

        protected override bool Generate()
        {
            var text = (GenTextFragment) GenFragment;
            if (text.Text == "") return false;
            Writer.Write(text.Text);
            return true;
        }
    }

    public class GenPlaceholderGenerator : GenFragmentGenerator
    {
        public GenPlaceholderGenerator(GenFragment genFragment, GenFragment prefix, GenData genData, GenWriter genWriter) 
            : base(prefix, genData, genWriter, genFragment.Fragment)
        {
            GenFragment = genFragment;
        }

        protected override bool Generate()
        {
            var placeholder = (GenPlaceholderFragment) GenFragment;
            var text = placeholder.GenObject.GetValue(placeholder.Id);
            if (text == "") return false;
            Writer.Write(text);
            return true;
        }
    }

    public class GenNullGenerator : GenFragmentGenerator
    {
        protected override bool Generate()
        {
            return false;
        }
    }
}