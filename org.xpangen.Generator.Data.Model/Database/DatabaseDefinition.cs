// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
            f.AddSubClass("Schema", "Table");
            f.AddSubClass("Schema", "Procedure");
            f.AddSubClass("Schema", "Function");
            f.AddSubClass("Schema", "View");
            f.AddSubClass("Table", "Column");
            f.AddSubClass("Table", "Index");
            f.AddSubClass("Table", "ForeignKey");
            f.AddSubClass("View", "Column");
            f.AddSubClass("Procedure", "Parameter");
            f.AddSubClass("Function", "Parameter");
            f.AddSubClass("Function", "Column");
            f.AddSubClass("Column", "Default");
            f.AddSubClass("Index", "KeyColumn");
            f.AddSubClass("Index", "DataColumn");
            f.AddSubClass("ForeignKey", "ForeignKeyColumn");
            f.Classes[1].InstanceProperties.Add("Name");
            f.Classes[2].InstanceProperties.Add("Name");
            f.Classes[2].InstanceProperties.Add("SchemaName");
            f.Classes[3].InstanceProperties.Add("Name");
            f.Classes[3].InstanceProperties.Add("TableName");
            f.Classes[4].InstanceProperties.Add("Name");
            f.Classes[4].InstanceProperties.Add("ViewName");
            f.Classes[5].InstanceProperties.Add("Name");
            f.Classes[5].InstanceProperties.Add("ProcedureName");
            f.Classes[6].InstanceProperties.Add("Name");
            f.Classes[6].InstanceProperties.Add("FunctionName");
            f.Classes[7].InstanceProperties.Add("Name");
            f.Classes[7].InstanceProperties.Add("ColumnName");
            f.Classes[7].InstanceProperties.Add("NativeDataType");
            f.Classes[7].InstanceProperties.Add("ODBCDataType");
            f.Classes[7].InstanceProperties.Add("Length");
            f.Classes[7].InstanceProperties.Add("Precision");
            f.Classes[7].InstanceProperties.Add("Scale");
            f.Classes[7].InstanceProperties.Add("IsNullable");
            f.Classes[7].InstanceProperties.Add("IsKey");
            f.Classes[8].InstanceProperties.Add("Name");
            f.Classes[8].InstanceProperties.Add("Value");
            f.Classes[9].InstanceProperties.Add("Name");
            f.Classes[9].InstanceProperties.Add("IsPrimaryKey");
            f.Classes[9].InstanceProperties.Add("IsUnique");
            f.Classes[9].InstanceProperties.Add("IsClusterKey");
            f.Classes[10].InstanceProperties.Add("Name");
            f.Classes[10].InstanceProperties.Add("Order");
            f.Classes[11].InstanceProperties.Add("Name");
            f.Classes[12].InstanceProperties.Add("Name");
            f.Classes[12].InstanceProperties.Add("ReferenceTable");
            f.Classes[12].InstanceProperties.Add("DeleteAction");
            f.Classes[12].InstanceProperties.Add("UpdateAction");
            f.Classes[13].InstanceProperties.Add("Name");
            f.Classes[13].InstanceProperties.Add("RelatedColumn");
            f.Classes[14].InstanceProperties.Add("Name");
            f.Classes[14].InstanceProperties.Add("ParameterName");
            f.Classes[14].InstanceProperties.Add("NativeDataType");
            f.Classes[14].InstanceProperties.Add("ODBCDataType");
            f.Classes[14].InstanceProperties.Add("Length");
            f.Classes[14].InstanceProperties.Add("Precision");
            f.Classes[14].InstanceProperties.Add("Scale");
            f.Classes[14].InstanceProperties.Add("IsNullable");
            f.Classes[14].InstanceProperties.Add("Direction");
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
