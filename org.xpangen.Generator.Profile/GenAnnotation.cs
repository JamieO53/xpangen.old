// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenAnnotation : GenContainerFragmentBase
    {
        public GenAnnotation(GenFragmentParams genFragmentParams)
            : base(genFragmentParams.SetFragmentType(FragmentType.Annotation))
        {
            Annotation = (Annotation) Fragment;
        }

        public Annotation Annotation { get; private set; }
    }
}