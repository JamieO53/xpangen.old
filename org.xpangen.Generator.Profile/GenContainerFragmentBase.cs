﻿// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public abstract class GenContainerFragmentBase : GenFragment
    {
        private int _classId = -1;

        protected GenContainerFragmentBase(GenDataDef genDataDef, GenContainerFragmentBase parentSegment,
            FragmentType fragmentType, GenObject genObject = null, GenData genData = null)
            : base(genDataDef, parentSegment, fragmentType, genObject, genData)
        {
            GenDataDef = genDataDef;
            ParentSegement = parentSegment;
            Body = new GenSegBody(genDataDef, parentSegment) {ParentSegement = parentSegment};
        }

        public GenSegBody Body { get; private set; }

        public new int ClassId
        {
            get
            {
                if (_classId != -1)
                    return _classId;
                return ParentSegement is GenSegment
                           ? (ParentSegement).ClassId
                           : (ParentSegement is GenLookup ? ((GenLookup) ParentSegement).ClassId : -1);
            }
            protected set { _classId = value; }
        }

        public new GenDataDefClass Definition
        {
            get { return GenDataDef.Classes[ClassId]; }
        }
    }
}