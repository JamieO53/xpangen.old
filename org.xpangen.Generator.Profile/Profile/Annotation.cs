// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile.Profile
{
    /// <summary>
    /// The Annotation fragment data
    /// </summary>
    public class Annotation : ContainerFragment
    {
        public Annotation()
        {
            Properties.Add("Name");
        }

        public Annotation(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        /// <summary>
        /// Generated name of the fragment
        /// </summary>
        public override string Name
        {
            get { return AsString("Name"); }
            set
            {
                if (Name == value) return;
                SetString("Name", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}
