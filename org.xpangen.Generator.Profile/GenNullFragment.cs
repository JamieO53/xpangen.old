// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenNullFragment : GenFragment
    {
        public GenNullFragment(GenDataDef genDataDef, GenContainerFragmentBase parentSegment,
            GenData genData = null, ProfileRoot profileRoot = null, GenObject genObject = null)
            : base(genDataDef, parentSegment, FragmentType.Null, genData, profileRoot, genObject)
        {
        }

        public override string ProfileLabel()
        {
            return "";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            return "";
        }

        public override string Expand(GenData genData)
        {
            return "";
        }
    }
}