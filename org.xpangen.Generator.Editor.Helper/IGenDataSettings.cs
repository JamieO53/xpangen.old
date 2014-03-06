// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using org.xpangen.Generator.Editor.Model;

namespace org.xpangen.Generator.Editor.Helper
{
    public interface IGenDataSettings
    {
        void DeleteFileGroup();
        FileGroup GetFileFromCaption(string caption);
        void GetFileGroup();
        List<string> GetFileHistory(); 
        void SetFileGroup();
    }
}