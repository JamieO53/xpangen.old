// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Profile;

namespace org.xpangen.Generator.Profile
{
    public abstract class GenContainerFragmentBase : GenFragment
    {
        private int _classId = -1;

        protected GenContainerFragmentBase(GenDataDef genDataDef, GenContainerFragmentBase parentSegment,
            FragmentType fragmentType, GenData genData = null, ProfileRoot profileRoot = null,
            GenObject genObject = null)
            : base(genDataDef, parentSegment, fragmentType, genData, profileRoot, genObject)
        {
            ParentSegment = parentSegment;
            Body = new GenSegBody(genDataDef, parentSegment, genData, profileRoot, genObject);
        }

        public GenSegBody Body { get; private set; }

        public new int ClassId
        {
            get
            {
                if (_classId != -1)
                    return _classId;
                var classId = ParentSegment is GenSegment
                                  ? (ParentSegment).ClassId
                                  : (ParentSegment is GenLookup ? ((GenLookup) ParentSegment).ClassId : -1);
                if (classId == -1)
                    throw new GeneratorException("Unable to identify fragment class ID", GenErrorType.Assertion);
                return classId;
            }
            protected set { _classId = value; }
        }

        public new GenDataDefClass Definition
        {
            get { return GenDataDef.Classes[ClassId]; }
        }
    }
}