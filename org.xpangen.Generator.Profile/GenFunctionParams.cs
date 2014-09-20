// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenFunctionParams : GenFragmentParams
    {
        public GenFunctionParams(GenDataDef genDataDef, GenContainerFragmentBase parentContainer, string functionName, FragmentType fragmentType, bool isPrimary = true) : base(genDataDef, parentContainer, fragmentType, isPrimary: isPrimary)
        {
            FunctionName = functionName;
        }

        public GenFunctionParams(GenDataDef genDataDef, GenContainerFragmentBase parentContainer, string functionName, bool isPrimary = true) : base(genDataDef, parentContainer, isPrimary: isPrimary)
        {
            FunctionName = functionName;
        }

        public string FunctionName { get; private set; }
    }
}