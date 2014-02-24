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
        public Root(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        public GenApplicationList<GenSettings> GenSettingsList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            GenSettingsList = new GenApplicationList<GenSettings>();
            var classId = GenDataDef.Classes.IndexOf("GenSettings");
            var classIdx = GenDataDef.IndexOfSubClass(0, classId);
            if (classIdx != -1)
            {
                var list = new GenObjectList(GenObject.SubClass[classIdx]);
                list.First();
                while (!list.Eol)
                {
                    GenSettingsList.Add(new GenSettings(GenDataDef) {GenObject = list.GenObject});
                    list.Next();
                }
            }

        }
    }
}
