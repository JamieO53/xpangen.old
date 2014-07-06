// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Minimal
{
    /// <summary>
    /// Property definition
    /// </summary>
    public class Property : GenNamedApplicationBase
    {
        public Property()
        {
        }

        public Property(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// Property name: must be well formed
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
