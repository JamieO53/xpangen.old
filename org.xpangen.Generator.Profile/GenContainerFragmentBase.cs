// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public abstract class GenContainerFragmentBase : GenFragment
    {
        private int _classId = -1;

        protected GenContainerFragmentBase(GenFragmentParams genFragmentParams)
            : base(genFragmentParams)
        {
            ParentSegment = genFragmentParams.ParentSegment;
            Body = new GenSegBody(genFragmentParams.ParentSegment, genFragmentParams.ParentContainer);
        }

        public new GenObject GenObject
        {
            get { return base.GenObject; }
            set
            {
                Body.GenObject = value;
                base.GenObject = value;
            }
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
                if (classId == -1) return 0;//Assert(classId != -1, "Unable to identify fragment class ID");
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