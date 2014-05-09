// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database definition
    /// </summary>
    public class Database : GenNamedApplicationBase
    {
        public Database()
        {
        }

        public Database(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// Database name
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

        public GenNamedApplicationList<Schema> SchemaList { get; private set; }
        public GenNamedApplicationList<Table> TableList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            SchemaList = new GenNamedApplicationList<Schema>(this);
            TableList = new GenNamedApplicationList<Table>(this);
        }

        public Schema AddSchema(string name)
        {
            var item = new Schema(GenData)
                           {
                               GenObject = GenData.CreateObject("Database", "Schema"),
                               Name = name
                           };
            SchemaList.Add(item);
            return item;
        }


        public Table AddTable(string name, string schema = "")
        {
            var item = new Table(GenData)
                           {
                               GenObject = GenData.CreateObject("Database", "Table"),
                               Name = name,
                               Schema = schema
                           };
            TableList.Add(item);
            return item;
        }
    }
}
