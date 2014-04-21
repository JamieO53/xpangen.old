// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Model
{
    /// <summary>
    /// Generator Data Root
    /// </summary>
    public class Root : GenApplicationBase
    {
        public Root(GenData genData) : base(genData)
        {
        }

        public GenNamedApplicationList<GenSettings> GenSettingsList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            GenSettingsList = new GenNamedApplicationList<GenSettings>();
            var classId = GenDataDef.Classes.IndexOf("GenSettings");
            var classIdx = GenDataDef.IndexOfSubClass(0, classId);
            if (classIdx != -1)
            {
                var list = new GenObjectList(GenObject.SubClass[classIdx], GenObject.GenDataBase);
                list.First();
                while (!list.Eol)
                {
                    GenSettingsList.Add(new GenSettings(GenData) {GenObject = list.GenObject});
                    list.Next();
                }
            }

        }

        public GenSettings AddGenSettings(string homeDir)
        {
            var item = new GenSettings(GenData)
                           {
                               GenObject = GenData.CreateObject("", "GenSettings"),
                               HomeDir = homeDir
                           };
            GenSettingsList.Add(item);
            return item;
        }
    }
}
