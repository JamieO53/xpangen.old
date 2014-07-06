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
        }

        public Default(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// The default constraint name
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
