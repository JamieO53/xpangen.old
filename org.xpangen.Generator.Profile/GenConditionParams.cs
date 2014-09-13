// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenConditionParams : GenFragmentParams
    {
        public GenDataId Var1 { get; private set; }
        public GenDataId Var2 { get; private set; }
        public GenComparison GenComparison { get; private set; }
        public bool UseLit { get; private set; }
        public string Lit { get; private set; }

        public GenConditionParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment,
            GenContainerFragmentBase parentContainer, ConditionParameters conditionParameters, bool isPrimary = true)
            : base(genDataDef, parentSegment, parentContainer, FragmentType.Condition, isPrimary)
        {
            Var1 = conditionParameters.Var1;
            Var2 = conditionParameters.Var2;
            GenComparison = conditionParameters.GenComparison;
            UseLit = conditionParameters.UseLit;
            Lit = conditionParameters.Lit;
        }
    }
}