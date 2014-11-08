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
        public GeData GeData { get; set; }

        public GeProfile(GeData geData)
        {
            GeData = geData;
        }

        public IList GetDataSource(object context, string name)
        {
            return GeData.ComboServer.GetComboItems(name);
        }

        public Profile.Profile.Profile Profile { get; set; }

        public Fragment Fragment { get; set; }

        public GenObject GenObject { get; set; }

        public string ProfileText { get; private set; }

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
            GenObject = genObject;
            if (Fragment == null) return "";
            var context = GenObject.GetContext(genObject ?? genData.Root, Fragment.ClassName());
            if (context == null) return "";
            return GenFragmentExpander.Expand(genData.GenDataDef, context, Fragment);
        }

        public void CreateNewProfile(string newProfile, string newProfileTitle, string newProfileText)
        {
            var profileParams = new GenProfileParams(GeData.GenDataDef);
            Profile = (Profile.Profile.Profile) profileParams.Fragment;
            var segment = Profile.Body().AddSegment(GeData.GenDataDef.Classes[1].Name);
            var textBlock = segment.Body().AddTextBlock();
            textBlock.Body().AddText(textBlock.Body().FragmentName(FragmentType.Text), newProfileText);
            Fragment = segment;
            GeData.GenObject = GeData.GenDataBase.Root;
            GenObject = GeData.GenObject;
            GeData.Settings.BaseFile.AddProfile(newProfile, newProfile + ".prf", GeData.Settings.BaseFile.FilePath,
                newProfileTitle).SaveFields();
        }

        public string GetNodeProfileText()
        {
            if (Fragment == null) return "";
            ProfileText = new GenProfileTextExpander(ActiveProfileFragmentSyntaxDictionary).GetText(Fragment);
            return ProfileText;
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