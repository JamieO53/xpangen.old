// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Index key column reference
    /// </summary>
    public class KeyColumn : GenNamedApplicationBase
    {
        public KeyColumn()
        {
        }

        public KeyColumn(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// The key column name
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

        /// <summary>
        /// The order of the column in the key
        /// </summary>
        public string Order
        {
            get { return AsString("Order"); }
            set
            {
                if (Order == value) return;
                SetString("Order", value);
                if (!DelayedSave) SaveFields();
            }
        }

    }
}
