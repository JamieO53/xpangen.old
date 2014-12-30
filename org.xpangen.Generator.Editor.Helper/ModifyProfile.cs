using System.Diagnostics.Contracts;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Editor.Helper
{
    public class ModifyProfile
    {
        private GeProfile GeProfile { get; set; }

        private ProfileTextPosition NodeFragmentPosition { get; set; }

        private string NewProfile { get; set; }

        public ModifyProfile(GeProfile geProfile)
        {
            GeProfile = geProfile;
        }

        public void CutSelection(FragmentSelection fragments)
        {
            Contract.Requires(GeProfile.IsSelectable(fragments.Start, fragments.End, false));
            Contract.Ensures(GeProfile.IsInputable(fragments.Start));
            SaveState();
            NewProfile = GeProfile.ProfileText.Substring(0, fragments.Start) + GeProfile.ProfileText.Substring(fragments.End);
            ChangeProfile();
            RestoreState();
        }

        public void InsertSelection(int position, FragmentSelection fragments)
        {
            Contract.Requires(GeProfile.IsInputable(position));
            Contract.Ensures(GeProfile.IsSelectable(position, position + fragments.ProfileText.Length, false));
            SaveState();
            NewProfile = GeProfile.ProfileText.Insert(NodeFragmentPosition.Position.Offset + position,
                fragments.ProfileText);
            ChangeProfile();
            RestoreState();
        }

        private void SaveState()
        {
            var fragment = GeProfile.Fragment;
            GeProfile.Fragment = GeProfile.Profile;
            GeProfile.GetNodeProfileText();
            NodeFragmentPosition = GeProfile.ProfileTextPostionList.GetFragmentPosition(fragment);
        }

        private void ChangeProfile()
        {
            GeProfile.Profile = new GenCompactProfileParser(GeProfile.GeData.GenDataDef, "", NewProfile).Profile;
            return;
        }

        private void RestoreState()
        {
            GeProfile.Fragment = GeProfile.Profile;
            GeProfile.GetNodeProfileText();
            Fragment after;
            Fragment before;
            GeProfile.GetFragmentsAt(out before, out after, NodeFragmentPosition.Position.Offset);
            GeProfile.Fragment = after;
            GeProfile.GetNodeProfileText();
        }
    }
}