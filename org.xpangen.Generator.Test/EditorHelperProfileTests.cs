// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using NUnit.Framework;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    ///     Tests the profile editor functionality
    /// </summary>
    [TestFixture]
    public class EditorHelperProfileTests : GenDataTestsBase
    {
        [Test(Description = "Test the creation of a new profile from text.")]
        public void NewProfileSetupTest()
        {
            const string newProfileText = "Profile text";
            const string newProfile = "NewProfile";
            const string expectedProfileText = "`[Class>:" + newProfileText + "`]";

            var geData = CreateNewProfile(newProfileText);

            Assert.IsTrue(geData.Settings.BaseFile.ProfileList.Contains(newProfile));
            Assert.IsTrue(geData.Settings.Model.GenDataBase.Changed);
            Assert.AreEqual(FragmentType.Segment, geData.Profile.Fragment.FragmentType);
            var segment = (Segment) geData.Profile.Fragment;
            Assert.AreEqual("Class", segment.ClassName());
            Assert.AreEqual("", geData.Profile.GenObject.ClassName);
            VerifyProfile(geData, newProfileText, expectedProfileText);
        }

        [Test(Description = "Test the substitution of a placeholder for given text.")]
        public void PlaceholderSubstitutionTest()
        {
            const string newProfileText =
                "Replace this Class with a placeholder but not this class. Here is another replacement Class.";
            const string expectedProfileText =
                "`[Class>:Replace this `Class.Name` with a placeholder but not this class. Here is another replacement `Class.Name`.`]";
            var geData = CreateNewProfile(newProfileText);
            var textBlock = (TextBlock) ((Segment) geData.Profile.Fragment).Body().FragmentList[0];
            geData.Profile.SubstitutePlaceholder(textBlock, "Class", geData.GenDataDef.GetId("Class.Name"));
            VerifyProfile(geData, newProfileText, expectedProfileText);
        }

        [Test(Description = "Test the substitution of a placeholder for given text and undo.")]
        public void PlaceholderSubstitutionUndoTest()
        {
            const string newProfileText =
                "Replace this Class with a placeholder but not this class. Here is another replacement Class.";
            const string expectedProfileText =
                "`[Class>:Replace this `Class.Name` with a placeholder but not this class. Here is another replacement `Class.Name`.`]";
            const string expectedUndoProfileText =
                "`[Class>:Replace this Class with a placeholder but not this class. Here is another replacement Class.`]";
            var geData = CreateNewProfile(newProfileText);
            var textBlock = (TextBlock) ((Segment) geData.Profile.Fragment).Body().FragmentList[0];
            ValidateProfileTextPosition(true, 22, "End of text before substitution", FragmentType.Text, expectedUndoProfileText, ref geData);
            ValidateProfileTextPosition(true, 23, "Substituted placholder position", FragmentType.Text, expectedUndoProfileText, ref geData);
            VerifyFragmentsAtPosition(geData, 22, textBlock.Body().FragmentList[0], textBlock.Body().FragmentList[0]);
            VerifyProfile(geData, newProfileText, expectedUndoProfileText);
            geData.Profile.SubstitutePlaceholder(textBlock, "Class", geData.GenDataDef.GetId("Class.Name"));
            ValidateProfileTextPosition(true, 22, "End of text before substitution", FragmentType.Placeholder, expectedProfileText, ref geData);
            ValidateProfileTextPosition(false, 23, "Substituted placholder", FragmentType.Placeholder, expectedProfileText, ref geData);
            VerifyFragmentsAtPosition(geData, 22, textBlock.Body().FragmentList[0], textBlock.Body().FragmentList[1]);
            VerifyProfile(geData, newProfileText, expectedProfileText);
            geData.Undo();
            ValidateProfileTextPosition(true, 22, "End of text before substitution", FragmentType.Text, expectedUndoProfileText, ref geData);
            ValidateProfileTextPosition(true, 23, "Substituted placholder position", FragmentType.Text, expectedUndoProfileText, ref geData);
            VerifyFragmentsAtPosition(geData, 22, textBlock.Body().FragmentList[0], textBlock.Body().FragmentList[0]);
            VerifyProfile(geData, newProfileText, expectedUndoProfileText);
        }

        [Test(Description = "Test the substitution of two placeholders for given text.")]
        public void MultiplePlaceholderSubstitutionTest()
        {
            const string newProfileText =
                "Replace this Class with a placeholder but not this class. It's title is Class Definition";
            const string expectedProfileText =
                "`[Class>:Replace this `Class.Name` with a placeholder but not this class. It's title is `Class.Title``]";
            var geData = CreateNewProfile(newProfileText);
            var textBlock = (TextBlock) ((Segment) geData.Profile.Fragment).Body().FragmentList[0];

            // Substitutions must be done in this order, because the first contains the second.
            geData.Profile.SubstitutePlaceholder(textBlock, "Class Definition", geData.GenDataDef.GetId("Class.Title"));
            geData.Profile.SubstitutePlaceholder(textBlock, "Class", geData.GenDataDef.GetId("Class.Name"));
            VerifyProfile(geData, newProfileText, expectedProfileText);
        }

        [Test(Description = "Test the substitution of two placeholders for given text and undo.")]
        public void MultiplePlaceholderSubstitutionUndoTest()
        {
            const string newProfileText =
                "Replace this Class with a placeholder but not this class. It's title is Class Definition";
            const string expectedProfileText =
                "`[Class>:Replace this `Class.Name` with a placeholder but not this class. It's title is `Class.Title``]";
            const string expectedUndoProfileText =
                "`[Class>:Replace this Class with a placeholder but not this class. It's title is Class Definition`]";
            var geData = CreateNewProfile(newProfileText);
            var textBlock = (TextBlock) ((Segment) geData.Profile.Fragment).Body().FragmentList[0];

            // Substitutions must be done in this order, because the first contains the second.
            geData.Profile.SubstitutePlaceholder(textBlock, "Class Definition", geData.GenDataDef.GetId("Class.Title"));
            geData.Profile.SubstitutePlaceholder(textBlock, "Class", geData.GenDataDef.GetId("Class.Name"));
            VerifyProfile(geData, newProfileText, expectedProfileText);
            geData.Undo();
            geData.Undo();
            VerifyProfile(geData, newProfileText, expectedUndoProfileText);
        }

        [Test(Description = "Tests the identification of an object by position")]
        public void FragementsAtPositionTest()
        {
            const string newProfileText =
                "`[Class>:Class:`Class.Name` - `Class.Title`\r\n\t`[Property>:`Property.Name` - `Property.Title``]`]";
            var geData = LoadProfile(newProfileText);
            geData.Profile.Fragment = geData.Profile.Profile;
            geData.Profile.GetNodeProfileText();
            var classSegment = (Segment) geData.Profile.Profile.Body().FragmentList[0];
            VerifyFragmentsAtPosition(geData, 0, null, classSegment);
            VerifyFragmentsAtPosition(geData, 1, classSegment, classSegment);
            VerifyFragmentsAtPosition(geData, 8, classSegment, classSegment);
            var classTextBlockBody = ((TextBlock) classSegment.Body().FragmentList[0]).Body();
            var classText = classTextBlockBody.FragmentList[0];
            VerifyFragmentsAtPosition(geData, 9, null, classText);
            VerifyFragmentsAtPosition(geData, 10, classText, classText);
            var classNamePlaceholder = classTextBlockBody.FragmentList[1];
            VerifyFragmentsAtPosition(geData, 15, classText, classNamePlaceholder);
            VerifyFragmentsAtPosition(geData, 16, classNamePlaceholder, classNamePlaceholder);
            VerifyFragmentsAtPosition(geData, 26, classNamePlaceholder, classNamePlaceholder);
            var classTextHyphen = classTextBlockBody.FragmentList[2];
            VerifyFragmentsAtPosition(geData, 27, classNamePlaceholder, classTextHyphen);
        }

        [Test(Description = "Tests if a position can accept keyboard input")]
        public void InputablePositionTest()
        {
            const string newProfileText = "`[Class>:Class:`Class.Name` - `Class.Title`\r\n\t`[Property>:`Property.Name` - `Property.Title``]`]";
            GeData geData = null;
            ValidateProfileTextPosition(true, 0, "refside start of segment", FragmentType.Segment, newProfileText, ref geData);
            ValidateProfileTextPosition(false, 1, "In segment prefix", FragmentType.Segment, newProfileText, ref geData);
            ValidateProfileTextPosition(false, 8, "In segment prefix", FragmentType.Segment, newProfileText, ref geData);
            ValidateProfileTextPosition(true, 9, "Start of text", FragmentType.Text, newProfileText, ref geData);
            ValidateProfileTextPosition(true, 10, "Text", FragmentType.Text, newProfileText, ref geData);
            ValidateProfileTextPosition(true, 15, "End of text", FragmentType.Placeholder, newProfileText, ref geData);
            ValidateProfileTextPosition(false, 16, "In placeholder", FragmentType.Placeholder, newProfileText, ref geData);
            ValidateProfileTextPosition(false, 26, "In placeholder", FragmentType.Placeholder, newProfileText, ref geData);
            ValidateProfileTextPosition(true, 27, "Start of text", FragmentType.Text, newProfileText, ref geData);
            ValidateProfileTextPosition(true, 94, "Between segment and containing segment", FragmentType.Segment, newProfileText, ref geData);
            ValidateProfileTextPosition(true, 58, "Between placeholder and containing segment end",
                FragmentType.Placeholder, newProfileText, ref geData);
            ValidateProfileTextPosition(true, 92, "Between placeholder and containing segment end",
                FragmentType.TextBlock, newProfileText, ref geData);
            ValidateProfileTextPosition(true, 96, "refside end of segment", FragmentType.Segment, newProfileText, ref geData);
        }

        [Test(Description = "Tests if text fragments get selected correctly")]
        public void TextSelectionTest()
        {
            const string newProfileText =
                "`[Class>:Class:`Class.Name` - `Class.Title`\r\n\t`[Property>:`Property.Name` - `Property.Title``]`]";
            var geData = LoadProfile(newProfileText);
            geData.Profile.Fragment = geData.Profile.Profile;
            geData.Profile.GetNodeProfileText();
            ValidateTextSelection(false, geData, 0, newProfileText.Length, "", "Whole profile text");
            ValidateTextSelection(true, geData, 9, 15, "Class:", "Whole text fragment");
            ValidateTextSelection(true, geData, 10, 14, "lass", "Whole text fragment");
            ValidateTextSelection(false, geData, 9, 16, "Class:`", "Text and partial placeholder");
            ValidateTextSelection(true, geData, 9, 27, "Class:`Class.Name`", "Text and placeholder");
            ValidateTextSelection(false, geData, 46, 94, "`[Property>:`Property.Name` - `Property.Title``]", "Segment");
            ValidateTextSelection(false, geData, 43, 74, "\r\n\t`[Property>:`Property.Name` ",
                "Outside container to inside container");
        }

        [Test(Description = "Tests if fragments get selected correctly"), TestCaseSource(typeof(ValidateFragmentSelectionParamsList))]
        public void FragmentSelectionTest(ValidateFragmentSelectionParams selectionParams)
        {
            ValidateFragmentSelection(selectionParams);
        }

        public class FragmentSelectionCutTestParamsList : List<FragmentSelectionCutTestParams>
        {
            public FragmentSelectionCutTestParamsList()
            {
                Add(new FragmentSelectionCutTestParams(
                    "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]", 
                    "Whole profile text",
                    0, 115, "", 
                    "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]"));
                Add(new FragmentSelectionCutTestParams(
                    "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Whole Property segment",
                    43, 94, "`[Class>:Class:`Class.Name` - `Class.Title`\r\nEnd `Class.Name` `]",
                    "`[Property>:\r\n\t`Property.Name` - `Property.Title``]"));
                Add(new FragmentSelectionCutTestParams(
                    "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Whole Text fragment",
                    9, 15, "`[Class>:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Class:"));
                Add(new FragmentSelectionCutTestParams(
                    "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "End of Text fragment",
                    10, 15, "`[Class>:C`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "lass:"));
                Add(new FragmentSelectionCutTestParams(
                    "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Start of Text fragment",
                    9, 14, 
                    "`[Class>::`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Class"));
                Add(new FragmentSelectionCutTestParams(
                    "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Substring of Text fragment",
                    10, 14,
                    "`[Class>:C:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "lass"));
                Add(new FragmentSelectionCutTestParams(
                    "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                    "Text and whole Property segment",
                    29, 94, "`[Class>:Class:`Class.Name` -\r\nEnd `Class.Name` `]",
                    " `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]"));
                Add(new FragmentSelectionCutTestParams(
                    "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
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

       [Test(Description = "Tests selection cut"), TestCaseSource(typeof(FragmentSelectionCutTestParamsList))]
        public void FragmentSelectionCutTest(FragmentSelectionCutTestParams cutParams)
        {
            var geData = LoadProfile(cutParams.NewProfileText);
            geData.Profile.Fragment = geData.Profile.Profile;
            geData.Profile.GetNodeProfileText();
           Assert.That(geData.Profile.IsSelectable(cutParams.Start, cutParams.End),
               cutParams.Comment + " - not selectable");
           var fragments = geData.Profile.GetSelection(cutParams.Start, cutParams.End);
           geData.Profile.Cut(fragments);
           Assert.AreEqual(cutParams.ExpectedText, geData.Profile.ProfileText, cutParams.Comment + " - Profile text after cut");
           Assert.AreEqual(cutParams.ExpectedSelectionText, fragments.ProfileText, cutParams.Comment + " - Cut text");
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
                       "Whole profile text", new[] {FragmentType.Segment}, "", "", ""));
               Add(
                   new ValidateFragmentSelectionParams(
                       "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                       true, 43, 94, "`[Property>:\r\n\t`Property.Name` - `Property.Title``]", "Whole Property segment",
                       new[] {FragmentType.Segment}, "", "", ""));
               Add(
                   new ValidateFragmentSelectionParams(
                       "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                       true, 9, 15, "Class:", "Whole Text fragment", new[] {FragmentType.Text}, "", "", ""));
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
                       "Text and whole Property segment", new[] {FragmentType.Placeholder, FragmentType.Segment}, " ",
                       "", ""));
               Add(
                   new ValidateFragmentSelectionParams(
                       "`[Class>:Class:`Class.Name` - `Class.Title``[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name` `]",
                       true, 43, 112, "`[Property>:\r\n\t`Property.Name` - `Property.Title``]\r\nEnd `Class.Name`",
                       "Whole Property segment and following text",
                       new[] {FragmentType.Segment, FragmentType.Text, FragmentType.Placeholder}, "", "", ""));
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

        private static void ValidateFragmentSelection(ValidateFragmentSelectionParams selectionParams)
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

        private void ValidateTextSelection(bool isSelectable, GeData geData, int start, int end, string expectedText,
            string comment)
        {
            Assert.AreEqual(geData.Profile.IsSelectable(start, end, true), isSelectable,
                "Is the specified text selectable? (" + comment + ")");
            if (!isSelectable) return;
            var fragments = geData.Profile.GetSelection(start, end);
        }

        private static void VerifyFragmentsAtPosition(GeData geData, int position, Fragment expectedBefore,
            Fragment expectedAfter)
        {
            Fragment before, after;
            geData.Profile.GetFragmentsAt(out before, out after, position);
            Assert.AreSame(expectedBefore, before, "Before position " + position);
            Assert.AreSame(expectedAfter, after, "After position " + position);
        }

        private static void ValidateProfileTextPosition(bool expected, int position, string message, FragmentType fragmentType, string newProfileText, ref GeData geData)
        {
            if (geData == null)
            {
                geData = LoadProfile(newProfileText);
                geData.Profile.Fragment = geData.Profile.Profile;
                var profileText = geData.Profile.GetNodeProfileText();
                Assert.AreEqual(newProfileText, profileText);
            }
            var pos = ((GeProfile)geData.Profile).ProfileTextPostionList.FindAtPosition(position);
            Assert.IsNotNull(pos);
            Assert.AreEqual(fragmentType, pos.Fragment.FragmentType);
            Assert.AreEqual(expected, geData.Profile.IsInputable(position), message);
        }

        private static GeData CreateNewProfile(string newProfileText, string fileGroup = "Definition",
            string newProfile = "NewProfile", string newProfileTitle = "New Profile Title")
        {
            var geData = SetUpGeData(fileGroup);
            geData.Profile.CreateNewProfile(newProfile, newProfileTitle, newProfileText);
            geData.Profile.GetNodeProfileText();
            return geData;
        }

        private static GeData LoadProfile(string newProfileText, string fileGroup = "Definition",
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

        private static void VerifyProfile(GeData geData, string newProfileText, string expectedProfileText)
        {
            Assert.AreEqual(newProfileText, geData.Profile.GetNodeExpansionText(geData.GenDataBase, null));
            var nodeProfileText = geData.Profile.GetNodeProfileText();
            Assert.AreEqual(expectedProfileText, nodeProfileText);
            Assert.AreSame(nodeProfileText, geData.Profile.ProfileText);
        }

        /// <summary>
        ///     Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
            GenDataLoader.Register();
        }

        /// <summary>
        ///     Tear down the Generator data definition tests
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
        }
    }
}