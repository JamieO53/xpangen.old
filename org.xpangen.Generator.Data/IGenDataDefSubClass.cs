// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    public interface IGenDataDefSubClass
    {
        IGenDataDefClass SubClass { get; set; }
        string Reference { get; set; }
        string ReferenceDefinition { get; set; }
    }
}