// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the profile editor functionality
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
            var textBlock = (TextBlock)((Segment)geData.Profile.Fragment).Body().FragmentList[0];
            ValidateProfileTextPosition(true, geData, 22, "End of text before substitution", FragmentType.Text);
            ValidateProfileTextPosition(true, geData, 23, "Substituted placholder position", FragmentType.Text);
            VerifyFragmentsAtPosition(geData, 22, textBlock.Body().FragmentList[0], textBlock.Body().FragmentList[0]);
            geData.Profile.SubstitutePlaceholder(textBlock, "Class", geData.GenDataDef.GetId("Class.Name"));
            ValidateProfileTextPosition(true, geData, 22, "End of text before substitution", FragmentType.Placeholder);
            ValidateProfileTextPosition(false, geData, 23, "Substituted placholder", FragmentType.Placeholder);
            VerifyFragmentsAtPosition(geData, 22, textBlock.Body().FragmentList[0], textBlock.Body().FragmentList[1]);
            VerifyProfile(geData, newProfileText, expectedProfileText);
            geData.Undo();
            ValidateProfileTextPosition(true, geData, 22, "End of text before substitution", FragmentType.Text);
            ValidateProfileTextPosition(true, geData, 23, "Substituted placholder position", FragmentType.Text);
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
            var textBlock = (TextBlock)((Segment)geData.Profile.Fragment).Body().FragmentList[0];

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
            var textBlock = (TextBlock)((Segment)geData.Profile.Fragment).Body().FragmentList[0];

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
            const string newProfileText =
                  "`[Class>:Class:`Class.Name` - `Class.Title`\r\n\t`[Property>:`Property.Name` - `Property.Title``]`]";
            var geData = LoadProfile(newProfileText);
            geData.Profile.Fragment = geData.Profile.Profile;
            var profileText = geData.Profile.GetNodeProfileText();
            Assert.AreEqual(newProfileText, profileText);
            ValidateProfileTextPosition(true,  geData,  0, "Outside segment", FragmentType.Segment);
            ValidateProfileTextPosition(false, geData,  1, "In segment prefix", FragmentType.Segment);
            ValidateProfileTextPosition(false, geData,  8, "In segment prefix", FragmentType.Segment);
            ValidateProfileTextPosition(true,  geData,  9, "Start of text", FragmentType.Text);
            ValidateProfileTextPosition(true,  geData, 10, "Text", FragmentType.Text);
            ValidateProfileTextPosition(true,  geData, 15, "End of text", FragmentType.Placeholder);
            ValidateProfileTextPosition(false, geData, 16, "In placeholder", FragmentType.Placeholder);
            ValidateProfileTextPosition(false, geData, 26, "In placeholder", FragmentType.Placeholder);
            ValidateProfileTextPosition(true,  geData, 27, "Start of text", FragmentType.Text);
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
            ValidateTextSelection(false, geData, 43, 74, "\r\n\t`[Property>:`Property.Name` ", "Outside container to inside container");
        }

        private void ValidateTextSelection(bool isSelectable, GeData geData, int start, int end, string expectedText, string comment)
        {
            Assert.AreEqual(geData.Profile.IsSelectable(start, end, true), isSelectable, "Is the specified text selectable? (" + comment + ")");
        }

        private static void VerifyFragmentsAtPosition(GeData geData, int position, Fragment expectedBefore,
            Fragment expectedAfter)
        {
            Fragment before, after;
            geData.Profile.GetFragmentsAt(out before, out after, position);
            Assert.AreSame(expectedBefore, before, "Before position " + position);
            Assert.AreSame(expectedAfter, after, "After position " + position);
        }

        private static void ValidateProfileTextPosition(bool expected, GeData geData, int position, string message, FragmentType fragmentType)
        {
            var pos = ((GeProfile) geData.Profile).ProfileTextPostionList.FindAtPosition(position);
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
        /// Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
            GenDataLoader.Register();
        }

        /// <summary>
        /// Tear down the Generator data definition tests
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {

        }
    }
}
