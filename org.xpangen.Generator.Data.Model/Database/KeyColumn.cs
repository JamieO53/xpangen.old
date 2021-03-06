// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Index key column reference
    /// </summary>
    public class KeyColumn : GenNamedApplicationBase
    {
        public KeyColumn()
        {
            Properties.Add("Name");
            Properties.Add("Order");
        }

        public KeyColumn(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
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
