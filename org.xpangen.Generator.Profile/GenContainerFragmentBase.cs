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
            GenContainerFragmentBase parentContainer, FragmentType fragmentType)
            : base(genDataDef, parentSegment, parentContainer, fragmentType)
        {
            ParentSegment = parentSegment;
            Body = new GenSegBody(parentSegment, parentContainer);
        }

        public GenSegBody Body { get; private set; }

        public int ClassId
        {
            get
            {
                if (_classId != -1)
                    return _classId;
                var classId = ParentSegment is GenSegment
                                  ? (ParentSegment).ClassId
                                  : (ParentSegment is GenLookup ? ((GenLookup) ParentSegment).ClassId : -1);
                Assert(classId != -1, "Unable to identify fragment class ID");
                return classId;
            }
            protected set { _classId = value; }
        }

        public GenDataDefClass Definition
        {
            get { return GenDataDef.Classes[ClassId]; }
        }
    }
}