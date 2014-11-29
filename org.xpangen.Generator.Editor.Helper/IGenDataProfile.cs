// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Editor.Helper
{
    public interface IGenDataProfile
    {
        IList GetDataSource(object context, string name);
        Profile.Profile.Profile Profile { get; set; }
        Fragment Fragment { get; set; }
        GenObject GenObject { get; }
        string ProfileText { get; }
        void LoadProfile(string profilePath, GenDataDef genDataDef);
        FragmentBody GetBody();
        string GetNodeProfileText();
        string GetNodeExpansionText(GenDataBase genDataBase, GenObject context);
        void CreateNewProfile(string newProfile, string newProfileTitle, string newProfileText);
        void SubstitutePlaceholder(TextBlock textBlock, string substitutedText, GenDataId id);
        bool IsInputable(int position);
        void GetFragmentsAt(out Fragment before, out Fragment after, int position);
        bool IsSelectable(int start, int end, bool b);
        FragmentSelection GetSelection(int start, int end);
    }
}
