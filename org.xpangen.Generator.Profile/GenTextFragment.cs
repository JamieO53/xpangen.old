// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

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

        private Text TextFragment { get { return (Text) Fragment; }}

        public string Text
        {
            set { TextFragment.TextValue = value; }
        }

        public override string ProfileLabel()
        {
            return "Text";
        }
    }
}