// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Codes
{
    /// <summary>
    /// Classes allowing access to codes tables
    /// </summary>
    public class CodesDefinition : GenApplicationBase
    {
        public CodesDefinition(): this(new GenData(GetDefinition()))
        {
        }

        public CodesDefinition(GenData genData)
        {
            GenData = genData;
            Classes.Add("CodesTable");
            Classes.Add("Code");
            SubClasses.Add("CodesTable");
            GenObject = genData.Root;
        }

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.DefinitionName = "CodesDefinition";
            f.AddSubClass("", "CodesTable");
            f.AddSubClass("CodesTable", "Code");
            f.Classes[1].AddInstanceProperty("Name");
            f.Classes[1].AddInstanceProperty("Title");
            f.Classes[2].AddInstanceProperty("Name");
            f.Classes[2].AddInstanceProperty("Description");
            f.Classes[2].AddInstanceProperty("Value");
            return f;
        }

        public GenNamedApplicationList<CodesTable> CodesTableList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            CodesTableList = new GenNamedApplicationList<CodesTable>(this, 1, 0);
        }

        public CodesTable AddCodesTable(string name, string title = "")
        {
            var item = new CodesTable(GenData)
                           {
                               GenObject = GenData.CreateObject("", "CodesTable"),
                               Name = name,
                               Title = title
                           };
            CodesTableList.Add(item);
            return item;
        }
    }
}
