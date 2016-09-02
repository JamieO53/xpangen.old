// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    ///     Tests the Generator correctly scans and expands profiles
    /// </summary>
    [TestFixture]
    public class GenCompactProfileTest : GenProfileFragmentsTestBase
    {
        /// <summary>
        ///     Tests that a text only profile is scanned and expanded correctly
        /// </summary>
        [Test(Description = "Text Profile test")]
        public void TextProfileTest()
        {
            const string txt = "Text to scan";
            const string expectedExpansion = txt;
            var expectedFragmentType = new[] { FragmentType.TextBlock };
            const string message = "Original text expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);
        }

        /// <summary>
        ///     Tests that a profile consiting of a segement is scanned and expanded correctly
        /// </summary>
        [Test(Description = "Segment Profile Test")]
        public void SegmentProfileTest()
        {
            const string txt = "`[Class>:`Class.Name`,`]";
            const string expectedExpansion = "Class,SubClass,Property,";
            var expectedFragmentType = new []{FragmentType.Segment} ;
            const string message = "Class list expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);
        }

        private static GenProfileFragment ValidateExpansion(string txt, FragmentType[] expectedFragmentTypes,
            string expectedExpansion, string message, string expectedProfileText = null)
        {
            var d = SetUpData();
            return ValidateExpansion(d, txt, expectedFragmentTypes, expectedExpansion, message, expectedProfileText);
        }

        private static GenProfileFragment ValidateExpansion(GenDataBase d, string txt, FragmentType[] expectedFragmentTypes,
            string expectedExpansion, string message, string expectedProfileText = null)
        {
            var expected = expectedProfileText ?? txt;
            var profile = new GenCompactProfileParser(d.GenDataDef, "", txt);
            Assert.AreEqual(expectedFragmentTypes.Count(), profile.Body.Count, "Number of fragments expected");
            for (var i = 0; i < profile.Body.Count; i++)
                Assert.AreEqual(expectedFragmentTypes[i], profile.Body.Fragment[i].FragmentType);
            Assert.AreEqual(expected,
                profile.ProfileText(
                    ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual(expectedExpansion,
                GenFragmentExpander.Expand(d.GenDataDef, d.Root, profile.Fragment),
                message);
            return profile;
        }

        /// <summary>
        ///     Tests that a profile consisting of a segment embedded in text is scanned and expanded correctly
        /// </summary>
        [Test(Description = "Segment Text Profile test")]
        public void SegmentTextProfileTest()
        {
            const string txt = "Some text `[Class>:`Class.Name`,`] some more text";
            const string expectedExpansion = "Some text Class,SubClass,Property, some more text";
            var expectedFragmentType = new[] {FragmentType.TextBlock, FragmentType.Segment, FragmentType.TextBlock};
            const string message = "Class list in text expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);
        }

        /// <summary>
        ///     Tests that a profile consisting of a delimited list segment is scanned and expanded correctly
        /// </summary>
        [Test(Description = "Segment List Profile test")]
        public void SegmentListProfileTest()
        {
            const string txt = "`[Class/:`Class.Name``;,`]";
            const string expectedExpansion = "Class,SubClass,Property";
            var expectedFragmentType = new[] {FragmentType.Segment};
            const string message = "Class list expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);
        }

        /// <summary>
        ///     Tests that a profile consisting of a block is scanned and expanded correctly
        /// </summary>
        [Test(Description = "Block Profile test")]
        public void BlockProfileTest()
        {
            const string txt = "`{Block text`]";
            const string expectedExpansion = "Block text";
            var expectedFragmentType = new[] { FragmentType.Block };
            const string message = "Block text expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);
        }

        /// <summary>
        ///     Tests that a profile consisting of an annotation is scanned and expanded correctly
        /// </summary>
        [Test(Description = "Annotation Profile test")]
        public void AnnotationProfileTest()
        {
            const string txt = "`-Annotation text`]";
            const string expectedExpansion = "";
            var expectedFragmentType = new[] { FragmentType.Annotation };
            const string message = "Empty annotation text expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);
        }

        /// <summary>
        ///     Tests that a profile consiting of a lookup is scanned and expanded correctly
        /// </summary>
        [Test(Description = "Lookup Profile test")]
        public void LookupProfileTest()
        {
            const string txt =
                "`[Class/:`Class.Name`{`[SubClass/:`%Class.Name=SubClass.Name:`Class.Name``]`;,`]}`;,`]";
            const string expectedExpansion = "Class{SubClass,Property},SubClass{},Property{}";
            var expectedFragmentType = new[] { FragmentType.Segment };
            const string message = "Class/Subclass list expected";
            var profile = ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);

            var nestedClass = ((GenSegment) profile.Body.Fragment[0]);
            Assert.AreEqual(3, nestedClass.Body.Count, "Three nested fragments expected");
            Assert.AreEqual(1, nestedClass.Body.SecondaryCount, "One secondary nested fragment expected");
            Assert.AreEqual(FragmentType.TextBlock, nestedClass.Body.Fragment[0].FragmentType);
            Assert.AreEqual(FragmentType.Segment, nestedClass.Body.Fragment[1].FragmentType);
            var nestedSubClass = ((GenSegment) nestedClass.Body.Fragment[1]);
            Assert.AreEqual(1, nestedSubClass.Body.Count, "One nested fragment expected");
            Assert.AreEqual(1, nestedSubClass.Body.SecondaryCount, "One secondary nested fragments expected");
            Assert.AreEqual(FragmentType.Lookup, nestedSubClass.Body.Fragment[0].FragmentType);
            Assert.IsFalse(((GenLookup) nestedSubClass.Body.Fragment[0]).NoMatch, "No match property for lookup");
            Assert.AreEqual(FragmentType.TextBlock, nestedClass.Body.Fragment[2].FragmentType);
            Assert.AreEqual(FragmentType.TextBlock, nestedClass.Body.SecondaryFragment[0].FragmentType);
        }

        /// <summary>
        ///     Tests that a profile consisting of a no-match lookup is scanned and expanded correctly
        /// </summary>
        [Test(Description = "No Match Profile test")]
        public void NoMatchProfileTest()
        {
            const string txt =
                "`[Class/:`Class.Name`{`[SubClass/:`%Class.Name=SubClass.Name:`;`Class.Name``]`;,`]}`;,`]";
            const string expectedExpansion = "Class{},SubClass{},Property{}";
            var expectedFragmentType = new[] { FragmentType.Segment };
            const string message = "Class/Subclass list expected";
            var profile = ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);

            var nestedClass = ((GenSegment) profile.Body.Fragment[0]);
            Assert.AreEqual(3, nestedClass.Body.Count, "Three nested fragments expected");
            Assert.AreEqual(FragmentType.TextBlock, nestedClass.Body.Fragment[0].FragmentType);
            Assert.AreEqual(FragmentType.Segment, nestedClass.Body.Fragment[1].FragmentType);
            var nestedSubClass = ((GenSegment) nestedClass.Body.Fragment[1]);
            Assert.AreEqual(1, nestedSubClass.Body.Count, "One nested fragment expected");
            Assert.AreEqual(1, nestedSubClass.Body.SecondaryCount, "One nested separator fragment expected");
            Assert.AreEqual(FragmentType.Lookup, nestedSubClass.Body.Fragment[0].FragmentType);
            Assert.IsTrue(((GenLookup) nestedSubClass.Body.Fragment[0]).Body.SecondaryCount > 0,
                "No match property for no match lookup");
            Assert.AreEqual(FragmentType.TextBlock, nestedClass.Body.Fragment[2].FragmentType);
            Assert.AreEqual(FragmentType.TextBlock, nestedClass.Body.SecondaryFragment[0].FragmentType);
        }

        /// <summary>
        ///     Tests a profile consisting of existence and non-existence condition tests expands correctly
        /// </summary>
        [Test(Description = "Existence Condition Profile test")]
        public void ExistenceConditionProfileTest()
        {
            const string txt = "`[Class':" +
                               "`?Class.Name:Name exists `]" +
                               "`?Class.Name~:Name does not exist `]" +
                               "`?Class.NoName:NoName exists `]" +
                               "`?Class.NoName~:NoName does not exist `]`]";
            const string expectedExpansion = "Name exists NoName does not exist ";
            var expectedFragmentType = new[] { FragmentType.Segment };
            const string message = "Existence conditions expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);
        }

        /// <summary>
        ///     Tests that a profile consisting of a combination literal comparison conditions is scanned and expanded correctly
        /// </summary>
        [Test(Description = "Literal Condition Profile test")]
        public void LiteralConditionProfileTest()
        {
            const string txt = "`[Class':" +
                               "`?Class.Name<Class:Class name is less than Class `]" +
                               "`?Class.Name=Class:Class name equals Class `]" +
                               "`?Class.Name>Class:Class name is greater than Class `]" +
                               "`?Class.Name<Clasa:Class name is less than Clasa `]" +
                               "`?Class.Name=Clasa:Class name equals Clasa `]" +
                               "`?Class.Name>Clasa:Class name is greater than Clasa `]" +
                               "`?Class.Name<Clasz:Class name is less than Clasz `]" +
                               "`?Class.Name=Clasz:Class name equals Clasz `]" +
                               "`?Class.Name>Clasz:Class name is greater than Clasz `]" +
                               "`]";
            const string expectedExpansion = "Class name equals Class " +
                            "Class name is greater than Clasa " +
                            "Class name is less than Clasz ";
            var expectedFragmentType = new[] { FragmentType.Segment };
            const string message = "Comparison conditions expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);
        }

        /// <summary>
        ///     Tests that a profile consisting of a combination of identifier comparisons is scanned and expanded correctly
        /// </summary>
        [Test(Description = "Identifier Condition Profile test")]
        public void IdentifierConditionProfileTest()
        {
            const string txt = "`[Class':" +
                               "`?Class.Name<Class.NameEQ:Class name is less than Class `]" +
                               "`?Class.Name=Class.NameEQ:Class name equals Class `]" +
                               "`?Class.Name>Class.NameEQ:Class name is greater than Class `]" +
                               "`?Class.Name<Class.NameLT:Class name is less than Clasa `]" +
                               "`?Class.Name=Class.NameLT:Class name equals Clasa `]" +
                               "`?Class.Name>Class.NameLT:Class name is greater than Clasa `]" +
                               "`?Class.Name<Class.NameGT:Class name is less than Clasz `]" +
                               "`?Class.Name=Class.NameGT:Class name equals Clasz `]" +
                               "`?Class.Name>Class.NameGT:Class name is greater than Clasz `]" +
                               "`]";
            var f = GenDataDef.CreateMinimal();
            f.AddClassInstanceProperty(ClassClassId, "NameLT");
            f.AddClassInstanceProperty(ClassClassId, "NameEQ");
            f.AddClassInstanceProperty(ClassClassId, "NameGT");
            f.AddClassInstanceProperty(ClassClassId, "NameBlank");

            var d = SetUpData(f);

            var c = GetFirstObject(d);
            Assert.AreEqual("Class", c.Attributes[0]);
            var a = new GenAttributes(f, ClassClassId) {GenObject = c};
            a.SetString("NameLT", "Clasa");
            a.SetString("NameEQ", "Class");
            a.SetString("NameGT", "Clasz");
            a.SetString("NameBlank", "");
            a.SaveFields();

            const string expectedExpansion = "Class name equals Class " +
                            "Class name is greater than Clasa " +
                            "Class name is less than Clasz ";
            var expectedFragmentType = new[] { FragmentType.Segment };
            const string message = "Comparison conditions expected";
            ValidateExpansion(d, txt, expectedFragmentType, expectedExpansion, message);
        }

        private new static GenDataBase SetUpData()
        {
            return SetUpData(GenDataDef.CreateMinimal());
        }

        private static GenDataBase SetUpData(GenDataDef f)
        {
            var d = new GenDataBase(f);
            SetUpData(d);
            return d;
        }

        /// <summary>
        ///     Tests that a profile consisting of a prameterless function invocation is scanned and expanded correctly
        /// </summary>
        [Test(Description = "No Param Function Profile test")]
        public void NoParamFunctionProfileTest()
        {
            const string txt = "`@Date:`]";
            var expectedExpansion = DateTime.Today.ToLongDateString();
            var expectedFragmentType = new[] { FragmentType.Function };
            const string message = "Function output expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);
        }

        /// <summary>
        ///     Tests that a profile consisting of a single prameter function invocation is scanned and expanded correctly
        /// </summary>
        [Test(Description = "Single Param Function Profile test")]
        public void SingleParamFunctionProfileTest()
        {
            const string txt = "`@File:`{filename.txt`]`]";
            const string expectedExpansion = "\r\n //File: filename.txt";
            var expectedFragmentType = new[] { FragmentType.Function };
            const string message = "Function output expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);
        }

        /// <summary>
        ///     Tests that a profile cosnsiting of a sequence of functions is scanned and expanded correctly
        /// </summary>
        [Test(Description = "Function Profile test")]
        public void FunctionProfileTest()
        {
            const string txt =
                "`@Set:`{Val`] `{5`]`]" + // Saves 5 in Val
                // Blank between parameters for comparison
                "`@Get:`{Val`]`]" + // Returns Val (i.e. 5)
                "`@Add:`{Val`] `{5`]`]" + // Adds 5 to Val
                // Blank between parameters for comparison
                " " + // Pads with a space for readability
                "`@Get:`{Val`]`]"; // Returns Val (i.e. 10)
            const string expectedExpansion = "5 10";
            var expectedFragmentType = new[]
                                       {
                                           FragmentType.Function, FragmentType.Function, FragmentType.Function,
                                           FragmentType.TextBlock, FragmentType.Function
                                       };
            const string message = "Function output expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message);
        }

        /// <summary>
        ///     Tests that a profile consisting of a sequence of function invocations with variously formatted literal parameters
        ///     are scanned and expanded correctly
        /// </summary>
        [Test(Description = "Literal Param Function Profile test")]
        public void LiteralParamFunctionProfileTest()
        {
            const string txt =
                "`@Set:'Val' '5'`]" + // Saves 5 in Val
                // Blank between parameters for comparison
                "`@Get:'Val'`]" + // Returns Val (i.e. 5)
                "`@Add:'Val' '5'`]" + // Adds 5 to Val
                // Blank between parameters for comparison
                " " + // Pads with a space for readability
                "`@Get:'Val'`]"; // Returns Val (i.e. 10)
            const string expected =
                "`@Set:`{Val`] `{5`]`]" + // Saves 5 in Val
                // Blank between parameters for comparison
                "`@Get:`{Val`]`]" + // Returns Val (i.e. 5)
                "`@Add:`{Val`] `{5`]`]" + // Adds 5 to Val
                // Blank between parameters for comparison
                " " + // Pads with a space for readability
                "`@Get:`{Val`]`]"; // Returns Val (i.e. 10)
            const string expectedExpansion = "5 10";
            var expectedFragmentType = new[]// `@Set:`{Val`] `{5`]`]`@Get:`{Val`]`]`@Add:`{Val`] `{5`]`] 
                                       {
                                           FragmentType.Function, FragmentType.Function, FragmentType.Function,
                                           FragmentType.TextBlock, FragmentType.Function
                                       };
            const string message = "Function output expected";
            ValidateExpansion(txt, expectedFragmentType, expectedExpansion, message, expected);
            //var d = SetUpData();
            //var profile = new GenCompactProfileParser(d.GenDataDef, "", txt);
            //Assert.AreEqual(5, profile.Body.Count, "Four function calls and one blanks");
            //Assert.AreEqual(FragmentType.Function, profile.Body.Fragment[0].FragmentType);
            //Assert.AreEqual(expected,
            //    profile.ProfileText(
            //        ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            //Assert.AreEqual("5 10",
            //    GenFragmentExpander.Expand(d.GenDataDef, d.Root, profile.Fragment),
            //    "Function output expected");
        }

        /// <summary>
        ///     Tests that a profile saved to a file is correctly scanned and expanded from the file
        /// </summary>
        [Test(Description = "File Stream Profile test")]
        public void FileStreamProfileTest()
        {
            const string txt = "`[Class>:`Class.Name`,`]";
            const string fileName = "GenProfileTest.prf";
            var d = SetUpData();

            if (File.Exists(fileName))
                File.Delete(fileName);

            File.WriteAllText(fileName, txt);

            var profile = new GenCompactProfileParser(d.GenDataDef, fileName, "");
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Segment, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt,
                profile.ProfileText(
                    ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("Class,SubClass,Property,",
                GenFragmentExpander.Expand(d.GenDataDef, d.Root, profile.Fragment),
                "Class list expected");
        }

        /// <summary>
        ///     Tests that a profile saved to a new directory is saved correctly
        /// </summary>
        [Test(Description = "New Directory File Stream Profile test")]
        public void DirectoryFileStreamProfileTest()
        {
            const string dir = "TestDir";
            const string fileName = dir + @"\GenProfileTest.dcb";
            var f = GenDataDef.CreateMinimal();
            var d = f.AsGenDataBase();

            if (File.Exists(fileName))
                File.Delete(fileName);
            if (Directory.Exists(dir))
                Directory.Delete(dir);

            GenParameters.SaveToFile(d, fileName);
            Assert.IsTrue(Directory.Exists(dir), "Output directory is not created.");
            Assert.IsTrue(File.Exists(fileName));
            GenParameters d1;
            using (var stream = new FileStream(fileName, FileMode.Open))
                d1 = new GenParameters(stream) {DataName = "GenProfileTest"};

            VerifyDataCreation(d1);
        }

        /// <summary>
        ///     Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
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