// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    public struct GenDataId
    {
        public int ClassId { get; set; }
        public int PropertyId { get; set; }
        public string ClassName { get; set; }
        public string PropertyName { get; set; }
        public string Identifier { get {
            return ClassId == -1 && PropertyId == -1
                       ? "<<<< Invalid Identifier >>>>"
                       : (PropertyId == -1 ? "<<<< Invalid Property >>>>" : ClassName + "." + PropertyName);
        }
        }
        public override string ToString()
        {
            return Identifier;
        }
    }
}
