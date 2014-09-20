// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenSegmentParams : GenFragmentParams
    {
        private readonly string _className;
        private readonly GenCardinality _cardinality;

        public GenSegmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentContainer, string className, GenCardinality cardinality, bool isPrimary = true)
            : base(genDataDef, parentContainer, isPrimary: isPrimary)
        {
            _className = className;
            _cardinality = cardinality;
        }

        public string ClassName
        {
            get { return _className; }
        }

        public GenCardinality Cardinality
        {
            get { return _cardinality; }
        }
    }
}