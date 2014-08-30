// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
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
            var format = syntaxDictionary[FragmentType + (Body.SecondaryCount == 0 ? "1" : "2")].Format;
            if (Body.SecondaryCount == 0)
                return string.Format(format, new object[]
                                             {
                                                 GenDataDef.Classes[ClassId].Name,
                                                 Body.ProfileText(syntaxDictionary),
                                                 syntaxDictionary.GenCardinalityText[(int) GenCardinality]
                                             }
                    );
            return string.Format(format, new object[]
                                             {
                                                 GenDataDef.Classes[ClassId].Name,
                                                 Body.ProfileText(syntaxDictionary),
                                                 syntaxDictionary.GenCardinalityText[(int) GenCardinality],
                                                 Body.SecondaryProfileText(syntaxDictionary)
                                             }
                    );
        }
    }
}