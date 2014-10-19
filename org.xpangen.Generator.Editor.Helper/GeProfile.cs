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

        public void LoadProfile(string profilePath, GenDataDef genDataDef)
        {
            Profile = profilePath != "" ? new GenCompactProfileParser(genDataDef, profilePath, "").Profile : null;
        }

        public FragmentBody GetBody(ContainerFragment containerFragment)
        {
            return containerFragment.Body();
        }

        public string GetNodeExpansionText(GenDataBase genData, GenObject genObject, Fragment fragment)
        {
            if (fragment == null) return "";
            var context = GenObject.GetContext(genObject ?? genData.Root, fragment.ClassName());
            if (context == null) return "";
            return GenFragmentExpander.Expand(genData.GenDataDef, context, fragment);
        }

        public string GetNodeProfileText(Fragment fragment)
        {
            var text = new GenProfileTextExpander(ProfileFragmentSyntaxDictionary
                .ActiveProfileFragmentSyntaxDictionary).GetText(fragment);
            return text;
        }
    }
}