// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenConditionParams : GenFragmentParams
    {
        public GenDataId Var1 { get; set; }
        public GenDataId Var2 { get; set; }
        public GenComparison GenComparison { get; set; }
        public bool UseLit { get; set; }
        public string Lit { get; set; }

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

        public GenConditionParams(GenDataDef genDataDef, Condition condition) : base(genDataDef, null, null, FragmentType.Condition)
        {
            UseLit = condition.UseLit != "";
            Lit = condition.Lit;
            Var1 = genDataDef.GetId(condition.Class1, condition.Property1);
            GenComparison comparison;
            if (!Enum.TryParse(condition.Comparison, out comparison))
                throw new ArgumentException("Invalid comparison: " + condition.Comparison);
            GenComparison = comparison;
            if (!UseLit && comparison != GenComparison.Exists && comparison != GenComparison.NotExists)
                Var2 = genDataDef.GetId(condition.Class2, condition.Property2);
            Fragment = condition;
        }
    }
}