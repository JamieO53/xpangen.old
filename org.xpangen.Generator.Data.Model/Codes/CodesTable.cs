// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Codes
{
    /// <summary>
    /// A table of codes that belong together
    /// </summary>
    public class CodesTable : GenNamedApplicationBase
    {
        public CodesTable()
        {
        }

        public CodesTable(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// The name of the code
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
        /// Description of the codes table
        /// </summary>
        public string Title
        {
            get { return AsString("Title"); }
            set
            {
                if (Title == value) return;
                SetString("Title", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<Code> CodeList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            CodeList = new GenNamedApplicationList<Code>(this);
        }

        public Code AddCode(string name, string description = "", string value = "")
        {
            var item = new Code(GenData)
                           {
                               GenObject = GenData.CreateObject("CodesTable", "Code"),
                               Name = name,
                               Description = description,
                               Value = value
                           };
            CodeList.Add(item);
            return item;
        }
    }
}