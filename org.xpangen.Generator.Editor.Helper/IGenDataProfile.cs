// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Helper
{
    public interface IGenDataProfile
    {
        IList GetDataSource(object context, string className);
        Profile.Profile.Profile Profile { get; set; }
        void LoadProfile(string profilePath, GenDataDef genDataDef1);
    }
}
