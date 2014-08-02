// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenTextBlock : GenContainerFragmentBase
    {
        public GenTextBlock(GenFragmentParams genFragmentParams)
            : base(genFragmentParams.SetFragmentType(FragmentType.TextBlock))
        {
        }

        public TextBlock TextBlock
        {
            get { return (TextBlock) Fragment; }
            set { Fragment = value; }
        }
        public override string ProfileLabel()
        {
            return "Text";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString()].Format;
            return string.Format(format, new object[]
                                             {
                                                 Body.ProfileText(syntaxDictionary)
                                             }
                );
        }

        public virtual string Expand(GenData genData)
        {
            Body.GenObject = GenObject;
            return Body.Expand(genData);
        }
    }
}