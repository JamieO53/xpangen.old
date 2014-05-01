// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Codes
{
    /// <summary>
    /// Generator Data Root
    /// </summary>
    public class Root : GenApplicationBase
    {
        public Root(GenData genData) : base(genData)
        {
            base.GenObject = genData.Root;
            genData.GenDataBase.RaiseDataChanged("", "");
        }

        public GenNamedApplicationList<CodesTable> CodesTableList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            CodesTableList = new GenNamedApplicationList<CodesTable>();
            var classId = GenDataDef.Classes.IndexOf("CodesTable");
            var classIdx = GenDataDef.IndexOfSubClass(0, classId);
            if (classIdx != -1)
            {
                var list = new GenObjectList(GenObject.SubClass[classIdx], GenObject.GenDataBase);
                list.First();
                while (!list.Eol)
                {
                    CodesTableList.Add(new CodesTable(GenData) {GenObject = list.GenObject});
                    list.Next();
                }
            }
        }

        public CodesTable AddCodesTable(string Name, string Title)
        {
            var item = new CodesTable(GenData)
                           {
                               GenObject = GenData.CreateObject("", "CodesTable"),
                               Name = Name,
                               Title = Title
                           };
            CodesTableList.Add(item);
            return item;
        }
    }
}
