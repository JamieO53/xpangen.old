// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Helper
{
    public interface IGenData
    {
        bool Changed { get; }
        GenDataBase DefGenDataBase { get; }
        GenDataBase GenDataBase { get; }
        void SetBase(string filePath);
        void SetData(string filePath);
    }
}