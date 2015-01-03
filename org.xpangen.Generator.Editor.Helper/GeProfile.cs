// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Editor.Helper
{
    public abstract class ProfileCommandBase : IGenCommand
    {
        protected ProfileCommandBase(GeProfile geProfile)
        {
            GeProfile = geProfile;
        }

        protected GeProfile GeProfile { get; set; }
        public virtual void Execute()
        {
            GeProfile.GetNodeProfileText();
        }
    }

    public class ReplaceSelectionWithPlaceholderCommand : ProfileCommandBase
    {
        public Text Text { get; private set; }
        private int SelectionStart { get; set; }
        private int SelectionEnd { get; set; }
        private GenDataId Id { get; set; }
        private FragmentBody Body { get; set; }
        private int FragmentIndex { get; set; }
        private string Prefix { get; set; }
        public string Suffix { get; private set; }
        public Placeholder Placeholder { get; private set; }


        public ReplaceSelectionWithPlaceholderCommand(Text text, int selectionStart, int selectionEnd, GenDataId id, GeProfile geProfile) : base(geProfile)
        {
            Text = text;
            SelectionStart = selectionStart;
            SelectionEnd = selectionEnd;
            Id = id;
            Body = (FragmentBody) Text.Parent;
            FragmentIndex = Body.FragmentList.IndexOf(Text) + 1;
            Prefix = Text.TextValue.Substring(0, SelectionStart);
            Suffix = Text.TextValue.Substring(SelectionEnd);
        }

        private void SetTextToPrefix()
        {
            if (Prefix == "")
            {
                Body.FragmentList.Remove(Text);
                FragmentIndex--;
            }
            else
                Text.TextValue = Prefix;
        }

        private void AddPlaceholderFragment()
        {
            Placeholder = Body.AddPlaceholder(Body.FragmentName(FragmentType.Placeholder), Id.ClassName, Id.PropertyName);
            FixAddedFragmentPosition();
        }

        private void FixAddedFragmentPosition()
        {
            for (var l = Body.FragmentList.Count - 1; l > FragmentIndex; l--)
                Body.FragmentList.Move(ListMove.Up, l);
            FragmentIndex++;
        }

        private void AddSuffixText()
        {
            if (Suffix == "") return;
            Text = Body.AddText(Body.FragmentName(FragmentType.Text), Suffix);
            FixAddedFragmentPosition();
        }

        public override void Execute()
        {
            SetTextToPrefix();
            AddPlaceholderFragment();
            AddSuffixText();
            base.Execute();
        }
    }

    public class ReplacePlaceholderWithTextCommand : ProfileCommandBase
    {
        private Placeholder Placeholder { get; set; }
        private string SubstitutedText { get; set; }
        private int SelectionStart { get; set; }
        private int SelectionEnd { get; set; }
        private GenDataId Id { get; set; }

        public ReplacePlaceholderWithTextCommand(Placeholder placeholder, string substitutedText, GeProfile geProfile) : base(geProfile)
        {
            Placeholder = placeholder;
            SubstitutedText = substitutedText;
            SelectionStart = 0;
            SelectionEnd = SubstitutedText.Length;
            Id = new GenDataId {ClassName = Placeholder.Class, PropertyName = Placeholder.Property};
        }

        public override void Execute()
        {
            var body = (FragmentBody) Placeholder.Parent;
            var i = body.FragmentList.IndexOf(Placeholder);
            if (i != 0 && body.FragmentList[i - 1] is Text)
            {
                var text = (Text) body.FragmentList[i - 1];
                SelectionStart = text.TextValue.Length;
                SelectionEnd = SelectionStart + SubstitutedText.Length;
                text.TextValue += SubstitutedText;
                body.FragmentList.RemoveAt(i);
                
                if (i >= body.FragmentList.Count || !(body.FragmentList[i] is Text)) return;
                text.TextValue += ((Text) body.FragmentList[i]).TextValue;
                body.FragmentList.RemoveAt(i);
            }
            else
            {
                if (i + 1 < body.FragmentList.Count && body.FragmentList[i + 1] is Text)
                {
                    var text = ((Text) body.FragmentList[i + 1]);
                    text.TextValue = SubstitutedText + text.TextValue;
                }
                else
                {
                    body.AddText(body.FragmentName(FragmentType.Text), SubstitutedText);
                    for (var j = body.FragmentList.Count - 1; j < i; j--)
                        body.FragmentList.Move(ListMove.Up, j);
                }
                body.FragmentList.RemoveAt(i);
            }
            base.Execute();
        }
    }

    public class CutSelectionCommand : ProfileCommandBase
    {
        public FragmentSelection Fragments { get; set; }

        public CutSelectionCommand(GeProfile geProfile, FragmentSelection fragments)
            : base(geProfile)
        {
            Fragments = fragments;
        }

        public override void Execute()
        {
            var modifyProfile = new ModifyProfile(GeProfile);
            modifyProfile.CutSelection(Fragments);
            base.Execute();
        }
    }

    public class InsertSelectionCommand : ProfileCommandBase
    {
        public int Position { get; set; }
        public FragmentSelection Fragments { get; set; }

        public InsertSelectionCommand(GeProfile geProfile, int position, FragmentSelection fragments)
            : base(geProfile)
        {
            Position = position;
            Fragments = fragments;
        }

        public override void Execute()
        {
            var modifyProfile = new ModifyProfile(GeProfile);
            modifyProfile.InsertSelection(Position, Fragments);
            base.Execute();
        }
    }
    
    public class GeProfile : IGenDataProfile
    {
        private ProfileFragmentSyntaxDictionary _activeProfileFragmentSyntaxDictionary;

        public GeData GeData { get; private set; }

        public ProfileTextPostionList ProfileTextPostionList { get; private set; }

        private ProfileFragmentSyntaxDictionary ActiveProfileFragmentSyntaxDictionary
        {
            get
            {
                return (_activeProfileFragmentSyntaxDictionary = _activeProfileFragmentSyntaxDictionary ??
                       ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary);
            }
        }

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

        public GenObject GenObject { get; private set; }

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
            GenMultiUndoRedo multi = null;
            var body = textBlock.Body();
            var n = body.FragmentList.Count;
            for (var i = n - 1; i >= 0; i--)
            {
                var text = body.FragmentList[i] as Text;
                if (text == null) continue;
                
                var t = text.TextValue;
                var k = t.IndexOf(substitutedText, StringComparison.Ordinal);
                while (k != -1 && text != null)
                {
                    var command = new ReplaceSelectionWithPlaceholderCommand(text, k, k + substitutedText.Length, id, this);
                    command.Execute();
                    var undoCommand = new ReplacePlaceholderWithTextCommand(command.Placeholder, substitutedText, this);
                    (multi = multi ?? new GenMultiUndoRedo()).Add(new GenUndoRedo(undoCommand, command));
                    text = command.Text;
                    t = command.Suffix;
                    k = t.IndexOf(substitutedText, StringComparison.Ordinal);
                }
            }
            if (multi != null) GeData.AddRedoUndo(multi);
            GetNodeProfileText();
        }

        public string GetNodeProfileText()
        {
            if (Fragment == null) return "";
            var textExpander = new GenProfileTextExpander(ActiveProfileFragmentSyntaxDictionary);
            ProfileTextPostionList = textExpander.ProfileTextPostionList;
            ProfileText = textExpander.GetText(Fragment);
            return ProfileText;
        }

        public bool IsInputable(int position)
        {
            if (position == 0 || position == ProfileText.Length) return true;
            var pos = ProfileTextPostionList.FindAtPosition(position);
            if (pos.Position.Offset == position) return true;
            if (pos.Position.Offset + pos.Position.Length == position) return true;
            if (pos.Fragment.FragmentType == FragmentType.Text) return true;
            return false;
        }

        public void GetFragmentsAt(out Fragment before, out Fragment after, int position)
        {
            var pos = ProfileTextPostionList.FindAtPosition(position);
           
            if (pos == null)
            {
                before = Profile;
                after = null;
                return;
            }
            
            if (pos.Position.Offset + pos.Position.Length == position)
            {
                before = pos.Fragment;
                after = null;
                return;
            }
            
            if (pos.Position.Offset != position && pos.Position.Offset + pos.Position.Length != position)
            {
                before = pos.Fragment;
                after = pos.Fragment;
                return;
            }

            after = pos.Fragment;
            var fragments = ((FragmentBody) after.Parent).FragmentList;
            var i = fragments.IndexOf(after);
            if (i == 0 && after.ParentFragment.FragmentType == FragmentType.TextBlock)
            {
                fragments = ((FragmentBody) after.ParentFragment.Parent).FragmentList;
                i = fragments.IndexOf(after.ParentFragment);
            }
            before = i == 0 ? null : fragments[i - 1];
        }

        public bool IsSelectable(int start, int end, bool textSelection)
        {
            if (start > end) return false;
            if (start < 0 || end > ProfileText.Length) return false;
            Fragment beforeStart, afterStart, beforeEnd, afterEnd;
            GetFragmentsAt(out beforeStart, out afterStart, start);
            GetFragmentsAt(out beforeEnd, out afterEnd, end);
            if (afterStart == null || beforeEnd == null) return false;
            if (afterStart.ParentFragment != beforeEnd.ParentFragment)
                if (afterStart.ParentFragment != beforeEnd.ParentFragment.ParentFragment)
                    if (afterStart.ParentFragment.ParentFragment != beforeEnd.ParentFragment)
                        return false;
            if (textSelection && !(afterStart.ParentFragment is TextBlock)) return false;
            return IsInputable(start) && IsInputable(end);
        }

        public FragmentSelection GetSelection(int start, int end)
        {
            Contract.Requires(IsSelectable(start, end, false));
            var fragmentSelection = new FragmentSelection(start, end);
            Fragment beforeStart, afterStart, beforeEnd, afterEnd;
            GetFragmentsAt(out beforeStart, out afterStart, start);
            GetFragmentsAt(out beforeEnd, out afterEnd, end);
            if (beforeStart != afterStart && beforeEnd != afterEnd)
            {
                if (afterStart == beforeEnd)
                    fragmentSelection.Fragments.Add(afterStart);
                else if (afterStart.ParentFragment == beforeEnd.ParentFragment)
                {
                    var fragments = ((FragmentBody) afterStart.Parent).FragmentList;
                    CopySelectionFragments(fragments, fragmentSelection.Fragments, fragments.IndexOf(afterStart),
                        fragments.IndexOf(beforeEnd));
                }
                else
                {
                    var prefixText = afterStart.ParentFragment is TextBlock;
                    var suffixText = beforeEnd.ParentFragment is TextBlock;
                    var fragments = prefixText
                        ? ((FragmentBody) afterStart.ParentFragment.Parent).FragmentList
                        : ((FragmentBody) afterStart.Parent).FragmentList;
                    int i;
                    if (prefixText)
                    {
                        i = fragments.IndexOf(afterStart.ParentFragment) + 1;
                        var textFragments = ((FragmentBody) afterStart.Parent).FragmentList;
                        CopySelectionFragments(textFragments, fragmentSelection.Fragments,
                            textFragments.IndexOf(afterStart), textFragments.Count - 1);
                    }
                    else i = fragments.IndexOf(afterStart);
                    var j = suffixText ? fragments.IndexOf(beforeEnd.ParentFragment) - 1 : fragments.IndexOf(beforeEnd);
                    CopySelectionFragments(fragments, fragmentSelection.Fragments, i, j);
                    if (suffixText)
                    {
                        var textFragments = ((FragmentBody) beforeEnd.Parent).FragmentList;
                        CopySelectionFragments(textFragments, fragmentSelection.Fragments, 0,
                            textFragments.IndexOf(beforeEnd));
                    }
                }
            }
            else if (beforeStart == afterStart && beforeEnd != afterEnd)
            {
                Contract.Assert(beforeStart is Text);
                var pos = ProfileTextPostionList.GetFragmentPosition(afterStart);
                fragmentSelection.SetPrefix((Text) afterStart, start - pos.Position.Offset);
                if (afterStart != beforeEnd)
                {
                    var fragments = ((FragmentBody) afterStart.Parent).FragmentList;
                    var n = fragments.Count;
                    var i = fragments.IndexOf(afterStart);
                    CopySelectionFragments(fragments, fragmentSelection.Fragments, i + 1, n - 1);
                    fragmentSelection.Fragments.Add(beforeEnd);
                }
            }
            else if (beforeStart != afterStart && beforeEnd == afterEnd)
            {
                Contract.Assert(afterEnd is Text);
                var pos = ProfileTextPostionList.GetFragmentPosition(beforeEnd);
                fragmentSelection.SetSuffix((Text) beforeEnd, end - pos.Position.Offset);
                if (beforeEnd != afterStart)
                    fragmentSelection.Fragments.Add(afterStart);
            }
            else if (beforeStart == afterStart && beforeEnd == afterEnd)
            {
                Contract.Assert(afterEnd is Text);
                var pos = ProfileTextPostionList.GetFragmentPosition(beforeEnd);
                fragmentSelection.SetInfix((Text) beforeEnd, start - pos.Position.Offset, end - pos.Position.Offset);
            }

            fragmentSelection.ProfileText = ProfileText.Substring(start, end - start);
            return fragmentSelection;
        }

        public void Cut(FragmentSelection fragments)
        {
            var command = new CutSelectionCommand(this, fragments);
            command.Execute();
            var undoCommand = new InsertSelectionCommand(this, fragments.Start, fragments);
            AddRedoUndo(undoCommand, command);
        }

        public void Insert(int position, FragmentSelection fragments)
        {
            var command = new InsertSelectionCommand(this, position, fragments);
            command.Execute();
            var undoCommand = new CutSelectionCommand(this, fragments);
            AddRedoUndo(undoCommand, command);
        }

        public void Insert(int position, string text)
        {
            Contract.Requires(IsInputable(position));
            Contract.Ensures(IsSelectable(position, position + text.Length, false));
            var fragments = new FragmentSelection(position, position + text.Length) {ProfileText = text};
            Insert(position, fragments);
        }
        
        private void AddRedoUndo(IGenCommand undoCommand, IGenCommand redoCommand)
        {
            var undoRedo = new GenUndoRedo(undoCommand, redoCommand);
            GeData.AddRedoUndo(undoRedo);
        }

        private static void CopySelectionFragments(GenNamedApplicationList<Fragment> fragments,
            List<Fragment> selectionFragments, int first, int last)
        {
            for (var i = first; i <= last; i++)
                selectionFragments.Add(fragments[i]);
        }
    }
}