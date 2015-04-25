// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database definition
    /// </summary>
    public class Database : GenNamedApplicationBase
    {
        public Database()
        {
            SubClasses.Add("Schema");
            Properties.Add("Name");
        }

        public Database(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        public GenNamedApplicationList<Schema> SchemaList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            SchemaList = new GenNamedApplicationList<Schema>(this, 2, 0);
        }

        public Schema AddSchema(string name, string schemaName = "")
        {
            var item = new Schema(GenDataBase)
                           {
                               GenObject = ((GenObject) GenObject).CreateGenObject("Schema"),
                               Name = name,
                               SchemaName = schemaName
                           };
            SchemaList.Add(item);
            return item;
        }

    }
}
