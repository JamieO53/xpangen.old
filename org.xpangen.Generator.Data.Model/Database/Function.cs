// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database function definition
    /// </summary>
    public class Function : GenNamedApplicationBase
    {
        public Function()
        {
        }

        public Function(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// Function name
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
        /// The function name as on the database
        /// </summary>
        public string FunctionName
        {
            get { return AsString("FunctionName"); }
            set
            {
                if (FunctionName == value) return;
                SetString("FunctionName", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<Parameter> ParameterList { get; private set; }
        public GenNamedApplicationList<Column> ColumnList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            ParameterList = new GenNamedApplicationList<Parameter>(this);
            ColumnList = new GenNamedApplicationList<Column>(this);
        }

        public Parameter AddParameter(string name, string parameterName = "", string nativeDataType = "", string oDBCDataType = "", string length = "", string precision = "", string scale = "", string isNullable = "", string direction = "")
        {
            var item = new Parameter(GenData)
                           {
                               GenObject = GenData.CreateObject("Function", "Parameter"),
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


        public Column AddColumn(string name, string columnName = "", string nativeDataType = "", string oDBCDataType = "", string length = "", string precision = "", string scale = "", string isNullable = "", string isKey = "")
        {
            var item = new Column(GenData)
                           {
                               GenObject = GenData.CreateObject("Function", "Column"),
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
