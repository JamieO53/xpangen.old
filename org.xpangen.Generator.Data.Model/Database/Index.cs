// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Table index
    /// </summary>
    public class Index : GenNamedApplicationBase
    {
        public Index()
        {
        }

        public Index(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// The name of the index
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
        /// Is this the table's primary key?
        /// </summary>
        public string IsPrimaryKey
        {
            get { return AsString("IsPrimaryKey"); }
            set
            {
                if (IsPrimaryKey == value) return;
                SetString("IsPrimaryKey", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// Is this a unique key?
        /// </summary>
        public string IsUnique
        {
            get { return AsString("IsUnique"); }
            set
            {
                if (IsUnique == value) return;
                SetString("IsUnique", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// Is this a cluster key?
        /// </summary>
        public string IsClusterKey
        {
            get { return AsString("IsClusterKey"); }
            set
            {
                if (IsClusterKey == value) return;
                SetString("IsClusterKey", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<KeyColumn> KeyColumnList { get; private set; }
        public GenNamedApplicationList<DataColumn> DataColumnList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            KeyColumnList = new GenNamedApplicationList<KeyColumn>(this);
            DataColumnList = new GenNamedApplicationList<DataColumn>(this);
        }

        public KeyColumn AddKeyColumn(string name, string order = "")
        {
            var item = new KeyColumn(GenData)
                           {
                               GenObject = GenData.CreateObject("Index", "KeyColumn"),
                               Name = name,
                               Order = order
                           };
            KeyColumnList.Add(item);
            return item;
        }


        public DataColumn AddDataColumn(string name)
        {
            var item = new DataColumn(GenData)
                           {
                               GenObject = GenData.CreateObject("Index", "DataColumn"),
                               Name = name
                           };
            DataColumnList.Add(item);
            return item;
        }

    }
}
