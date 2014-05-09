// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

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
            base.GenObject = genData.Root;
		}

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.Definition = "CodesDefinition";
            f.AddSubClass("", "CodesTable");
            f.AddSubClass("CodesTable", "Code");
            f.Classes[1].Properties.Add("Name");
            f.Classes[1].Properties.Add("Title");
            f.Classes[2].Properties.Add("Name");
            f.Classes[2].Properties.Add("Description");
            f.Classes[2].Properties.Add("Value");
            return f;
        }

        public GenNamedApplicationList<CodesTable> CodesTableList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            CodesTableList = new GenNamedApplicationList<CodesTable>(this);
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
