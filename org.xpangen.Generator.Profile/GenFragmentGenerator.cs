using System;
using System.IO;
using System.Linq;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFragmentGenerator : GenBase
    {
        private GenFragment _prefix;
        private GenData _genData;
        private GenWriter _writer;

        public GenFragmentGenerator(GenFragment prefix, GenData genData, GenWriter writer)
        {
            _prefix = prefix;
            _genData = genData;
            _writer = writer;
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

        public GenFragment Fragment { get; set; }

        public virtual bool Generate()
        {
            var expanded = Fragment.Expand(GenData);
            if (expanded != "" && Prefix != null)
                Create(Prefix, null, GenData, Writer).Generate();
            Writer.Write(expanded);
            return expanded != "";
        }

        public static GenFragmentGenerator Create(GenFragment fragment, GenFragment prefix, GenData genData, GenWriter genWriter)
        {
            FragmentType fragmentType;
            Enum.TryParse(fragment.Fragment.GetType().Name, out fragmentType);
            switch (fragmentType)
            {
                case FragmentType.Profile:
                    return new GenContainerGenerator(fragment, prefix, genData, genWriter);
                //case FragmentType.Null:
                //    break;
                //case FragmentType.Text:
                //    break;
                //case FragmentType.Placeholder:
                //    break;
                //case FragmentType.Body:
                //    break;
                case FragmentType.Segment:
                    return new GenSegmentGenerator(fragment, prefix, genData, genWriter);
                case FragmentType.Block:
                    return new GenContainerGenerator(fragment, prefix, genData, genWriter);
                case FragmentType.Lookup:
                    return new GenLookupGenerator(fragment, prefix, genData, genWriter);
                case FragmentType.Condition:
                    return new GenConditionGenerator(fragment, prefix, genData, genWriter);
                case FragmentType.Function:
                    return new GenFunctionGenerator(fragment, prefix, genData, genWriter);
                case FragmentType.TextBlock:
                    return new GenContainerGenerator(fragment, prefix, genData, genWriter);
                default:
                    return new GenFragmentGenerator(prefix, genData, genWriter) {Fragment = fragment};
            }
        }
    }

    public class GenConditionGenerator : GenFragmentGenerator
    {
        public GenConditionGenerator(GenFragment fragment, GenFragment prefix, GenData genData, GenWriter writer) : base(prefix, genData, writer)
        {
            Fragment = fragment;
        }

        public override bool Generate()
        {
            var cond = (GenCondition) Fragment;

            cond.Body.GenObject = cond.GenObject;
            if (cond.Test(GenData)) return cond.Body.Generate(Prefix, GenData, Writer);
            return false;
        }
    }

    public class GenContainerGenerator : GenFragmentGenerator
    {
        public GenContainerGenerator(GenFragment fragment, GenFragment prefix, GenData genData, GenWriter genWriter) 
            : base(prefix, genData, genWriter)
        {
            Fragment = fragment;
        }

        public override bool Generate()
        {
            var generated = false;
            var container = (GenContainerFragmentBase) Fragment;
            foreach (var fragment in container.Body.Fragment)
            {
                fragment.GenObject = container.GenObject;
                generated |= Create(fragment, Prefix, GenData, Writer).Generate();
            }
            return generated;
        }
    }

    public class GenLookupGenerator : GenFragmentGenerator
    {
        public GenLookupGenerator(GenFragment fragment, GenFragment prefix, GenData genData, GenWriter genWriter) 
            : base(prefix, genData, genWriter)
        {
            Fragment = fragment;
        }

        public override bool Generate()
        {
            var generated = false;
            var lkp = (GenLookup)Fragment;
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
        public GenFunctionGenerator(GenFragment fragment, GenFragment prefix, GenData genData, GenWriter genWriter) 
            : base(prefix, genData, genWriter)
        {
            Fragment = fragment;
        }

        public override bool Generate()
        {
            var fn = (GenFunction) Fragment;
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

        public GenSegmentGenerator(GenFragment fragment, GenFragment prefix, GenData genData, GenWriter genWriter) 
            : base(prefix, genData, genWriter)
        {
            Fragment = fragment;
        }


        private GenObject GenObject
        {
            get { return _genObject; }
            set
            {
                ((GenSegment)Fragment).Body.GenObject = value;
                _genObject = value;
            }
        }
        public GenBlock ItemBody { get; set; }
        public GenFragment Separator { get; set; }

        public override bool Generate()
        {
            var generated = false;
            var seg = (GenSegment) Fragment;
            bool generatedItem;
            bool isEmpty;
            GenBlock myPrefix;
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
                    isEmpty = true;
                    myPrefix = new GenBlock(new GenFragmentParams(seg.GenDataDef, seg, seg.ParentContainer));
                    if (Prefix != null)
                        myPrefix.Body.Add(Prefix);
                    GenData.First(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        seg.ItemBody.GenObject = GenObject;
                        generatedItem =
                            Create(seg.ItemBody, myPrefix, GenData,
                                Writer).Generate();
                        if (isEmpty && generatedItem)
                        {
                            isEmpty = false;
                            if (seg.Separator != null)
                                myPrefix.Body.Add(seg.Separator);
                        }
                        GenData.Next(seg.ClassId);
                    }
                    generated = !isEmpty;
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
                    isEmpty = true;
                    myPrefix = new GenBlock(new GenFragmentParams(seg.GenDataDef, seg, seg.ParentContainer));
                    GenData.Last(seg.ClassId);
                    while (!GenData.Eol(seg.ClassId))
                    {
                        GenObject = GenData.Context[seg.ClassId].GenObject;
                        seg.ItemBody.GenObject = GenObject;
                        generatedItem =
                            Create(seg.ItemBody, myPrefix, GenData,
                                Writer).Generate();
                        if (isEmpty && generatedItem)
                        {
                            isEmpty = false;
                            if (seg.Separator != null)
                                myPrefix.Body.Add(seg.Separator);
                        }
                        GenData.Prior(seg.ClassId);
                    }
                    generated = !isEmpty;
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
                    : new GenTextFragment(new GenTextFragmentParams(seg.GenDataDef, seg, seg, ""));
        }
    }
}