// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// View definition
    /// </summary>
    public class View : GenNamedApplicationBase
    {
        public View()
        {
        }

        public View(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// View name
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
        /// The view name as on the database
        /// </summary>
        public string ViewName
        {
            get { return AsString("ViewName"); }
            set
            {
                if (ViewName == value) return;
                SetString("ViewName", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<Column> ColumnList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            ColumnList = new GenNamedApplicationList<Column>(this);
        }

        public Column AddColumn(string name, string columnName = "", string nativeDataType = "", string oDBCDataType = "", string length = "", string precision = "", string scale = "", string isNullable = "", string isKey = "")
        {
            var item = new Column(GenData)
                           {
                               GenObject = GenData.CreateObject("View", "Column"),
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

    }
}
