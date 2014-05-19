// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenProfileFragment : GenContainerFragmentBase
    {
        public GenProfileFragment(GenDataDef genDataDef) :
            base(genDataDef, null, FragmentType.Profile)
        {
            ClassId = 0;
        }

        public override string ProfileLabel()
        {
            return "Profile";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            return Body.ProfileText(syntaxDictionary);
        }

        public override string Expand(GenData genData)
        {
            return Body.Expand(genData);
        }

        public override bool Generate(GenFragment prefix, GenData genData, GenWriter writer)
        {
            return Body.Generate(prefix, genData, writer);
        }
    }
}