// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Column default constraint
    /// </summary>
    public class Default : GenNamedApplicationBase
    {
        public Default()
        {
            Properties.Add("Name");
            Properties.Add("Value");
        }

        public Default(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        /// <summary>
        /// The default value - quoted if required
        /// </summary>
        public string Value
        {
            get { return AsString("Value"); }
            set
            {
                if (Value == value) return;
                SetString("Value", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}
