// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Defines the column relationship
    /// </summary>
    public class ForeignKeyColumn : GenNamedApplicationBase
    {
        public ForeignKeyColumn()
        {
            Properties.Add("Name");
            Properties.Add("RelatedColumn");
        }

        public ForeignKeyColumn(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        /// <summary>
        /// The column name
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
        /// The name of the referenced column
        /// </summary>
        public string RelatedColumn
        {
            get { return AsString("RelatedColumn"); }
            set
            {
                if (RelatedColumn == value) return;
                SetString("RelatedColumn", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}
