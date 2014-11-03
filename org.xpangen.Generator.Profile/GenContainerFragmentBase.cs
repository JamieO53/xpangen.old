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
            ParentContainer = genFragmentParams.ParentContainer;
            Body = new GenSegBody(genFragmentParams.ParentContainer, this);
        }

        public new GenObject GenObject
        {
            set { base.GenObject = value; }
        }

        public GenSegBody Body { get; private set; }

        public int ClassId
        {
            get
            {
                if (_classId != -1)
                    return _classId;
                var classId = Fragment.ClassId;
                if (classId == -1) return 0;
                return classId;
            }
            protected set { _classId = value; }
        }

        public GenDataDefClass Definition
        {
            get { return GenDataDef.GetClassDef(ClassId); }
        }
    }
}