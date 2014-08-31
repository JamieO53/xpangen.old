// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFunction : GenContainerFragmentBase
    {
        public GenFunction(GenFragmentParams genFragmentParams)
            : base(genFragmentParams.SetFragmentType(FragmentType.Function))
        {
        }

        public string FunctionName
        {
            private get { return Function.FunctionName; }
            set { Function.FunctionName = value; }
        }

        private Function Function { get { return (Function) Fragment; }
        }
        public override string ProfileLabel()
        {
            return FunctionName;
        }
    }
}