// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenApplicationBase: GenAttributes
    {
        private Dictionary<string, IList> _lists;


        protected GenApplicationBase()
        {
            DelayedSave = false;
        }

        protected virtual void GenObjectSetNotification()
        {
            
        }

        public GenData GenData { get; set; }

        public bool DelayedSave { get; set; }
        
        public override IGenObject GenObject
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

        public Dictionary<string, IList> Lists 
        {
            get { return _lists ?? (_lists = new Dictionary<string, IList>()); }
        }

        public GenApplicationBase Parent { get; set; }
    }
}
