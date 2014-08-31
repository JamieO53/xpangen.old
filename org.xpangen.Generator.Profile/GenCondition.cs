// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenCondition : GenContainerFragmentBase
    {
        public GenCondition(GenConditionParams genConditionParams)
            : base(genConditionParams.SetFragmentType(FragmentType.Condition))
        {
            var condition = (Condition) Fragment;
            condition.Class1 = genConditionParams.Var1.ClassName;
            condition.Property1 = genConditionParams.Var1.PropertyName;
            condition.Class2 = genConditionParams.Var2.ClassName;
            condition.Property2 = genConditionParams.Var2.PropertyName;
            condition.Comparison = genConditionParams.GenComparison.ToString();
            condition.UseLit = genConditionParams.UseLit ? "True" : "";
            condition.Lit = genConditionParams.Lit;
        }
    }
}