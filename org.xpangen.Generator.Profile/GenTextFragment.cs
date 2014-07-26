﻿// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenTextFragment : GenFragment
    {
        public GenTextFragment(GenTextFragmentParams genTextFragmentParams) :
            base(genTextFragmentParams.SetFragmentType(FragmentType.Text))
        {
            Text = genTextFragmentParams.Text;
        }

        public Text TextFragment { get { return (Text) Fragment; }}

        public string Text
        {
            get { return TextFragment.TextValue; } 
            set { TextFragment.TextValue = value; }
        }

        public override string ProfileLabel()
        {
            return "Text";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            return Text;
        }

        public override string Expand(GenData genData)
        {
            return Text;
        }
    }
}