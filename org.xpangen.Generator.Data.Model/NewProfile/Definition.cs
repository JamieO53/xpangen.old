// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// The definition of the data file being generated
    /// </summary>
    public class Definition : GenNamedApplicationBase
    {
        public Definition()
        {
        }

        public Definition(GenData genData)
        {
            GenData = genData;
            Properties.Add("Name");
            Properties.Add("Path");
        }

        /// <summary>
        /// The name of the definition
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
        /// The location of the definition
        /// </summary>
        public string Path
        {
            get { return AsString("Path"); }
            set
            {
                if (Path == value) return;
                SetString("Path", value);
                if (!DelayedSave) SaveFields();
            }
        }


        protected override void GenObjectSetNotification()
        {
        }
    }
}
