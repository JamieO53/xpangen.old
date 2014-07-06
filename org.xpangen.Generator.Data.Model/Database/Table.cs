// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database table
    /// </summary>
    public class Table : GenNamedApplicationBase
    {
        public Table()
        {
        }

        public Table(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// Database table name
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
        /// The table name as on the database
        /// </summary>
        public string TableName
        {
            get { return AsString("TableName"); }
            set
            {
                if (TableName == value) return;
                SetString("TableName", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<Column> ColumnList { get; private set; }
        public GenNamedApplicationList<Index> IndexList { get; private set; }
        public GenNamedApplicationList<ForeignKey> ForeignKeyList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            ColumnList = new GenNamedApplicationList<Column>(this);
            IndexList = new GenNamedApplicationList<Index>(this);
            ForeignKeyList = new GenNamedApplicationList<ForeignKey>(this);
        }

        public Column AddColumn(string name, string columnName = "", string nativeDataType = "", string oDBCDataType = "", string length = "", string precision = "", string scale = "", string isNullable = "", string isKey = "")
        {
            var item = new Column(GenData)
                           {
                               GenObject = GenData.CreateObject("Table", "Column"),
                               Name = name,
                               ColumnName = columnName,
                               NativeDataType = nativeDataType,
                               ODBCDataType = oDBCDataType,
                               Length = length,
                               Precision = precision,
                               Scale = scale,
                               IsNullable = isNullable,
                               IsKey = isKey
                           };
            ColumnList.Add(item);
            return item;
        }


        public Index AddIndex(string name, string isPrimaryKey = "", string isUnique = "", string isClusterKey = "")
        {
            var item = new Index(GenData)
                           {
                               GenObject = GenData.CreateObject("Table", "Index"),
                               Name = name,
                               IsPrimaryKey = isPrimaryKey,
                               IsUnique = isUnique,
                               IsClusterKey = isClusterKey
                           };
            IndexList.Add(item);
            return item;
        }


        public ForeignKey AddForeignKey(string name, string referenceTable = "", string deleteAction = "", string updateAction = "")
        {
            var item = new ForeignKey(GenData)
                           {
                               GenObject = GenData.CreateObject("Table", "ForeignKey"),
                               Name = name,
                               ReferenceTable = referenceTable,
                               DeleteAction = deleteAction,
                               UpdateAction = updateAction
                           };
            ForeignKeyList.Add(item);
            return item;
        }

    }
}
