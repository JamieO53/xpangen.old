// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database table
    /// </summary>
    public class Table : GenNamedApplicationBase
    {
        public Table()
        {
            Properties.Add("Name");
            Properties.Add("TableName");
        }

        public Table(GenData genData) : this()
        {
            GenData = genData;
        }

        /// <summary>
        /// Database table name
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
        /// The table name as on the database
        /// </summary>
        public string TableName
        {
            get { return AsString("TableName"); }
            set
            {
                if (TableName == value) return;
                SetString("TableName", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}
