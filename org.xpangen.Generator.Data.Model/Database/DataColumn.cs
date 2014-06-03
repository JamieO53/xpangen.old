// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Index data column reference
    /// </summary>
    public class DataColumn : GenNamedApplicationBase
    {
        public DataColumn()
        {
        }

        public DataColumn(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// The name of the data column
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
