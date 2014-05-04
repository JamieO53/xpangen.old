// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Definition
{
    /// <summary>
    /// Generator Data Root
    /// </summary>
    public class Root : GenApplicationBase
    {
        public Root(GenData genData)
        {
             GenData = genData;
			 base.GenObject = genData.Root;
		}

        public GenNamedApplicationList<Class> ClassList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            ClassList = new GenNamedApplicationList<Class>(this);
        }

        public Class AddClass(string Name, string Title)
        {
            var item = new Class(GenData)
                           {
                               GenObject = GenData.CreateObject("", "Class"),
                               Name = Name,
                               Title = Title
                           };
            ClassList.Add(item);
            return item;
        }
    }
}
