// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFunctionParams : GenFragmentParams
    {
        public GenFunctionParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, string functionName, FragmentType fragmentType, bool isPrimary = true) : base(genDataDef, parentSegment, parentContainer, fragmentType, isPrimary)
        {
            FunctionName = functionName;
        }

        public GenFunctionParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, string functionName, bool isPrimary = true) : base(genDataDef, parentSegment, parentContainer, isPrimary)
        {
            FunctionName = functionName;
        }

        public GenFunctionParams(GenDataDef genDataDef, Function function) : base(genDataDef, function)
        {
        }

        public string FunctionName { get; private set; }
    }
}