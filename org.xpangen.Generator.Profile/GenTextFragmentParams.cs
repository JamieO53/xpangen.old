// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenTextFragmentParams : GenFragmentParams
    {
        public string Text { get { return ((Text) Fragment).TextValue; } }

        /// <summary>
        ///     Parameters for creating a GenFragment
        /// </summary>
        /// <param name="genDataDef">The definition of the data being generated.</param>
        /// <param name="parentContainer">The container fragment conataining this fragment.</param>
        /// <param name="text">The text to be generated.</param>
        /// <param name="isPrimary">The fragment is part of the primary part of the body.</param>
        public GenTextFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentContainer, string text, bool isPrimary = true) 
            : base(genDataDef, parentContainer, FragmentType.Text, isPrimary: isPrimary)
        {
            ((Text) Fragment).TextValue = text;
        }
    }
}