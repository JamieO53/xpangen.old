// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the Generator correctly scans and expands profiles
    /// </summary>
	[TestFixture]
    public class GenCompactProfileTest : GenProfileFragmentsTestBase
    {
        /// <summary>
        /// Tests that a text only profile is scanned and expanded correctly
        /// </summary>
        [TestCase(Description="Text Profile test")]
        public void TextProfileTest()
        {
            const string txt = "Text to scan";
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Text, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt, profile.Body.Expand(d), "Original text expected");
        }

        /// <summary>
        /// Tests that a profile consiting of a segement is scanned and expanded correctly
        /// </summary>
        [TestCase(Description="Segment Profile Test")]
        public void SegmentProfileTest()
        {
            const string txt = "`[Class>:`Class.Name`,`]";
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Segment, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("Class,SubClass,Property,", profile.Expand(d), "Class list expected");
        }

        /// <summary>
        /// Tests that a profile consisting of a segment embedded in text is scanned and expanded correctly
        /// </summary>
        [TestCase(Description="Segment Text Profile test")]
        public void SegmentTextProfileTest()
        {
            const string txt = "Some text `[Class>:`Class.Name`,`] some more text";
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(3, profile.Body.Count, "Three fragments expected");
            Assert.AreEqual(FragmentType.Text, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(FragmentType.Segment, profile.Body.Fragment[1].FragmentType);
            Assert.AreEqual(FragmentType.Text, profile.Body.Fragment[2].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("Some text Class,SubClass,Property, some more text", profile.Expand(d),
                            "Class list in text expected");
        }

        /// <summary>
        /// Tests that a profile consisting of a delimited list segment is scanned and expanded correctly
        /// </summary>
        [TestCase(Description="Segment List Profile test")]
        public void SegmentListProfileTest()
        {
            const string txt = "`[Class/:`Class.Name`,`]";
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Segment, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("Class,SubClass,Property", profile.Expand(d), "Class list expected");
        }

        /// <summary>
        /// Tests that a profile consisting of a block is scanned and expanded correctly
        /// </summary>
        [TestCase(Description="Block Profile test")]
        public void BlockProfileTest()
        {
            const string txt = "`{Block text`]";
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Block, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("Block text", profile.Expand(d), "Block text expected");
        }

        /// <summary>
        /// Tests that a profile consiting of a lookup is scanned and expanded correctly
        /// </summary>
        [TestCase(Description="Lookup Profile test")]
        public void LookupProfileTest()
        {
            const string txt = "`[Class/:`Class.Name`{`[SubClass/:`%Class.Name=SubClass.Name:`Class.Name``],`]}`{,`]`]";
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Segment, profile.Body.Fragment[0].FragmentType);
            var nestedClass = ((GenSegment) profile.Body.Fragment[0]);
            Assert.AreEqual(5, nestedClass.Body.Count, "Five nested fragments expected");
            Assert.AreEqual(FragmentType.Placeholder, nestedClass.Body.Fragment[0].FragmentType);
            Assert.AreEqual(FragmentType.Text, nestedClass.Body.Fragment[1].FragmentType);
            Assert.AreEqual(FragmentType.Segment, nestedClass.Body.Fragment[2].FragmentType);
            var nestedSubClass = ((GenSegment) nestedClass.Body.Fragment[2]);
            Assert.AreEqual(2, nestedSubClass.Body.Count, "Two nested fragments expected");
            Assert.AreEqual(FragmentType.Lookup, nestedSubClass.Body.Fragment[0].FragmentType);
            Assert.IsFalse(((GenLookup) nestedSubClass.Body.Fragment[0]).NoMatch, "No match property for lookup");
            Assert.AreEqual(FragmentType.Text, nestedClass.Body.Fragment[3].FragmentType);
            Assert.AreEqual(FragmentType.Block, nestedClass.Body.Fragment[4].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("Class{SubClass,Property},SubClass{},Property{}", profile.Expand(d), "Class/Subclass list expected");
        }

        /// <summary>
        /// Tests that a profile consisting of a no-match lookup is scanned and expanded correctly
        /// </summary>
        [TestCase(Description="No Match Profile test")]
        public void NoMatchProfileTest()
        {
            const string txt = "`[Class/:`Class.Name`{`[SubClass/:`&Class.Name=SubClass.Name:`Class.Name``],`]}`{,`]`]";
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Segment, profile.Body.Fragment[0].FragmentType);
            var nestedClass = ((GenSegment)profile.Body.Fragment[0]);
            Assert.AreEqual(5, nestedClass.Body.Count, "Five nested fragments expected");
            Assert.AreEqual(FragmentType.Placeholder, nestedClass.Body.Fragment[0].FragmentType);
            Assert.AreEqual(FragmentType.Text, nestedClass.Body.Fragment[1].FragmentType);
            Assert.AreEqual(FragmentType.Segment, nestedClass.Body.Fragment[2].FragmentType);
            var nestedSubClass = ((GenSegment)nestedClass.Body.Fragment[2]);
            Assert.AreEqual(2, nestedSubClass.Body.Count, "Two nested fragments expected");
            Assert.AreEqual(FragmentType.Lookup, nestedSubClass.Body.Fragment[0].FragmentType);
            Assert.IsTrue(((GenLookup)nestedSubClass.Body.Fragment[0]).NoMatch, "No match property for no match lookup");
            Assert.AreEqual(FragmentType.Text, nestedClass.Body.Fragment[3].FragmentType);
            Assert.AreEqual(FragmentType.Block, nestedClass.Body.Fragment[4].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("Class{},SubClass{},Property{}", profile.Expand(d), "Class/Subclass list expected");
        }

        /// <summary>
        /// Tests a profile consisting of existence and non-existence condition tests expands correctly
        /// </summary>
        [TestCase(Description="Existence Condition Profile test")]
        public void ExistenceConditionProfileTest()
        {
            const string txt = "`[Class':" +
                "`?Class.Name:Name exists `]" +
                "`?Class.Name~:Name does not exist `]" +
                "`?Class.NoName:NoName exists `]" +
                "`?Class.NoName~:NoName does not exist `]`]";
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Segment, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("Name exists NoName does not exist ", profile.Expand(d), "Existence conditions expected");
        }

        /// <summary>
        /// Tests that a profile consisting of a combination literal comparison conditions is scanned and expanded correctly
        /// </summary>
        [TestCase(Description="Literal Condition Profile test")]
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
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Segment, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("Class name equals Class " +
                "Class name is greater than Clasa " +
                "Class name is less than Clasz ",
                profile.Expand(d), "Comparison conditions expected");
        }

        /// <summary>
        /// Tests that a profile consisting of a combination of identifier comparisons is scanned and expanded correctly
        /// </summary>
        [TestCase(Description="Identifier Condition Profile test")]
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
            f.Properties[ClassClassId].Add("NameLT");
            f.Properties[ClassClassId].Add("NameEQ");
            f.Properties[ClassClassId].Add("NameGT");
            f.Properties[ClassClassId].Add("NameBlank");

            var d = new GenData(f);
            SetUpData(d);

            d.First(ClassClassId);
            Assert.AreEqual("Class", d.Context[ClassClassId].Context.Attributes[0]);
            var a = new GenAttributes(f) { GenObject = d.Context[ClassClassId].Context };
            a.SetString("NameLT", "Clasa");
            a.SetString("NameEQ", "Class");
            a.SetString("NameGT", "Clasz");
            a.SetString("NameBlank", "");
            a.SaveFields();

            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Segment, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("Class name equals Class " +
                "Class name is greater than Clasa " +
                "Class name is less than Clasz ",
                profile.Expand(d), "Comparison conditions expected");
        }

        /// <summary>
        /// Tests that a profile consisting of a prameterless function invocation is scanned and expanded correctly
        /// </summary>
        [TestCase(Description = "No Param Function Profile test")]
        public void NoParamFunctionProfileTest()
        {
            const string txt =
                "`@Date:`]";
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Function, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual(DateTime.Today.ToLongDateString(), profile.Expand(d), "Function output expected");
        }

        /// <summary>
        /// Tests that a profile consisting of a single prameter function invocation is scanned and expanded correctly
        /// </summary>
        [TestCase(Description = "Single Param Function Profile test")]
        public void SingleParamFunctionProfileTest()
        {
            const string txt =
                "`@File:`{filename.txt`]`]";
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Function, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("\r\n //File: filename.txt", profile.Expand(d), "Function output expected");
        }

        /// <summary>
        /// Tests that a profile cosnsiting of a sequence of functions is scanned and expanded correctly
        /// </summary>
        [TestCase(Description="Function Profile test")]
        public void FunctionProfileTest()
        {
            const string txt =
                "`@Set:`{Val`] `{5`]`]" +   // Saves 5 in Val
                                            // Blank between parameters for comparison
                "`@Get:`{Val`]`]" +         // Returns Val (i.e. 5)
                "`@Add:`{Val`] `{5`]`]" +   // Adds 5 to Val
                                            // Blank between parameters for comparison
                " " +                       // Pads with a space for readability
                "`@Get:`{Val`]`]";          // Returns Val (i.e. 10)
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(5, profile.Body.Count, "Four function calls and a blank");
            Assert.AreEqual(FragmentType.Function, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("5 10", profile.Expand(d), "Function output expected");
        }

        /// <summary>
        /// Tests that a profile consisting of a sequence of function invocations with variously formatted literal parameters are scanned and expanded correctly
        /// </summary>
        [TestCase(Description="Literal Param Function Profile test")]
        public void LiteralParamFunctionProfileTest()
        {
            const string txt =
                "`@Set:'Val' '5'`]" +   // Saves 5 in Val
                                        // Blank between parameters for comparison
                "`@Get:'Val'`]" +       // Returns Val (i.e. 5)
                "`@Add:'Val' '5'`]" +   // Adds 5 to Val
                                        // Blank between parameters for comparison
                " " +                   // Pads with a space for readability
                "`@Get:'Val'`]";        // Returns Val (i.e. 10)
            const string expected =
                "`@Set:`{Val`] `{5`]`]" +   // Saves 5 in Val
                // Blank between parameters for comparison
                "`@Get:`{Val`]`]" +         // Returns Val (i.e. 5)
                "`@Add:`{Val`] `{5`]`]" +   // Adds 5 to Val
                // Blank between parameters for comparison
                " " +                       // Pads with a space for readability
                "`@Get:`{Val`]`]";          // Returns Val (i.e. 10)
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            var profile = new GenCompactProfileParser(d, "", txt);
            Assert.AreEqual(5, profile.Body.Count, "Four function calls and one blanks");
            Assert.AreEqual(FragmentType.Function, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(expected,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("5 10", profile.Expand(d), "Function output expected");
        }

        /// <summary>
        /// Tests that a profile saved to a file is correctly scanned and expanded from the file
        /// </summary>
        [TestCase(Description="File Stream Profile test")]
        public void FileStreamProfileTest()
        {
            const string txt = "`[Class>:`Class.Name`,`]";
            const string fileName = "GenProfileTest.prf";
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            SetUpData(d);
            
            if (File.Exists(fileName))
                File.Delete(fileName);
            
            File.WriteAllText(fileName, txt);
            
            var profile = new GenCompactProfileParser(d, fileName, "");
            Assert.AreEqual(1, profile.Body.Count, "Only one fragment expected");
            Assert.AreEqual(FragmentType.Segment, profile.Body.Fragment[0].FragmentType);
            Assert.AreEqual(txt,
                            profile.Body.ProfileText(
                                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));
            Assert.AreEqual("Class,SubClass,Property,", profile.Expand(d), "Class list expected");
        }

        /// <summary>
        /// Tests that a profile saved to a new directory is saved correctly
        /// </summary>
        [TestCase(Description = "New Directory File Stream Profile test")]
        public void DirectoryFileStreamProfileTest()
        {
            const string dir = "TestDir";
            const string fileName = dir + @"\GenProfileTest.dcb";
            var f = GenDataDef.CreateMinimal();
            var d = f.AsGenData();

            if (File.Exists(fileName))
            File.Delete(fileName);
            if (Directory.Exists(dir))
                Directory.Delete(dir);

            GenParameters.SaveToFile(d, fileName);
            Assert.IsTrue(Directory.Exists(dir), "Output directory is not created.");
            Assert.IsTrue(File.Exists(fileName));
            var stream = new FileStream(fileName, FileMode.Open);
            var d1 = new GenParameters(stream);
            
            VerifyDataCreation(d1);
        }

        /// <summary>
        /// Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
            
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
