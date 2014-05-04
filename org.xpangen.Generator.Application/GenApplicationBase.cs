// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Application
{
    public class GenApplicationBase: GenAttributes
    {
        private GenData _genData;

        protected GenApplicationBase()
        {
            DelayedSave = false;
        }

        protected virtual void GenObjectSetNotification()
        {
            
        }

        public GenData GenData
        {
            get { return _genData; }
            set
            {
                GenDataDef = value.GenDataDef;
                _genData = value;
            }
        }

        public bool DelayedSave { get; set; }
        
        public override GenObject GenObject
        {
            get
            {
                return base.GenObject;
            }
            set
            {
                if (base.GenObject == value) return;
                base.GenObject = value;
                GenObjectSetNotification();
            }
        }
    }
}
