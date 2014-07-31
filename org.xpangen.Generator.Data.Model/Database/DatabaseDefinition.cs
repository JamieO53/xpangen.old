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
            Classes.Add("Database");
            Classes.Add("Schema");
            Classes.Add("Object");
            Classes.Add("Table");
            Classes.Add("View");
            Classes.Add("Procedure");
            Classes.Add("Function");
            Classes.Add("Column");
            Classes.Add("Default");
            Classes.Add("Index");
            Classes.Add("KeyColumn");
            Classes.Add("DataColumn");
            Classes.Add("ForeignKey");
            Classes.Add("ForeignKeyColumn");
            Classes.Add("Parameter");
            SubClasses.Add("Database");
            base.GenObject = genData.Root;
        }

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.DefinitionName = "DatabaseDefinition";
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
            f.Classes[1].AddInstanceProperty("Name");
            f.Classes[2].AddInstanceProperty("Name");
            f.Classes[2].AddInstanceProperty("SchemaName");
            f.Classes[3].AddInstanceProperty("Name");
            f.Classes[4].AddInstanceProperty("Name");
            f.Classes[4].AddInstanceProperty("TableName");
            f.Classes[5].AddInstanceProperty("Name");
            f.Classes[5].AddInstanceProperty("ViewName");
            f.Classes[6].AddInstanceProperty("Name");
            f.Classes[6].AddInstanceProperty("ProcedureName");
            f.Classes[7].AddInstanceProperty("Name");
            f.Classes[7].AddInstanceProperty("FunctionName");
            f.Classes[8].AddInstanceProperty("Name");
            f.Classes[8].AddInstanceProperty("ColumnName");
            f.Classes[8].AddInstanceProperty("NativeDataType");
            f.Classes[8].AddInstanceProperty("ODBCDataType");
            f.Classes[8].AddInstanceProperty("Length");
            f.Classes[8].AddInstanceProperty("Precision");
            f.Classes[8].AddInstanceProperty("Scale");
            f.Classes[8].AddInstanceProperty("IsNullable");
            f.Classes[8].AddInstanceProperty("IsKey");
            f.Classes[9].AddInstanceProperty("Name");
            f.Classes[9].AddInstanceProperty("Value");
            f.Classes[10].AddInstanceProperty("Name");
            f.Classes[10].AddInstanceProperty("IsPrimaryKey");
            f.Classes[10].AddInstanceProperty("IsUnique");
            f.Classes[10].AddInstanceProperty("IsClusterKey");
            f.Classes[11].AddInstanceProperty("Name");
            f.Classes[11].AddInstanceProperty("Order");
            f.Classes[12].AddInstanceProperty("Name");
            f.Classes[13].AddInstanceProperty("Name");
            f.Classes[13].AddInstanceProperty("ReferenceTable");
            f.Classes[13].AddInstanceProperty("DeleteAction");
            f.Classes[13].AddInstanceProperty("UpdateAction");
            f.Classes[14].AddInstanceProperty("Name");
            f.Classes[14].AddInstanceProperty("RelatedColumn");
            f.Classes[15].AddInstanceProperty("Name");
            f.Classes[15].AddInstanceProperty("ParameterName");
            f.Classes[15].AddInstanceProperty("NativeDataType");
            f.Classes[15].AddInstanceProperty("ODBCDataType");
            f.Classes[15].AddInstanceProperty("Length");
            f.Classes[15].AddInstanceProperty("Precision");
            f.Classes[15].AddInstanceProperty("Scale");
            f.Classes[15].AddInstanceProperty("IsNullable");
            f.Classes[15].AddInstanceProperty("Direction");
            return f;
        }

        public GenNamedApplicationList<Database> DatabaseList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            DatabaseList = new GenNamedApplicationList<Database>(this, 1, 0);
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
