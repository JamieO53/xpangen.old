// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Classes allowing access to codes tables
    /// </summary>
    public class DatabaseDefinition : GenApplicationBase
    {
        public DatabaseDefinition(): this(new GenData(GetDefinition()))
        {
        }

        public DatabaseDefinition(GenData genData)
        {
            GenData = genData;
            base.GenObject = genData.Root;
		}

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.Definition = "DatabaseDefinition";
            f.AddSubClass("", "Database");
            f.AddSubClass("Database", "Schema");
            f.AddSubClass("Database", "Table");
            f.AddSubClass("Table", "Column");
            f.AddSubClass("Table", "Index");
            f.AddSubClass("Table", "ForeignKey");
            f.AddSubClass("Column", "Default");
            f.AddSubClass("Index", "KeyColumn");
            f.AddSubClass("Index", "DataColumn");
            f.AddSubClass("ForeignKey", "ForeignKeyColumn");
            f.Classes[1].Properties.Add("Name");
            f.Classes[2].Properties.Add("Name");
            f.Classes[3].Properties.Add("Name");
            f.Classes[3].Properties.Add("Schema");
            f.Classes[4].Properties.Add("Name");
            f.Classes[4].Properties.Add("NativeDataType");
            f.Classes[4].Properties.Add("ODBCDataType");
            f.Classes[4].Properties.Add("Length");
            f.Classes[4].Properties.Add("Precison");
            f.Classes[4].Properties.Add("Scale");
            f.Classes[4].Properties.Add("IsNullable");
            f.Classes[4].Properties.Add("IsKey");
            f.Classes[5].Properties.Add("Name");
            f.Classes[5].Properties.Add("Value");
            f.Classes[6].Properties.Add("Name");
            f.Classes[6].Properties.Add("IsPrimaryKey");
            f.Classes[6].Properties.Add("IsUnique");
            f.Classes[6].Properties.Add("IsClusterKey");
            f.Classes[7].Properties.Add("Name");
            f.Classes[7].Properties.Add("Order");
            f.Classes[8].Properties.Add("Name");
            f.Classes[9].Properties.Add("Name");
            f.Classes[9].Properties.Add("ReferenceTable");
            f.Classes[9].Properties.Add("DeleteAction");
            f.Classes[9].Properties.Add("UpdateAction");
            f.Classes[10].Properties.Add("Name");
            f.Classes[10].Properties.Add("RelatedColumn");
            return f;
        }

        public GenNamedApplicationList<Database> DatabaseList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            DatabaseList = new GenNamedApplicationList<Database>(this);
        }

        public Database AddDatabase(string name)
        {
            var item = new Database(GenData)
                           {
                               GenObject = GenData.CreateObject("", "Database"),
                               Name = name
                           };
            DatabaseList.Add(item);
            return item;
        }
    }
}
