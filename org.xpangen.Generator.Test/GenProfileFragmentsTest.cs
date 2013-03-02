// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the generator profile fragment functionality
    /// </summary>
	[TestFixture]
    public class GenProfileFragmentsTest : GenProfileFragmentsTestBase
    {
        private GenDataDef GenDataDef { get; set; }

        private GenData GenData { get; set; }

        /// <summary>
        /// Tests for the correct creation of a null fragment
        /// </summary>
        [TestCase(Description="Generator Fragment test")]
        public void GenFragmentTest()
        {
            var g = new GenFragmentTest(GenDataDef, null);
            VerifyFragment(GenData, g, "GenFragmentTest", FragmentType.Null, "Label", "Text", "Generate", true, -1);
        }

        /// <summary>
        /// Tests for the correct creation of a segment body
        /// </summary>
        [TestCase(Description="Generator segment body test")]
        public void GenSegmentBodyTest()
        {
            var g = new GenSegBody(GenDataDef, null);
            var t = new GenTextFragment(GenDataDef, g.ParentSegement) { Text = "Text piece" };
            g.Add(t);
            VerifyFragment(GenData, g, "GenSegBody", FragmentType.Body, "Body", "Text piece", "Text piece", false, -1);
            Assert.AreEqual(0, g.IndexOf(t));
            Assert.AreEqual(t, g.Fragment[0]);
            Assert.AreEqual(1, g.Count);
        }

        /// <summary>
        /// Tests for the correct creation of a segment
        /// </summary>
        [TestCase(Description = "Generator segment test")]
        public void GenSegmentTest()
        {
            ProcessSegment(GenData, "", GenCardinality.All, "Property,Property2,");
            ProcessSegment(GenData, ">", GenCardinality.All, "Property,Property2,");
            ProcessSegment(GenData, "'", GenCardinality.First, "Property,");
            ProcessSegment(GenData, "+", GenCardinality.Tail, "Property2,");
            ProcessSegment(GenData, ".", GenCardinality.Last, "Property2,");
            ProcessSegment(GenData, "-", GenCardinality.Trunk, "Property,");
            ProcessSegment(GenData, "<", GenCardinality.Back, "Property2,Property,");
            ProcessSegment(GenData, "/", GenCardinality.AllDlm, "Property,Property2");
            ProcessSegment(GenData, "\\", GenCardinality.BackDlm, "Property2,Property");
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded
        /// </summary>
        [TestCase(Description="Generator Segment Separator test")]
        public void GenSegmentSeparatorTest()
        {
            const string display = "111";
            const string expected = "One, Two, Three";
            VerifySegmentSeparator(display, expected, GenCardinality.AllDlm, null);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded when the first item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Segment Separator with first item empty test")]
        public void GenSegmentSeparatorFirstEmptyTest()
        {
            const string display = "011";
            const string expected = "Two, Three";
            VerifySegmentSeparator(display, expected, GenCardinality.AllDlm, null);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded when the middle item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Segment Separator with middle item empty test")]
        public void GenSegmentSeparatorMiddleEmptyTest()
        {
            const string display = "101";
            const string expected = "One, Three";
            VerifySegmentSeparator(display, expected, GenCardinality.AllDlm, null);
        }

        /// <summary>
        /// Tests for the correct creation of a null fragment
        /// </summary>
        [TestCase(Description="Generator null fragment test")]
        public void GenNullFragmentTest()
        {
            var g = new GenNullFragment(GenDataDef, null);
            VerifyFragment(GenData, g, "GenNullFragment", FragmentType.Null, "", "", "", true, -1);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded when the final item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Segment Separator with final item empty test")]
        public void GenSegmentSeparatorFinalEmptyTest()
        {
            const string display = "110";
            const string expected = "One, Two";
            VerifySegmentSeparator(display, expected, GenCardinality.AllDlm, null);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded backwards
        /// </summary>
        [TestCase(Description = "Generator Backwards Segment Separator test")]
        public void GenSegmentBackSeparatorTest()
        {
            const string display = "111";
            const string expected = "Three, Two, One";
            VerifySegmentSeparator(display, expected, GenCardinality.BackDlm, null);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded backwards when the first item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Backwards Segment Separator with first item empty test")]
        public void GenSegmentBackSeparatorFirstEmptyTest()
        {
            const string display = "011";
            const string expected = "Three, Two";
            VerifySegmentSeparator(display, expected, GenCardinality.BackDlm, null);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded backwards when the middle item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Backwards Segment Separator with middle item empty test")]
        public void GenSegmentSeparatorBackMiddleEmptyTest()
        {
            const string display = "101";
            const string expected = "Three, One";
            VerifySegmentSeparator(display, expected, GenCardinality.BackDlm, null);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded backwards when the final item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Backwards Segment Separator with final item empty test")]
        public void GenSegmentBackSeparatorFinalEmptyTest()
        {
            const string display = "110";
            const string expected = "Two, One";
            VerifySegmentSeparator(display, expected, GenCardinality.BackDlm, null);
        }

        /// <summary>
        /// Tests for the correct creation of a text fragment
        /// </summary>
        [TestCase(Description="Generator text fragment test")]
        public void GenTextFragmentTest()
        {
            var g = new GenTextFragment(GenDataDef, null) {Text = "Text fragment"};
            VerifyFragment(GenData, g, "GenTextFragment", FragmentType.Text, "Text", "Text fragment", "Text fragment", true, -1);
        }

        /// <summary>
        /// Tests for the correct creation of a placeholder fragment
        /// </summary>
        [TestCase(Description="Generator placeholder test")]
        public void GenPlaceholderTest()
        {
            var g = new GenPlaceholderFragment(GenDataDef, null) {Id = GenDataDef.GetId("Property.Name")};
            VerifyFragment(GenData, g, "GenPlaceholderFragment", FragmentType.Placeholder, "Property.Name", "`Property.Name`",
                           "Property2", true, -1);
        }

        /// <summary>
        /// Tests for the correct creation of a block fragment
        /// </summary>
        [TestCase(Description="Generator Block test")]
        public void GenBlockTest()
        {
            var p = new GenPlaceholderFragment(GenDataDef, null) {Id = GenDataDef.GetId("Property.Name")};
            var t = new GenTextFragment(GenData.GenDataDef, null) {Text = ","};

            var g = new GenBlock(GenDataDef, null);
            g.Body.Add(p);
            g.Body.Add(t);
            VerifyFragment(GenData, g, "GenBlock", FragmentType.Block, "Block", "`{`Property.Name`,`]", "Property2,", false, -1);
        }

        /// <summary>
        /// Tests for the correct creation of a lookup fragment matching a name
        /// </summary>
        [TestCase(Description="Generator Lookup Match test")]
        public void GenLookupMatchTest()
        {
            var d = SetUpLookupData();
            var f = d.GenDataDef;
            var p = new GenPlaceholderFragment(f, null) { Id = f.GetId("Class.Name") };
            var t = new GenTextFragment(d.GenDataDef, null) { Text = "," };

            var g = new GenLookup(f, "Class.Name=SubClass.Name", null);
            g.Body.Add(p);
            g.Body.Add(t);
            Assert.IsFalse(g.NoMatch);

            d.Last(ClassClassId); // Has no subclasses
            d.First(SubClassClassId);

            VerifyFragment(d, g, "GenLookup", FragmentType.Lookup, "Class.Name=SubClass.Name",
                           "`%Class.Name=SubClass.Name:`Class.Name`,`]", "", false, -1);

            d.First(ClassClassId); // Has subclasses
            d.First(SubClassClassId);

            VerifyFragment(d, g, "GenLookup", FragmentType.Lookup, "Class.Name=SubClass.Name",
                           "`%Class.Name=SubClass.Name:`Class.Name`,`]", "SubClass,", false, -1);
        }

        /// <summary>
        /// Tests for the correct creation of a lookup fragment where the given name does not match
        /// </summary>
        [TestCase(Description="Generator Lookup No Match test")]
        public void GenLookupNoMatchTest()
        {
            const string txt = "SubClass does not exist";
            var d = SetUpLookupData();
            var f = d.GenDataDef;
            var t = new GenTextFragment(f, null) { Text = txt };

            var g = new GenLookup(f, "Class.Name=SubClass.Name", null) {NoMatch = true};
            g.Body.Add(t);
            Assert.AreEqual(FragmentType.Lookup, g.FragmentType);
            Assert.IsFalse(g.IsTextFragment);
            Assert.AreEqual("~Class.Name=SubClass.Name", g.ProfileLabel());
            Assert.AreEqual("`&Class.Name=SubClass.Name:" + txt + "`]",
                            g.ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));

            d.Last(ClassClassId); // Has no subclasses
            Assert.AreEqual("Property", d.Context[ClassClassId].Context.Attributes[0]);
            d.First(SubClassClassId);
            Assert.AreEqual(txt, g.Expand(d));
            var str = GenerateFragment(d, g);
            Assert.AreEqual(txt, str);

            d.First(ClassClassId); // Has subclasses
            Assert.AreEqual("Class", d.Context[ClassClassId].Context.Attributes[0]);
            d.First(SubClassClassId);
            Assert.AreEqual("SubClass", d.Context[SubClassClassId].Context.Attributes[0]);
            str = GenerateFragment(d, g);
            Assert.AreEqual("", str);
        }

        /// <summary>
        /// Checks for the correct creation of a lookup in context fragment
        /// </summary>
        [TestCase(Description="Generator Lookup Context Test")]
        public void GenLookupContextTest()
        {
            var d = SetUpLookupContextData();
            var f = d.GenDataDef;
            var p0 = new GenPlaceholderFragment(f, null) {Id = f.GetId("Parent.Name")};
            var p1 = new GenPlaceholderFragment(f, null) {Id = f.GetId("Lookup.Name")};
            var t = new GenTextFragment(f, null) {Text = ","};

            var b = new GenBlock(f, null);
            b.Body.Add(p0);
            b.Body.Add(t);

            var g = new GenLookup(f, "Lookup.Name=Child.Lookup", null);
            g.Body.Add(p1);
            g.Body.Add(t);
            Assert.IsFalse(g.NoMatch);

            b.Body.Add(g);

            var parentId = f.Classes.IndexOf("Parent");
            var childId = f.Classes.IndexOf("Child");
            d.First(parentId);
            d.First(childId); // Valid lookup

            VerifyFragment(d, b, "GenBlock", FragmentType.Block, "Block",
                           "`{`Parent.Name`,`%Lookup.Name=Child.Lookup:`Lookup.Name`,`]`]", "Parent,Valid,", false, -1);

            d.First(parentId);
            d.Last(childId); // Invalid lookup
            VerifyFragment(d, b, "GenBlock", FragmentType.Block, "Block",
                           "`{`Parent.Name`,`%Lookup.Name=Child.Lookup:`Lookup.Name`,`]`]", "Parent,", false, -1);
        }

        /// <summary>
        /// Tests for the correct creation of an attribute value existing condition fragment
        /// </summary>
        [TestCase(Description="Generator Existence Condition test")]
        public void GenExistenceConditionTest()
        {
            var d = SetUpComparisonData();

            // Test for Existence 1
            TestCondition(d, "Property.Name Exists", "Property.Name", "Property.Name", true);

            // Test for Existence 2
            TestCondition(d, "Property.Name", "", "", true);
            
            // Test for Existence 3
            TestCondition(d, "Property.NameBlank", "", "", false);

            // Test for non-existence 1
            TestCondition(d, "Property.Name~", "", "", false);

            // Test for non-existence 2
            TestCondition(d, "Property.NameBlank~", "", "", true);
        }

        /// <summary>
        /// Tests for the correct creation of a condition matching a literal value
        /// </summary>
        [TestCase(Description = "Generator Literal Condition test")]
        public void GenLiteralConditionTest()
        {
            var d = SetUpComparisonData();

            // Test for equality
            TestComparison(d, "=", false, true, false);

            // Test for inequality
            TestComparison(d, "<>", true, false, true);

            // Test for less than
            TestComparison(d, "<", false, false, true);

            // Test for greater than
            TestComparison(d, ">", true, false, false);

            // Test for less or equal to
            TestComparison(d, "<=", false, true, true);

            // Test for greater or equal to 1
            TestComparison(d, ">=", true, true, false);
        }

        /// <summary>
        /// Tests for the correct creation of a condition testing for a numeric literal value
        /// </summary>
        [TestCase(Description="Generator Numeric Literal Condition Test")]
        public void GenNumericLiteralConditionTest()
        {
            var d = SetUpNumericComparisonData();

            // Test for equality
            TestNumericComparison(d, "=", false, true, false);

            // Test for inequality
            TestNumericComparison(d, "<>", true, false, true);

            // Test for less than
            TestNumericComparison(d, "<", false, false, true);

            // Test for greater than
            TestNumericComparison(d, ">", true, false, false);

            // Test for less or equal to
            TestNumericComparison(d, "<=", false, true, true);

            // Test for greater or equal to 1
            TestNumericComparison(d, ">=", true, true, false);
        }

        /// <summary>
        /// Tests for the correct creation of a condition testing for the equality of two attribute values
        /// </summary>
        [TestCase(Description="Generator Identifier Condition test")]
        public void GenIdentifierConditionTest()
        {
            var d = SetUpComparisonData();

            // Test for equality
            TestIdentifierComparison(d, "=", false, true, false);

            // Test for inequality
            TestIdentifierComparison(d, "<>", true, false, true);

            // Test for less than
            TestIdentifierComparison(d, "<", false, false, true);

            // Test for greater than
            TestIdentifierComparison(d, ">", true, false, false);

            // Test for less or equal to
            TestIdentifierComparison(d, "<=", false, true, true);

            // Test for greater or equal to 1
            TestIdentifierComparison(d, ">=", true, true, false);
        }

        /// <summary>
        /// Tests the Set, Add, Sub and Get functions for numeric data
        /// </summary>
        [TestCase(Description="Generator Function Test Set Add Sub Get")]
        public void GenFunctionTestSetAddSubGet()
        {
            ExecuteFunction(GenData, "Set", "Var", "5", "");
            CheckFunctionVarValue(GenData, "5");
            ExecuteFunction(GenData, "Add", "Var", "", ""); // 1 gets added twice
            CheckFunctionVarValue(GenData, "7");
            ExecuteFunction(GenData, "Add", "Var", "10", ""); // 10 gets added twice
            CheckFunctionVarValue(GenData, "27");
            ExecuteFunction(GenData, "Sub", "Var", "10", ""); // 10 gets subtracted twice
            CheckFunctionVarValue(GenData, "7");
            ExecuteFunction(GenData, "Sub", "Var", "", ""); // 1 gets subtracted twice
            CheckFunctionVarValue(GenData, "5");
        }

        /// <summary>
        /// Tests the Map Set and Get functions are created correctly
        /// </summary>
        [TestCase(Description = "Generator Function test for Map Set and Get")]
        public void GenFunctionTestMapSetGet()
        {
            ExecuteFunction(GenData, "MapSet", "Name 1", "Value 1", "");
            ExecuteFunction(GenData, "MapSet", "Name 2", "Value 2", "");
            ExecuteFunction(GenData, "MapGet", "Name 1", "", "Value 1");
            ExecuteFunction(GenData, "MapGet", "Name 2", "", "Value 2");
            ExecuteFunction(GenData, "MapClear", "", "", "");
            ExecuteFunction(GenData, "MapGet", "Name 1", "", "<<Mapping not set: Name 1>>");
        }

        /// <summary>
        /// Tests if the StringOrName function is created correctly
        /// </summary>
        [TestCase(Description = "Generator Function Test for String or Name")]
        public void GenFunctionTestStringOrName()
        {
            ExecuteFunction(GenData, "StringOrName", "NameValue", "", "NameValue");
            ExecuteFunction(GenData, "StringOrName", "Non Name Value", "", "'Non Name Value'");
        }

        /// <summary>
        /// Tests for the correct creation of the quoted string function
        /// </summary>
        [TestCase(Description = "Generator Quote String function test")]
        public void GenFunctionQuoteString()
        {
            ExecuteFunction(GenData, "QuoteString", "This string has no quotes", "", "'This string has no quotes'");
            ExecuteFunction(GenData, "QuoteString", "This string's quotes", "", "'This string''s quotes'");
        }

        /// <summary>
        /// Tests that the Cut String function is created correctly
        /// </summary>
        [TestCase(Description = "Generator Function test - Cut String")]
        public void GenFunctionCutString()
        {
            ExecuteFunction(GenData, "CutString", "This string is cut", " is", "This string cut");
            ExecuteFunction(GenData, "CutString", "This string is not cut", "isn't", "This string is not cut");
        }

        /// <summary>
        /// Tests that the function to convert from camel case is created correctly
        /// </summary>
        [TestCase(Description = "Generator Function test - Un-Identifier")]
        public void GenFunctionUnIdentifier()
        {
            ExecuteFunction(GenData, "UnIdentifier", "I-am_anIdentifier IAm", "", "I am an Identifier I Am");
        }

        /// <summary>
        /// Tests that the current date function is created correctly
        /// </summary>
        [TestCase(Description = "Generator Function test - Date")]
        public void GenFunctionDate()
        {
            ExecuteFunction(GenData, "Date", "", "", DateTime.Today.ToString("D")); // Long date format
        }

        /// <summary>
        /// Tests that the Current Time function is created correctly
        /// </summary>
        [TestCase(Description="Generator Function Tests for Time")]
        public void GenFunctionTime()
        {
            ExecuteFunction(GenData, "Time", "", "", DateTime.Now.ToShortTimeString()); // Short time format
        }

        /// <summary>
        /// Tests that the function to close the generated file and create a new one is created correctly
        /// </summary>
        [TestCase(Description = "Generator Function test - File")]
        public void GenFunctionFile()
        {
            ExecuteFunction(GenData, "File", "FileName.txt", "", "\r\n //File: FileName.txt");
            // Note: this test is valid for String Streams, not for File Streams.
            // For File Streams, the existing file is closed and a new file is
            // opened with the given file name.
        }

        /// <summary>
        /// Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
            GenData = SetUpData();
            GenDataDef = GenData.GenDataDef;
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
