// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    public class GenNamedApplicationBase: GenApplicationBase
    {
        /// <summary>
        /// Object name: must be well formed
        /// </summary>
        public virtual string Name
        {
            get { return AsString("Name"); }
            set
            {
                if (Name == value) return;
                SetString("Name", value);
                if (!DelayedSave) SaveFields();
                if (List != null) ((IGenNamedApplicationList)List).NameChanged(this);
            }
        }

        public override string ToString()
        {
            return GetType().Name + "." + Name;
        }
    }
}
