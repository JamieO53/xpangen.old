﻿// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Profile
{
    public class GenTextBlock : GenContainerFragmentBase
    {
        public GenTextBlock(GenFragmentParams genFragmentParams)
            : base(genFragmentParams.SetFragmentType(FragmentType.TextBlock))
        {
        }

        public override string ProfileLabel()
        {
            return "Text";
        }
    }
}