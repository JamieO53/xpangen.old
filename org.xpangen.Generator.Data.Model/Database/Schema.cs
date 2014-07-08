// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

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
            Properties.Add("Name");
            Properties.Add("SchemaName");
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
            ObjectList = new GenNamedApplicationList<Object>(this);
        }
    }
}
