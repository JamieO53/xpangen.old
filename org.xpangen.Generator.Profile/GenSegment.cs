// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Text;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenSegment : GenContainerFragmentBase
    {
        public GenBlock ItemBody { get; private set; }
        public GenFragment Separator { get; private set; }

        public GenSegment(GenSegmentParams genSegmentParams)
            : base(genSegmentParams.SetFragmentType(FragmentType.Segment))
        {
            Body.ParentSegment = this;
            ClassId = GenDataDef.Classes.IndexOf(genSegmentParams.ClassName);
            GenCardinality = genSegmentParams.Cardinality;
        }

        public GenCardinality GenCardinality 
        {
            get
            {
                GenCardinality c;
                Enum.TryParse(Segment.Cardinality, out c);
                return c;
            }
            private set { Segment.Cardinality = value.ToString(); } 
        }

        private Segment Segment
        {
            get { return (Segment) Fragment; }
        }
        public override string ProfileLabel()
        {
            return GenDataDef.Classes[ClassId].Name;
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString()].Format;
            return string.Format(format, new object[]
                                             {
                                                 GenDataDef.Classes[ClassId].Name,
                                                 Body.ProfileText(syntaxDictionary),
                                                 syntaxDictionary.GenCardinalityText[(int) GenCardinality]
                                             }
                );
        }

        public virtual string Expand(GenData genData)
        {
            var s = new StringBuilder();
            var isEmpty = true;
            string expanded;
            switch (GenCardinality)
            {
                case GenCardinality.All:
                    genData.First(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        GenObject = genData.Context[ClassId].GenObject;
                        s.Append(Body.Expand(genData));
                        genData.Next(ClassId);
                    }
                    break;
                case GenCardinality.AllDlm:
                    CheckDelimiter();
                    genData.First(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        GenObject = genData.Context[ClassId].GenObject;
                        ItemBody.GenObject = GenObject;
                        GenFragment genFragment1 = ItemBody;
                        expanded = GenFragmentExpander.Expand(genFragment1, genData, genFragment1.GenObject, genFragment1.Fragment);
                        if (!isEmpty && expanded != "")
                        {
                            GenFragment genFragment = Separator;
                            s.Append(GenFragmentExpander.Expand(genFragment, genData, genFragment.GenObject, genFragment.Fragment));
                        }
                        isEmpty = isEmpty && expanded == "";
                        s.Append(expanded);
                        genData.Next(ClassId);
                    }
                    break;
                case GenCardinality.Back:
                    genData.Last(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        GenObject = genData.Context[ClassId].GenObject;
                        s.Append(Body.Expand(genData));
                        genData.Prior(ClassId);
                    }
                    break;
                case GenCardinality.BackDlm:
                    CheckDelimiter();
                    genData.Last(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        GenObject = genData.Context[ClassId].GenObject;
                        ItemBody.GenObject = GenObject;
                        GenFragment genFragment = ItemBody;
                        expanded = GenFragmentExpander.Expand(genFragment, genData, genFragment.GenObject, genFragment.Fragment);
                        if (!isEmpty && expanded != "")
                        {
                            GenFragment genFragment1 = Separator;
                            s.Append(GenFragmentExpander.Expand(genFragment1, genData, genFragment1.GenObject, genFragment1.Fragment));
                        }
                        isEmpty = isEmpty && expanded == "";
                        s.Append(expanded);
                        genData.Prior(ClassId);
                    }
                    break;
                case GenCardinality.First:
                    genData.First(ClassId);
                    GenObject = genData.Context[ClassId].GenObject;
                    if (!genData.Eol(ClassId))
                        s.Append(Body.Expand(genData));
                    break;
                case GenCardinality.Tail:
                    genData.First(ClassId);
                    genData.Next(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        GenObject = genData.Context[ClassId].GenObject;
                        s.Append(Body.Expand(genData));
                        genData.Next(ClassId);
                    }
                    break;
                case GenCardinality.Last:
                    genData.Last(ClassId);
                    GenObject = genData.Context[ClassId].GenObject;
                    if (!genData.Eol(ClassId))
                        s.Append(Body.Expand(genData));
                    break;
                case GenCardinality.Trunk:
                    genData.Last(ClassId);
                    genData.Prior(ClassId);
                    while (!genData.Eol(ClassId))
                    {
                        GenObject = genData.Context[ClassId].GenObject;
                        s.Append(Body.Expand(genData));
                        genData.Prior(ClassId);
                    }
                    break;
                case GenCardinality.Reference:
                    s.Append(genData.GenDataDef.Classes[ClassId].Name);
                    s.Append("[Reference='");
                    s.Append(genData.Context[ClassId].Reference);
                    s.AppendLine("']");
                    break;
                case GenCardinality.Inheritance:
                    genData.SetInheritance(ClassId);
                    GenObject = genData.Context[ClassId].GenObject;
                    if (!genData.Eol(ClassId))
                        s.Append(Body.Expand(genData));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return s.ToString();
        }

        private void CheckDelimiter()
        {
            if (Separator != null) return;

            // Optimization: This is done once when this method is first called
            ItemBody = new GenBlock(new GenFragmentParams(GenDataDef, this, ParentContainer));

            for (var i = 0; i < Body.Count - 1; i++)
                ItemBody.Body.Add(Body.Fragment[i]);

            var last = Body.Count > 0 ? Body.Fragment[Body.Count - 1] : null;
            var lastText = last as GenTextBlock;
            if (last is GenTextBlock && lastText.Body.Count > 1)
            {
                var newText = new GenTextBlock(new GenFragmentParams(GenDataDef, this, this));
                for (var i = 0; i < lastText.Body.Count - 1; i++)
                    newText.Body.Add(lastText.Body.Fragment[i]);
                Separator = lastText.Body.Fragment[lastText.Body.Count - 1];
                ItemBody.Body.Add(newText);
            }
            else
                Separator = Body.Count > 0
                    ? last
                    : new GenTextFragment(new GenTextFragmentParams(GenDataDef, this, this, ""));
        }
    }
}