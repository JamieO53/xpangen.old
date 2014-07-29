// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    /// <summary>
    /// A sub class reference
    /// </summary>
    public class GenDataDefSubClass : IGenDataDefSubClass
    {
        public IGenDataDefClass SubClass { get; set; }

        public string Reference { get; set; }

        public string ReferenceDefinition { get; set; }
    }
}