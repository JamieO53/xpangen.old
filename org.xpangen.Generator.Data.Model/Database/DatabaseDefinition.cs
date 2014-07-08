// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

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
            f.AddSubClass("Schema", "Object");
            f.AddSubClass("Object", "Table");
            f.AddSubClass("Object", "View");
            f.AddSubClass("Object", "Procedure");
            f.AddSubClass("Object", "Function");
            f.AddSubClass("Object", "Column");
            f.AddSubClass("Object", "Index");
            f.AddSubClass("Object", "ForeignKey");
            f.AddSubClass("Object", "Parameter");
            f.AddSubClass("Column", "Default");
            f.AddSubClass("Index", "KeyColumn");
            f.AddSubClass("Index", "DataColumn");
            f.AddSubClass("ForeignKey", "ForeignKeyColumn");
            f.Classes[1].InstanceProperties.Add("Name");
            f.Classes[2].InstanceProperties.Add("Name");
            f.Classes[2].InstanceProperties.Add("SchemaName");
            f.Classes[3].InstanceProperties.Add("Name");
            f.Classes[4].InstanceProperties.Add("Name");
            f.Classes[4].InstanceProperties.Add("TableName");
            f.Classes[5].InstanceProperties.Add("Name");
            f.Classes[5].InstanceProperties.Add("ViewName");
            f.Classes[6].InstanceProperties.Add("Name");
            f.Classes[6].InstanceProperties.Add("ProcedureName");
            f.Classes[7].InstanceProperties.Add("Name");
            f.Classes[7].InstanceProperties.Add("FunctionName");
            f.Classes[8].InstanceProperties.Add("Name");
            f.Classes[8].InstanceProperties.Add("ColumnName");
            f.Classes[8].InstanceProperties.Add("NativeDataType");
            f.Classes[8].InstanceProperties.Add("ODBCDataType");
            f.Classes[8].InstanceProperties.Add("Length");
            f.Classes[8].InstanceProperties.Add("Precision");
            f.Classes[8].InstanceProperties.Add("Scale");
            f.Classes[8].InstanceProperties.Add("IsNullable");
            f.Classes[8].InstanceProperties.Add("IsKey");
            f.Classes[9].InstanceProperties.Add("Name");
            f.Classes[9].InstanceProperties.Add("Value");
            f.Classes[10].InstanceProperties.Add("Name");
            f.Classes[10].InstanceProperties.Add("IsPrimaryKey");
            f.Classes[10].InstanceProperties.Add("IsUnique");
            f.Classes[10].InstanceProperties.Add("IsClusterKey");
            f.Classes[11].InstanceProperties.Add("Name");
            f.Classes[11].InstanceProperties.Add("Order");
            f.Classes[12].InstanceProperties.Add("Name");
            f.Classes[13].InstanceProperties.Add("Name");
            f.Classes[13].InstanceProperties.Add("ReferenceTable");
            f.Classes[13].InstanceProperties.Add("DeleteAction");
            f.Classes[13].InstanceProperties.Add("UpdateAction");
            f.Classes[14].InstanceProperties.Add("Name");
            f.Classes[14].InstanceProperties.Add("RelatedColumn");
            f.Classes[15].InstanceProperties.Add("Name");
            f.Classes[15].InstanceProperties.Add("ParameterName");
            f.Classes[15].InstanceProperties.Add("NativeDataType");
            f.Classes[15].InstanceProperties.Add("ODBCDataType");
            f.Classes[15].InstanceProperties.Add("Length");
            f.Classes[15].InstanceProperties.Add("Precision");
            f.Classes[15].InstanceProperties.Add("Scale");
            f.Classes[15].InstanceProperties.Add("IsNullable");
            f.Classes[15].InstanceProperties.Add("Direction");
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
