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
            get { return Function.FunctionName; }
            set { Function.FunctionName = value; }
        }

        private Function Function { get { return (Function) Fragment; }
        }
        public override string ProfileLabel()
        {
            return FunctionName;
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString()].Format;

            var param = new string[Body.Count];
            for (var i = 0; i < Body.Count; i++)
                param[i] = Body.Fragment[i].ProfileText(syntaxDictionary);
            var p = string.Join(" ", param);

            return string.Format(format, new object[]
                                             {
                                                 FunctionName,
                                                 p
                                             }
                );
        }
    }
}