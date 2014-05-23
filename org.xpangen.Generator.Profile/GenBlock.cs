// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenBlock : GenFragment
    {
        public GenSegBody Body { get; private set; }

        public GenBlock(GenDataDef genDataDef, GenContainerFragmentBase parentSegment,
            GenData genData = null, ProfileRoot profileRoot = null, GenObject genObject = null)
            : base(genDataDef, parentSegment, FragmentType.Block, genData ?? parentSegment.GenData,
                profileRoot ?? parentSegment.ProfileRoot, genObject ?? parentSegment.GenObject)
        {
            Body = new GenSegBody(genDataDef, parentSegment);
        }

        public override string ProfileLabel()
        {
            return "Block";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString()].Format;
            return string.Format(format, new object[]
                                             {
                                                 Body.ProfileText(syntaxDictionary)
                                             }
                );
        }

        public override string Expand(GenData genData)
        {
            return Body.Expand(genData);
        }
    }
}