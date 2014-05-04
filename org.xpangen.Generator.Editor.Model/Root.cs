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
        public Root(GenData genData) : base()
        {
             base.GenData = genData;
			 base.GenObject = genData.Root;
		}

        public GenNamedApplicationList<GenSettings> GenSettingsList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            GenSettingsList = new GenNamedApplicationList<GenSettings>(this);
        }

        public GenSettings AddGenSettings(string HomeDir)
        {
            var item = new GenSettings(GenData)
                           {
                               GenObject = GenData.CreateObject("", "GenSettings"),
                               HomeDir = HomeDir
                           };
            GenSettingsList.Add(item);
            return item;
        }
    }
}
