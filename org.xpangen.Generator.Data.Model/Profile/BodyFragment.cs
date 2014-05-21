// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// A fragment that forms part of the body of a container fragment
    /// </summary>
    public class BodyFragment : GenNamedApplicationBase
    {
        public BodyFragment()
        {
        }

        public BodyFragment(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// The name of the body fragment
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
