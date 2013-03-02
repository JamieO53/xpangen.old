// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Helper
{
    public interface IGenValidator
    {
        void Disable();
        void DisplayObject(bool created);
        bool DataChanged { get; set; }
        GenObject GenObject { get; set; }
        char Key { get; set; }
        void Validate();
        void Save();
    }
}