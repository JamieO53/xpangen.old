using System.Collections.Generic;
using NUnit.Framework;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Test
{
    public class EditorHelperProfileTestsBase : GenDataTestsBase
    {
        protected static GeData CreateNewProfile(string newProfileText, string fileGroup = "Definition",
            string newProfile = "NewProfile", string newProfileTitle = "New Profile Title")
        {
            var geData = SetUpGeData(fileGroup);
            geData.Profile.CreateNewProfile(newProfile, newProfileTitle, newProfileText);
            geData.Profile.GetNodeProfileText();
            return geData;
        }

        protected static GeData LoadProfile(string newProfileText, string fileGroup = "Definition",
            string newProfile = "NewProfile", string newProfileTitle = "New Profile Title")
        {
            var geData = SetUpGeData(fileGroup);
            var parser = new GenCompactProfileParser(geData.GenDataDef, "", newProfileText);
            geData.Profile.Profile = parser.Profile;
            geData.Settings.BaseFile.AddProfile(newProfile, newProfile + ".prf", "Data", newProfileTitle);
            return geData;
        }

        private static GeData SetUpGeData(string fileGroup)
        {
            var geData = GeData.GetDefaultGeData(true);
            var settings = PopulateGenSettings();
            geData.Settings = new GeSettings(settings);
            geData.SetFileGroup(fileGroup);
            return geData;
        }

        protected static void VerifyProfile(GeData geData, string newProfileText, string expectedProfileText)
        {
            Assert.AreEqual(newProfileText, geData.Profile.GetNodeExpansionText(geData.GenDataBase, null));
            var nodeProfileText = geData.Profile.GetNodeProfileText();
            Assert.AreEqual(expectedProfileText, nodeProfileText);
            Assert.AreSame(nodeProfileText, geData.Profile.ProfileText);
        }

        protected static void ValidateFragmentSelection(ValidateFragmentSelectionParams selectionParams)
        {
            var geData = LoadProfile(selectionParams.NewProfileText);
            geData.Profile.Fragment = geData.Profile.Profile;
            geData.Profile.GetNodeProfileText();
            Assert.AreEqual(selectionParams.IsSelectable,
                geData.Profile.IsSelectable(selectionParams.Start, selectionParams.End, false),
                "Is the specified profile text selectable? (" + selectionParams.Comment + ")");
            var fragments = geData.Profile.GetSelection(selectionParams.Start, selectionParams.End);
            Assert.AreEqual(selectionParams.ExpectedText, fragments.ProfileText);
            Assert.AreEqual(selectionParams.FragmentTypes.Count, fragments.Fragments.Count);
            for (var i = 0; i < fragments.Fragments.Count; i++)
                Assert.AreEqual(selectionParams.FragmentTypes[i], fragments.Fragments[i].FragmentType,
                    "Fragment[" + i + "] (" + selectionParams.Comment + ")");
            Assert.AreEqual(selectionParams.ExpectedPrefix, fragments.Prefix, "Prefix (" + selectionParams.Comment + ")");
            if (!fragments.HasPrefix)
            {
                Assert.IsNull(fragments.TextPrefix.Text, "Null Prefix (" + selectionParams.Comment + ")");
                Assert.AreEqual("", selectionParams.ExpectedPrefix, "Blank Prefix (" + selectionParams.Comment + ")");
            }
            Assert.AreEqual(selectionParams.ExpectedSuffix, fragments.Suffix, "Suffix (" + selectionParams.Comment + ")");
            if (!fragments.HasSuffix)
            {
                Assert.IsNull(fragments.TextSuffix.Text, "Null Suffix (" + selectionParams.Comment + ")");
                Assert.AreEqual("", selectionParams.ExpectedSuffix, "Blank Suffix (" + selectionParams.Comment + ")");
            }
            Assert.AreEqual(selectionParams.ExpectedInfix, fragments.Infix, "Infix (" + selectionParams.Comment + ")");
            if (!fragments.HasInfix)
            {
                Assert.IsNull(fragments.TextInfix.Text, "Null Infix (" + selectionParams.Comment + ")");
                Assert.AreEqual("", selectionParams.ExpectedInfix, "Blank Infix (" + selectionParams.Comment + ")");
            }
        }

        protected void ValidateTextSelection(TextSelectionParams textSelectionParams)
        {
            var geData = LoadProfile(textSelectionParams.NewProfileText);
            geData.Profile.Fragment = geData.Profile.Profile;
            geData.Profile.GetNodeProfileText();
            Assert.AreEqual(geData.Profile.IsSelectable(textSelectionParams.Start, textSelectionParams.End, true), textSelectionParams.IsSelectable,
                "Is the specified text selectable? (" + textSelectionParams.Comment + ")");
            if (!textSelectionParams.IsSelectable) return;
            var fragments = geData.Profile.GetSelection(textSelectionParams.Start, textSelectionParams.End);
        }

        protected static void VerifyFragmentsAtPosition(GeData geData, int position, Fragment expectedBefore,
            Fragment expectedAfter)
        {
            Fragment before, after;
            geData.Profile.GetFragmentsAt(out before, out after, position);
            Assert.AreSame(expectedBefore, before, "Before position " + position);
            Assert.AreSame(expectedAfter, after, "After position " + position);
        }

        protected static void ValidateProfileTextPosition(ProfileTextPositionParams positionParams)
        {
            GeData geData = null;
            ValidateProfileTextPosition(positionParams, ref geData);
        }

        protected static void ValidateProfileTextPosition(ProfileTextPositionParams positionParams, ref GeData geData)
        {
            if (geData == null)
            {
                geData = LoadProfile(positionParams.NewProfileText);
                geData.Profile.Fragment = geData.Profile.Profile;
                var profileText = geData.Profile.GetNodeProfileText();
                Assert.AreEqual(positionParams.NewProfileText, profileText);
            }
            var pos = ((GeProfile)geData.Profile).ProfileTextPostionList.FindAtPosition(positionParams.Position);
            Assert.IsNotNull(pos);
            Assert.AreEqual(positionParams.FragmentType, pos.Fragment.FragmentType);
            Assert.AreEqual(positionParams.Expected, geData.Profile.IsInputable(positionParams.Position), positionParams.Message);
        }

        public class ProfileTextPositionParamsList : List<ProfileTextPositionParams>
        {
            public ProfileTextPositionParamsList()
            {
                const string newProfileText = "`[Class>:Class:`Class.Name` - `Class.Title`\r\n\t`[Property>:`Property.Name` - `Property.Title``]`]";
                Add(new ProfileTextPositionParams(true, 0, "refside start of segment", FragmentType.Segment, newProfileText));
                Add(new ProfileTextPositionParams(false, 1, "In segment prefix", FragmentType.Segment, newProfileText));
                Add(new ProfileTextPositionParams(false, 8, "In segment prefix", FragmentType.Segment, newProfileText));
                Add(new ProfileTextPositionParams(true, 9, "Start of text", FragmentType.Text, newProfileText));
                Add(new ProfileTextPositionParams(true, 10, "Text", FragmentType.Text, newProfileText));
                Add(new ProfileTextPositionParams(true, 15, "End of text", FragmentType.Placeholder, newProfileText));
                Add(new ProfileTextPositionParams(false, 16, "In placeholder", FragmentType.Placeholder, newProfileText));
                Add(new ProfileTextPositionParams(false, 26, "In placeholder", FragmentType.Placeholder, newProfileText));
                Add(new ProfileTextPositionParams(true, 27, "Start of text", FragmentType.Text, newProfileText));
                Add(new ProfileTextPositionParams(true, 94, "Between segment and containing segment", FragmentType.Segment, newProfileText));
                Add(new ProfileTextPositionParams(true, 58, "Between placeholder and containing segment end", FragmentType.Placeholder, newProfileText));
                Add(new ProfileTextPositionParams(true, 92, "Between placeholder and containing segment end", FragmentType.TextBlock, newProfileText));
                Add(new ProfileTextPositionParams(true, 96, "refside end of segment", FragmentType.Segment, newProfileText));
            }
        }
        public class ProfileTextPositionParams
        {
            public ProfileTextPositionParams(bool expected, int position, string message, FragmentType fragmentType, string newProfileText)
            {
                Expected = expected;
                Position = position;
                Message = message;
                FragmentType = fragmentType;
                NewProfileText = newProfileText;
            }

            public bool Expected { get; private set; }
            public int Position { get; private set; }
            public string Message { get; private set; }
            public FragmentType FragmentType { get; private set; }
            public string NewProfileText { get; private set; }
        }

        public class FragmentSelectionCutTestParamsList : List<FragmentSelectionCutTestParams>
        {
            public FragmentSelectionCutTestParamsList()
            {
                const string newProfileText =
                    "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]";
                Add(new FragmentSelectionCutTestParams(
                    newProfileText,
                    "Whole profile text",
                    0, 115, "",
                    newProfileText));
                Add(new FragmentSelectionCutTestParams(
                    newProfileText,
                    "Whole Property segment",
                    43, 94, "`[Class>:Class:`Class.Name` - `Class.Title`\r\nEnd `Class.Name` `]",
                    "`[Property>:\r\n\t`Property.Name` - `Property.Title``]"));
                Add(new FragmentSelectionCutTestParams(
                    newProfileText,
                    "Whole Text fragment",
                    9, 15, "`[Class>:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Class:"));
                Add(new FragmentSelectionCutTestParams(
                    newProfileText,
                    "End of Text fragment",
                    10, 15, "`[Class>:C`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "lass:"));
                Add(new FragmentSelectionCutTestParams(
                    newProfileText,
                    "Start of Text fragment",
                    9, 14,
                    "`[Class>::`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Class"));
                Add(new FragmentSelectionCutTestParams(
                    newProfileText,
                    "Substring of Text fragment",
                    10, 14,
                    "`[Class>:C:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "lass"));
                Add(new FragmentSelectionCutTestParams(
                    newProfileText,
                    "Text and whole Property segment",
                    29, 94, "`[Class>:Class:`Class.Name` -\r\nEnd `Class.Name` `]",
                    " `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]"));
                Add(new FragmentSelectionCutTestParams(
                    newProfileText,
                    "Whole Property segment and following text",
                    43, 112, "`[Class>:Class:`Class.Name` - `Class.Title` `]",
                    "`[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name`"));
            }
        }

        public class FragmentSelectionCutTestParams
        {
            public FragmentSelectionCutTestParams(string newProfileText, string comment, int start, int end, string expectedText, string expectedSelectionText)
            {
                NewProfileText = newProfileText;
                Comment = comment;
                Start = start;
                End = end;
                ExpectedText = expectedText;
                ExpectedSelectionText = expectedSelectionText;
            }
            public string NewProfileText { get; private set; }
            public string Comment { get; set; }
            public int Start { get; private set; }
            public int End { get; private set; }
            public string ExpectedText { get; private set; }
            public string ExpectedSelectionText { get; private set; }
            public override string ToString()
            {
                return Comment;
            }
        }

        public class FragmentInsertTestParamsList : List<FragmentInsertTestParams>
        {
            public FragmentInsertTestParamsList()
            {
                const string expectedSelectionText = "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]";
                Add(new FragmentInsertTestParams("",
                    "Whole profile text", 0, expectedSelectionText, expectedSelectionText));
                Add(new FragmentInsertTestParams("`[Class>:Class:`Class.Name` - `Class.Title`\r\nEnd `Class.Name` `]",
                    "Whole Property segment", 43, expectedSelectionText, "`[Property>:\r\n\t`Property.Name` - `Property.Title``]"));
                Add(new FragmentInsertTestParams("`[Class>:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Whole Text fragment", 9, expectedSelectionText, "Class:"));
                Add(new FragmentInsertTestParams("`[Class>:C`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "End of Text fragment", 10, expectedSelectionText, "lass:"));
                Add(new FragmentInsertTestParams("`[Class>::`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Start of Text fragment", 9, expectedSelectionText, "Class"));
                Add(new FragmentInsertTestParams("`[Class>:C:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Substring of Text fragment", 10, expectedSelectionText, "lass"));
                Add(new FragmentInsertTestParams("`[Class>:Class:`Class.Name` -\r\nEnd `Class.Name` `]",
                    "Text and whole Property segment", 29, expectedSelectionText, " `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]"));
                Add(new FragmentInsertTestParams("`[Class>:Class:`Class.Name` - `Class.Title` `]",
                    "Whole Property segment and following text", 43, expectedSelectionText, "`[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name`"));
            }
        }

        public class FragmentInsertTestParams
        {
            public FragmentInsertTestParams(string newProfileText, string comment, int position, string expectedText, string expectedSelectionText)
            {
                NewProfileText = newProfileText;
                Comment = comment;
                Position = position;
                ExpectedText = expectedText;
                ExpectedSelectionText = expectedSelectionText;
            }
            public string NewProfileText { get; private set; }
            public string Comment { get; set; }
            public int Position { get; private set; }
            public string ExpectedText { get; private set; }
            public string ExpectedSelectionText { get; private set; }
            public override string ToString()
            {
                return Comment;
            }
        }

        public class ValidateFragmentSelectionParamsList : List<ValidateFragmentSelectionParams>
        {
            public ValidateFragmentSelectionParamsList()
            {
                Add(
                    new ValidateFragmentSelectionParams(
                        "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                        true, 0,
                        "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]"
                            .Length,
                        "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                        "Whole profile text", new[] { FragmentType.Segment }, "", "", ""));
                Add(
                    new ValidateFragmentSelectionParams(
                        "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                        true, 43, 94, "`[Property>:\r\n\t`Property.Name` - `Property.Title``]", "Whole Property segment",
                        new[] { FragmentType.Segment }, "", "", ""));
                Add(
                    new ValidateFragmentSelectionParams(
                        "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                        true, 9, 15, "Class:", "Whole Text fragment", new[] { FragmentType.Text }, "", "", ""));
                Add(
                    new ValidateFragmentSelectionParams(
                        "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                        true, 10, 15, "lass:", "End of Text fragment", new FragmentType[0], "lass:", "", ""));
                Add(
                    new ValidateFragmentSelectionParams(
                        "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                        true, 9, 14, "Class", "Start of Text fragment", new FragmentType[0], "", "Class", ""));
                Add(
                    new ValidateFragmentSelectionParams(
                        "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                        true, 10, 14, "lass", "Substring of Text fragment", new FragmentType[0], "", "", "lass"));
                Add(
                    new ValidateFragmentSelectionParams(
                        "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                        true, 29, 94, " `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]",
                        "Text and whole Property segment", new[] { FragmentType.Placeholder, FragmentType.Segment }, " ",
                        "", ""));
                Add(
                    new ValidateFragmentSelectionParams(
                        "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                        true, 43, 112, "`[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name`",
                        "Whole Property segment and following text",
                        new[] { FragmentType.Segment, FragmentType.Text, FragmentType.Placeholder }, "", "", ""));
            }
        }

        public class ValidateFragmentSelectionParams
        {
            public ValidateFragmentSelectionParams(string newProfileText, bool isSelectable, int start, int end,
                string expectedText, string comment, IList<FragmentType> fragmentTypes, string expectedPrefix,
                string expectedSuffix, string expectedInfix)
            {
                NewProfileText = newProfileText;
                IsSelectable = isSelectable;
                Start = start;
                End = end;
                ExpectedText = expectedText;
                Comment = comment;
                FragmentTypes = fragmentTypes;
                ExpectedPrefix = expectedPrefix;
                ExpectedSuffix = expectedSuffix;
                ExpectedInfix = expectedInfix;
            }

            public string NewProfileText { get; private set; }
            public bool IsSelectable { get; private set; }
            public int Start { get; private set; }
            public int End { get; private set; }
            public string ExpectedText { get; private set; }
            public string Comment { get; private set; }
            public IList<FragmentType> FragmentTypes { get; private set; }
            public string ExpectedPrefix { get; private set; }
            public string ExpectedSuffix { get; private set; }
            public string ExpectedInfix { get; private set; }

            public override string ToString()
            {
                return Comment;
            }
        }

        public class TextSelectionParamsList : List<TextSelectionParams>
        {
            public TextSelectionParamsList()
            {
                const string newProfileText =
                    "`[Class>:Class:`Class.Name` - `Class.Title`\r\n\t`[Property>:`Property.Name` - `Property.Title``]`]";
                Add(new TextSelectionParams(false, 0, 96, "", "Whole profile text", newProfileText));
                Add(new TextSelectionParams(true, 9, 15, "Class:", "Whole text fragment", newProfileText));
                Add(new TextSelectionParams(true, 10, 14, "lass", "Whole text fragment", newProfileText));
                Add(new TextSelectionParams(false, 9, 16, "Class:`", "Text and partial placeholder", newProfileText));
                Add(new TextSelectionParams(true, 9, 27, "Class:`Class.Name`", "Text and placeholder", newProfileText));
                Add(new TextSelectionParams(false, 46, 94, "`[Property>:`Property.Name` - `Property.Title``]", "Segment", newProfileText));
                Add(new TextSelectionParams(false, 43, 74, "\r\n\t`[Property>:`Property.Name` ", "Outside container to inside container", newProfileText));
            }
        }

        public class TextSelectionParams
        {
            public TextSelectionParams(bool isSelectable, int start, int end, string expectedText, string comment, string newProfileText)
            {
                IsSelectable = isSelectable;
                Start = start;
                End = end;
                ExpectedText = expectedText;
                Comment = comment;
                NewProfileText = newProfileText;
            }

            public bool IsSelectable { get; private set; }
            public int Start { get; private set; }
            public int End { get; private set; }
            public string ExpectedText { get; private set; }
            public string Comment { get; private set; }
            public string NewProfileText { get; private set; }
        }
    }
}