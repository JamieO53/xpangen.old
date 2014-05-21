// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Profile;
using org.xpangen.Generator.Profile;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Additions to GenFragment class for testing
    /// </summary>
    internal class GenFragmentTest : GenFragment
    {
        public GenFragmentTest(GenDataDef genDataDef, GenContainerFragmentBase parentSegment,
            GenData genData = null, ProfileRoot profileRoot = null, GenObject genObject = null)
            : base(genDataDef, parentSegment, FragmentType.Null, genData, profileRoot, genObject) // Default for testing
        {
        }

        public override string ProfileLabel()
        {
            return "Label";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            return "Text";
        }

        public override string Expand(GenData genData)
        {
            return "Generate";
        }
    }
}