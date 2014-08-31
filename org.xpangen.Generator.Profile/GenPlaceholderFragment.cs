// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenPlaceholderFragment : GenFragment
    {
        public GenPlaceholderFragment(GenPlaceholderFragmentParams genPlaceholderFragmentParams) :
            base(genPlaceholderFragmentParams.SetFragmentType(FragmentType.Placeholder))
        {
            var placeholder = ((Placeholder) Fragment);
            placeholder.Class = genPlaceholderFragmentParams.Id.ClassName;
            placeholder.Property = genPlaceholderFragmentParams.Id.PropertyName;
        }
    }
}