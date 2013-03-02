// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public abstract class GenContainerFragmentBase : GenFragment
    {
        protected GenContainerFragmentBase(GenDataDef genDataDef, GenContainerFragmentBase parentSegment)
            : base(genDataDef, parentSegment)
        {
            GenDataDef = genDataDef;
            ParentSegement = parentSegment;
            Body = new GenSegBody(genDataDef, parentSegment) {ParentSegement = parentSegment};
        }

        public GenSegBody Body { get; private set; }

        public int ClassId
        {
            get
            {
                return ParentSegement is GenSegment
                           ? ((GenSegment) ParentSegement).ClassId
                           : (ParentSegement is GenLookup ? ((GenLookup) ParentSegement).ClassId : -1);
            }
        }
    }
}