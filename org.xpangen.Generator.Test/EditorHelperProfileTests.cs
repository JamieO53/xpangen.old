// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    ///     Tests the profile editor functionality
    /// </summary>
    [TestFixture]
    public class EditorHelperProfileTests : EditorHelperProfileTestsBase
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
            ValidateProfileTextPosition(new ProfileTextPositionParams(true, 22, "End of text before substitution", FragmentType.Text, expectedUndoProfileText), ref geData);
            ValidateProfileTextPosition(new ProfileTextPositionParams(true, 23, "Substituted placholder position", FragmentType.Text, expectedUndoProfileText), ref geData);
            VerifyFragmentsAtPosition(geData, 22, textBlock.Body().FragmentList[0], textBlock.Body().FragmentList[0]);
            VerifyProfile(geData, newProfileText, expectedUndoProfileText);
            geData.Profile.SubstitutePlaceholder(textBlock, "Class", geData.GenDataDef.GetId("Class.Name"));
            ValidateProfileTextPosition(new ProfileTextPositionParams(true, 22, "End of text before substitution", FragmentType.Placeholder, expectedProfileText), ref geData);
            ValidateProfileTextPosition(new ProfileTextPositionParams(false, 23, "Substituted placholder", FragmentType.Placeholder, expectedProfileText), ref geData);
            VerifyFragmentsAtPosition(geData, 22, textBlock.Body().FragmentList[0], textBlock.Body().FragmentList[1]);
            VerifyProfile(geData, newProfileText, expectedProfileText);
            geData.Undo();
            ValidateProfileTextPosition(new ProfileTextPositionParams(true, 22, "End of text before substitution", FragmentType.Text, expectedUndoProfileText), ref geData);
            ValidateProfileTextPosition(new ProfileTextPositionParams(true, 23, "Substituted placholder position", FragmentType.Text, expectedUndoProfileText), ref geData);
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

        [Test(Description = "Tests if a position can accept keyboard input"), TestCaseSource(typeof(ProfileTextPositionParamsList))]
        public void InputablePositionTest(ProfileTextPositionParams positionParams)
        {
            ValidateProfileTextPosition(positionParams);
        }

        [Test(Description = "Tests if text fragments get selected correctly"), TestCaseSource(typeof(TextSelectionParamsList))]
        public void TextSelectionTest(TextSelectionParams selectionParams)
        {
            ValidateTextSelection(selectionParams);
        }

        [Test(Description = "Tests if fragments get selected correctly"), TestCaseSource(typeof(ValidateFragmentSelectionParamsList))]
        public void FragmentSelectionTest(ValidateFragmentSelectionParams selectionParams)
        {
            ValidateFragmentSelection(selectionParams);
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

       [Test(Description = "Tests selection insertion"), TestCaseSource(typeof(FragmentInsertTestParamsList))]
       public void FragmentInsertTest(FragmentInsertTestParams insertParams)
       {
           var geData = LoadProfile(insertParams.NewProfileText);
           geData.Profile.Fragment = geData.Profile.Profile;
           geData.Profile.GetNodeProfileText();
           var position = insertParams.Position;
           Assert.That(geData.Profile.IsInputable(position),
               insertParams.Comment + " - not selectable");
           var text = insertParams.ExpectedSelectionText;
           geData.Profile.Insert(position, text);
           Assert.AreEqual(insertParams.ExpectedText, geData.Profile.ProfileText,
               insertParams.Comment + " - Profile text after insertion");
           Assert.AreEqual(text, geData.Profile.ProfileText.Substring(position, text.Length),
               insertParams.Comment + " - Inserted text in Profile");
           //var fragments = geData.Profile.GetSelection(position, position + text.Length);
           //Assert.AreEqual(text, fragments.ProfileText, insertParams.Comment + " - Insert text");
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