// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database schema definition
    /// </summary>
    public class Schema : GenNamedApplicationBase
    {
        public Schema()
        {
        }

        public Schema(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// Database schema name
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
