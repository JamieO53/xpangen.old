// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Index data column reference
    /// </summary>
    public class DataColumn : GenNamedApplicationBase
    {
        public DataColumn()
        {
            Properties.Add("Name");
        }

        public DataColumn(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }


    }
}
