// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database schema definition
    /// </summary>
    public class Schema : GenNamedApplicationBase
    {
        public Schema()
        {
        }

        public Schema(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// Database schema name
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
        /// The schema name as on the database
        /// </summary>
        public string SchemaName
        {
            get { return AsString("SchemaName"); }
            set
            {
                if (SchemaName == value) return;
                SetString("SchemaName", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<Table> TableList { get; private set; }
        public GenNamedApplicationList<Procedure> ProcedureList { get; private set; }
        public GenNamedApplicationList<Function> FunctionList { get; private set; }
        public GenNamedApplicationList<View> ViewList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            TableList = new GenNamedApplicationList<Table>(this);
            ProcedureList = new GenNamedApplicationList<Procedure>(this);
            FunctionList = new GenNamedApplicationList<Function>(this);
            ViewList = new GenNamedApplicationList<View>(this);
        }

        public Table AddTable(string name, string tableName = "")
        {
            var item = new Table(GenData)
                           {
                               GenObject = GenData.CreateObject("Schema", "Table"),
                               Name = name,
                               TableName = tableName
                           };
            TableList.Add(item);
            return item;
        }


        public Procedure AddProcedure(string name, string procedureName = "")
        {
            var item = new Procedure(GenData)
                           {
                               GenObject = GenData.CreateObject("Schema", "Procedure"),
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
                               GenObject = GenData.CreateObject("Schema", "Function"),
                               Name = name,
                               FunctionName = functionName
                           };
            FunctionList.Add(item);
            return item;
        }


        public View AddView(string name, string viewName = "")
        {
            var item = new View(GenData)
                           {
                               GenObject = GenData.CreateObject("Schema", "View"),
                               Name = name,
                               ViewName = viewName
                           };
            ViewList.Add(item);
            return item;
        }

    }
}
