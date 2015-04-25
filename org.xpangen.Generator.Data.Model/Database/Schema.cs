// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database schema definition
    /// </summary>
    public class Schema : GenNamedApplicationBase
    {
        public Schema()
        {
            SubClasses.Add("Object");
            Properties.Add("Name");
            Properties.Add("SchemaName");
        }

        public Schema(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        /// <summary>
        /// The schema name as on the database
        /// </summary>
        public string SchemaName
        {
            get { return AsString("SchemaName"); }
            set
            {
                if (SchemaName == value) return;
                SetString("SchemaName", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<Object> ObjectList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            ObjectList = new GenNamedApplicationList<Object>(this, 3, 0);
        }
    }
}
