// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database object
    /// </summary>
    public class Object : GenNamedApplicationBase
    {
        public Object()
        {
            SubClasses.Add("Table");
            SubClasses.Add("View");
            SubClasses.Add("Procedure");
            SubClasses.Add("Function");
            SubClasses.Add("Column");
            SubClasses.Add("Index");
            SubClasses.Add("ForeignKey");
            SubClasses.Add("Parameter");
            Properties.Add("Name");
        }

        public Object(GenData genData) : this()
        {
            GenData = genData;
        }

        public GenNamedApplicationList<Table> TableList { get; private set; }
        public GenNamedApplicationList<View> ViewList { get; private set; }
        public GenNamedApplicationList<Procedure> ProcedureList { get; private set; }
        public GenNamedApplicationList<Function> FunctionList { get; private set; }
        public GenNamedApplicationList<Column> ColumnList { get; private set; }
        public GenNamedApplicationList<Index> IndexList { get; private set; }
        public GenNamedApplicationList<ForeignKey> ForeignKeyList { get; private set; }
        public GenNamedApplicationList<Parameter> ParameterList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            TableList = new GenNamedApplicationList<Table>(this, 4, 0);
            base.GenObjectSetNotification();
            ViewList = new GenNamedApplicationList<View>(this, 5, 1);
            base.GenObjectSetNotification();
            ProcedureList = new GenNamedApplicationList<Procedure>(this, 6, 2);
            base.GenObjectSetNotification();
            FunctionList = new GenNamedApplicationList<Function>(this, 7, 3);
            base.GenObjectSetNotification();
            ColumnList = new GenNamedApplicationList<Column>(this, 8, 4);
            base.GenObjectSetNotification();
            IndexList = new GenNamedApplicationList<Index>(this, 10, 5);
            base.GenObjectSetNotification();
            ForeignKeyList = new GenNamedApplicationList<ForeignKey>(this, 13, 6);
            base.GenObjectSetNotification();
            ParameterList = new GenNamedApplicationList<Parameter>(this, 15, 7);
        }

        public Table AddTable(string name, string tableName = "")
        {
            var item = new Table(GenData)
                           {
                               GenObject = GenData.CreateObject("Object", "Table"),
                               Name = name,
                               TableName = tableName
                           };
            TableList.Add(item);
            return item;
        }


        public View AddView(string name, string viewName = "")
        {
            var item = new View(GenData)
                           {
                               GenObject = GenData.CreateObject("Object", "View"),
                               Name = name,
                               ViewName = viewName
                           };
            ViewList.Add(item);
            return item;
        }


        public Procedure AddProcedure(string name, string procedureName = "")
        {
            var item = new Procedure(GenData)
                           {
                               GenObject = GenData.CreateObject("Object", "Procedure"),
                               Name = name,
                               ProcedureName = procedureName
                           };
            ProcedureList.Add(item);
            return item;
        }


        public Function AddFunction(string name, string functionName = "")
        {
            var item = new Function(GenData)
                           {
                               GenObject = GenData.CreateObject("Object", "Function"),
                               Name = name,
                               FunctionName = functionName
                           };
            FunctionList.Add(item);
            return item;
        }


        public Column AddColumn(string name, string columnName = "", string nativeDataType = "", string oDBCDataType = "", string length = "", string precision = "", string scale = "", string isNullable = "", string isKey = "", string isIdentity = "")
        {
            var item = new Column(GenData)
                           {
                               GenObject = GenData.CreateObject("Object", "Column"),
                               Name = name,
                               ColumnName = columnName,
                               NativeDataType = nativeDataType,
                               ODBCDataType = oDBCDataType,
                               Length = length,
                               Precision = precision,
                               Scale = scale,
                               IsNullable = isNullable,
                               IsKey = isKey,
                               IsIdentity = isIdentity
                           };
            ColumnList.Add(item);
            return item;
        }


        public Index AddIndex(string name, string isPrimaryKey = "", string isUnique = "", string isClusterKey = "")
        {
            var item = new Index(GenData)
                           {
                               GenObject = GenData.CreateObject("Object", "Index"),
                               Name = name,
                               IsPrimaryKey = isPrimaryKey,
                               IsUnique = isUnique,
                               IsClusterKey = isClusterKey
                           };
            IndexList.Add(item);
            return item;
        }


        public ForeignKey AddForeignKey(string name, string referenceSchema = "", string referenceTable = "", string deleteAction = "", string updateAction = "")
        {
            var item = new ForeignKey(GenData)
                           {
                               GenObject = GenData.CreateObject("Object", "ForeignKey"),
                               Name = name,
                               ReferenceSchema = referenceSchema,
                               ReferenceTable = referenceTable,
                               DeleteAction = deleteAction,
                               UpdateAction = updateAction
                           };
            ForeignKeyList.Add(item);
            return item;
        }


        public Parameter AddParameter(string name, string parameterName = "", string nativeDataType = "", string oDBCDataType = "", string length = "", string precision = "", string scale = "", string isNullable = "", string direction = "")
        {
            var item = new Parameter(GenData)
                           {
                               GenObject = GenData.CreateObject("Object", "Parameter"),
                               Name = name,
                               ParameterName = parameterName,
                               NativeDataType = nativeDataType,
                               ODBCDataType = oDBCDataType,
                               Length = length,
                               Precision = precision,
                               Scale = scale,
                               IsNullable = isNullable,
                               Direction = direction
                           };
            ParameterList.Add(item);
            return item;
        }

    }
}
