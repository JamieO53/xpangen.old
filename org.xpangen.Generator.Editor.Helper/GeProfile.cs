// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeProfile : IGenDataProfile
    {
        private ProfileFragmentSyntaxDictionary _activeProfileFragmentSyntaxDictionary;
        public ComboServer ComboServer { get; set; }

        public GeProfile(ComboServer comboServer)
        {
            ComboServer = comboServer;
        }

        public IList GetDataSource(object context, string className)
        {
            return ComboServer.GetComboItems(className);
        }

        public Profile.Profile.Profile Profile { get; set; }

        public Fragment Fragment { get; set; }

        public void LoadProfile(string profilePath, GenDataDef genDataDef)
        {
            Profile = profilePath != "" ? new GenCompactProfileParser(genDataDef, profilePath, "").Profile : null;
        }

        public FragmentBody GetBody()
        {
            var containerFragment = (ContainerFragment) Fragment;
            return containerFragment.Body();
        }

        public string GetNodeExpansionText(GenDataBase genData, GenObject genObject)
        {
            if (Fragment == null) return "";
            var context = GenObject.GetContext(genObject ?? genData.Root, Fragment.ClassName());
            if (context == null) return "";
            return GenFragmentExpander.Expand(genData.GenDataDef, context, Fragment);
        }

        public string GetNodeProfileText()
        {
            if (Fragment == null) return "";
            return new GenProfileTextExpander(ActiveProfileFragmentSyntaxDictionary).GetText(Fragment);
        }

        public ProfileFragmentSyntaxDictionary ActiveProfileFragmentSyntaxDictionary
        {
            get
            {
                return _activeProfileFragmentSyntaxDictionary ??
                       ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary;
            }
            set { _activeProfileFragmentSyntaxDictionary = value; }
        }
    }
}