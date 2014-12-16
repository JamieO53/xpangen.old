﻿// // This Source Code Form is subject to the terms of the Mozilla Public
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
        public ProfileCommandBase(GeProfile geProfile)
        {
            GeProfile = geProfile;
        }

        public GeProfile GeProfile { get; private set; }
        public virtual void Execute()
        {
            GeProfile.GetNodeProfileText();
        }
    }

    public class ReplaceSelectionWithPlaceholderCommand : ProfileCommandBase
    {
        public Text Text { get; private set; }
        public int SelectionStart { get; private set; }
        public int SelectionEnd { get; private set; }
        public GenDataId Id { get; private set; }
        public FragmentBody Body { get; private set; }
        public int FragmentIndex { get; private set; }
        public string Prefix { get; private set; }
        public string Suffix { get; private set; }
        public Placeholder Placeholder { get; private set; }


        public ReplaceSelectionWithPlaceholderCommand(Text text, int selectionStart, int selectionEnd, GenDataId id, GeProfile geProfile) : base(geProfile)
        {
            var prefix = text.TextValue.Substring(0, selectionStart);
            var suffix = text.TextValue.Substring(selectionEnd);
            Text = text;
            SelectionStart = selectionStart;
            SelectionEnd = selectionEnd;
            Id = id;
            Body = (FragmentBody) Text.Parent;
            FragmentIndex = Body.FragmentList.IndexOf(Text) + 1;
            Prefix = prefix;
            Suffix = suffix;
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
        public Placeholder Placeholder { get; private set; }
        public string SubstitutedText { get; private set; }
        public int SelectionStart { get; private set; }
        public int SelectionEnd { get; private set; }
        public GenDataId Id { get; private set; }

        public ReplacePlaceholderWithTextCommand(Placeholder placeholder, string substitutedText, GeProfile geProfile) : base(geProfile)
        {
            Placeholder = placeholder;
            SubstitutedText = substitutedText;
            SelectionStart = 0;
            SelectionEnd = SubstitutedText.Length;
            Id = new GenDataId {ClassName = placeholder.Class, PropertyName = placeholder.Property};
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
                    var text = body.AddText(body.FragmentName(FragmentType.Text), SubstitutedText);
                    for (int j = body.FragmentList.Count - 1; j < i; j--)
                    {
                        body.FragmentList.Move(ListMove.Up, j);
                    }
                }
                body.FragmentList.RemoveAt(i);
            }
            base.Execute();
        }
    }

    public class GeProfile : IGenDataProfile
    {
        private ProfileFragmentSyntaxDictionary _activeProfileFragmentSyntaxDictionary;

        private GeData GeData { get; set; }

        public ProfileTextPostionList ProfileTextPostionList { get; private set; }

        public ProfileFragmentSyntaxDictionary ActiveProfileFragmentSyntaxDictionary
        {
            get
            {
                return _activeProfileFragmentSyntaxDictionary ??
                       ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary;
            }
            set { _activeProfileFragmentSyntaxDictionary = value; }
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
            ProfileText = ProfileText.Substring(0, fragments.Start) + ProfileText.Substring(fragments.End);
            Profile = new GenCompactProfileParser(GeData.GenDataDef, "", ProfileText).Profile;
        }

        private static void CopySelectionFragments(GenNamedApplicationList<Fragment> fragments,
            List<Fragment> selectionFragments, int first, int last)
        {
            for (var i = first; i <= last; i++)
                selectionFragments.Add(fragments[i]);
        }
    }
}