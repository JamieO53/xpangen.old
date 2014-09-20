// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenCondition : GenContainerFragmentBase
    {
        public GenCondition(GenConditionParams genConditionParams)
            : base(genConditionParams.SetFragmentType(FragmentType.Condition))
        {
            Condition = (Condition) Fragment;
            Condition.Class1 = genConditionParams.Var1.ClassName;
            Condition.Property1 = genConditionParams.Var1.PropertyName;
            Condition.Class2 = genConditionParams.Var2.ClassName;
            Condition.Property2 = genConditionParams.Var2.PropertyName;
            Condition.Comparison = genConditionParams.GenComparison.ToString();
            Condition.UseLit = genConditionParams.UseLit ? "True" : "";
            Condition.Lit = genConditionParams.Lit;
        }

        public Condition Condition { get; private set; }
    }
}