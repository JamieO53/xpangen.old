// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
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
        public ProfileTextPostionList _ProfileTextPostionList;
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

        public void SubstitutePlaceholder(TextBlock textBlock, string substitutedText, GenDataId id)
        {
            var body = textBlock.Body();
            var n = body.FragmentList.Count;
            for (var i = n - 1; i >= 0; i--)
            {
                var text = body.FragmentList[i] as Text;
                if (text == null) continue;
                
                var t = text.TextValue;
                var k = t.IndexOf(substitutedText, StringComparison.Ordinal);
                var fragmentIndex = i + 1;
                while (k != -1 && text != null)
                {
                    var prefix = t.Substring(0, k);
                    var suffix = t.Substring(k + substitutedText.Length);
                    text = SplitTextAtPlaceholder(prefix, id, suffix, text, body, ref fragmentIndex);
                    t = suffix;
                    k = t.IndexOf(substitutedText, StringComparison.Ordinal);
                }
            }
        }

        private static Text SplitTextAtPlaceholder(string prefix, GenDataId id, string suffix, Text text,
            FragmentBody body, ref int fragmentIndex)
        {
            SetTextToPrefix(prefix, text, body, ref fragmentIndex);

            AddPlaceholderFragment(id, body, ref fragmentIndex);
            return AddSuffixText(suffix, body, ref fragmentIndex);
        }

        private static Text AddSuffixText(string suffix, FragmentBody body, ref int fragmentIndex)
        {
            if (suffix == "") return null;
            var text = body.AddText(body.FragmentName(FragmentType.Text), suffix);
            FixAddedFragmentPosition(body, ref fragmentIndex);
            return text;
        }

        private static void AddPlaceholderFragment(GenDataId id, FragmentBody body, ref int fragmentIndex)
        {
            body.AddPlaceholder(body.FragmentName(FragmentType.Placeholder), id.ClassName, id.PropertyName);
            FixAddedFragmentPosition(body, ref fragmentIndex);
        }

        private static void SetTextToPrefix(string prefix, Text text, FragmentBody body, ref int fragmentIndex)
        {
            if (prefix == "")
            {
                body.FragmentList.Remove(text);
                fragmentIndex--;
            }
            else
                text.TextValue = prefix;
        }

        private static void FixAddedFragmentPosition(FragmentBody body, ref int fragmentIndex)
        {
            for (var l = body.FragmentList.Count - 1; l > fragmentIndex; l--)
                body.FragmentList.Move(ListMove.Up, l);
            fragmentIndex++;
        }

        public string GetNodeProfileText()
        {
            if (Fragment == null) return "";
            var textExpander = new GenProfileTextExpander(ActiveProfileFragmentSyntaxDictionary);
            _ProfileTextPostionList = textExpander.ProfileTextPostionList;
            ProfileText = textExpander.GetText(Fragment);
            return ProfileText;
        }

        public bool IsInputable(int position)
        {
            return (position == 0 || position == ProfileText.Length);
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