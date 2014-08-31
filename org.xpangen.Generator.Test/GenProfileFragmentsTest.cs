// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;
using org.xpangen.Generator.Profile.Profile;
using org.xpangen.Generator.Profile.Scanner;

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
            ProcessSegment(GenData, "@", GenCardinality.Reference, "Property[Reference='']\r\n");
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded
        /// </summary>
        [TestCase(Description="Generator Segment Separator test")]
        public void GenSegmentSeparatorTest()
        {
            const string display = "111";
            const string expected = "One, Two, Three";
            VerifySegmentSeparator(display, expected, GenCardinality.AllDlm);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded when the first item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Segment Separator with first item empty test")]
        public void GenSegmentSeparatorFirstEmptyTest()
        {
            const string display = "011";
            const string expected = "Two, Three";
            VerifySegmentSeparator(display, expected, GenCardinality.AllDlm);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded when the middle item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Segment Separator with middle item empty test")]
        public void GenSegmentSeparatorMiddleEmptyTest()
        {
            const string display = "101";
            const string expected = "One, Three";
            VerifySegmentSeparator(display, expected, GenCardinality.AllDlm);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded when the final item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Segment Separator with final item empty test")]
        public void GenSegmentSeparatorFinalEmptyTest()
        {
            const string display = "110";
            const string expected = "One, Two";
            VerifySegmentSeparator(display, expected, GenCardinality.AllDlm);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded backwards
        /// </summary>
        [TestCase(Description = "Generator Backwards Segment Separator test")]
        public void GenSegmentBackSeparatorTest()
        {
            const string display = "111";
            const string expected = "Three, Two, One";
            VerifySegmentSeparator(display, expected, GenCardinality.BackDlm);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded backwards when the first item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Backwards Segment Separator with first item empty test")]
        public void GenSegmentBackSeparatorFirstEmptyTest()
        {
            const string display = "011";
            const string expected = "Three, Two";
            VerifySegmentSeparator(display, expected, GenCardinality.BackDlm);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded backwards when the middle item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Backwards Segment Separator with middle item empty test")]
        public void GenSegmentSeparatorBackMiddleEmptyTest()
        {
            const string display = "101";
            const string expected = "Three, One";
            VerifySegmentSeparator(display, expected, GenCardinality.BackDlm);
        }

        /// <summary>
        /// Tests that a segment with a separator is correctly expanded backwards when the final item in the list is empty
        /// </summary>
        [TestCase(Description = "Generator Backwards Segment Separator with final item empty test")]
        public void GenSegmentBackSeparatorFinalEmptyTest()
        {
            const string display = "110";
            const string expected = "Two, One";
            VerifySegmentSeparator(display, expected, GenCardinality.BackDlm);
        }

        /// <summary>
        /// Tests for the correct creation of a text fragment
        /// </summary>
        [TestCase(Description="Generator text fragment test")]
        public void GenTextFragmentTest()
        {
            var r = new GenProfileFragment(new GenProfileParams(GenData.GenDataDef));
            var g = new GenTextFragment(new GenTextFragmentParams(GenDataDef, r, r, "Text fragment"));
            VerifyFragment(GenData, g, "GenTextFragment", FragmentType.Text, "Text", "Text fragment", "Text fragment",
                true, -1);
        }

        /// <summary>
        /// Tests for the correct creation of a placeholder fragment
        /// </summary>
        [TestCase(Description="Generator placeholder test")]
        public void GenPlaceholderTest()
        {
            var r = new GenProfileFragment(new GenProfileParams(GenData.GenDataDef));
            var g =
                new GenPlaceholderFragment(new GenPlaceholderFragmentParams(GenDataDef, r, r,
                    GenDataDef.GetId("Property.Name")));
            VerifyFragment(GenData, g, "GenPlaceholderFragment", FragmentType.Placeholder, "Property.Name", "`Property.Name`",
                           "Property2", true, -1);
        }

        /// <summary>
        /// Tests for the correct creation of a block fragment
        /// </summary>
        [TestCase(Description="Generator Block test")]
        public void GenBlockTest()
        {
            var r = new GenProfileFragment(new GenProfileParams(GenData.GenDataDef));

            var g = new GenBlock(new GenFragmentParams(GenDataDef, r, r));
            r.Body.Add(g);
            var p =
                new GenPlaceholderFragment(new GenPlaceholderFragmentParams(GenDataDef, r, g,
                    GenDataDef.GetId("Property.Name")));
            var t = new GenTextFragment(new GenTextFragmentParams(GenData.GenDataDef, r, g, ","));
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
            var r = new GenProfileFragment(new GenProfileParams(GenData.GenDataDef));

            var g = new GenLookup(new GenLookupParams(f, r, r, "Class.Name=SubClass.Name"));
            var p = new GenPlaceholderFragment(new GenPlaceholderFragmentParams(f, r, g, f.GetId("Class.Name")));
            var t = new GenTextFragment(new GenTextFragmentParams(d.GenDataDef, r, g, ","));
            g.Body.Add(p);
            g.Body.Add(t);
            Assert.IsFalse(g.NoMatch);

            d.Last(ClassClassId); // Has no subclasses
            d.First(SubClassClassId);
            g.GenObject = d.Context[SubClassClassId].GenObject;
            VerifyFragment(d, g, "GenLookup", FragmentType.Lookup, "Class.Name=SubClass.Name",
                           "`%Class.Name=SubClass.Name:`Class.Name`,`]", "", false, -1);

            d.First(ClassClassId); // Has subclasses
            d.First(SubClassClassId);
            g.GenObject = d.Context[SubClassClassId].GenObject;
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
            var profile = "`[Class:`[SubClass:`%Class.Name=SubClass.Name:`;" + txt + "`]`]`]";
            var d = SetUpLookupData();
            var f = d.GenDataDef;
            var p = new GenCompactProfileParser(f, "", profile);

            var g = ((GenSegment) ((GenSegment) p.Body.Fragment[0]).Body.Fragment[0]).Body.Fragment[0];
            Assert.AreEqual(FragmentType.Lookup, g.FragmentType);
            Assert.IsFalse(g.IsTextFragment);
            Assert.AreEqual("~Class.Name=SubClass.Name", g.ProfileLabel());
            Assert.AreEqual("`%Class.Name=SubClass.Name:`;" + txt + "`]",
                            g.ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));

            d.Last(ClassClassId); // Has no subclasses
            Assert.AreEqual("Property", d.Context[ClassClassId].GenObject.Attributes[0]);
            d.First(SubClassClassId);
            g.ParentContainer.GenObject = d.Context[ClassClassId].GenObject;
            g.GenObject = d.Context[SubClassClassId].GenObject;
            Assert.AreEqual(txt, GenFragmentExpander.Expand(d, g.GenObject, g.Fragment));
            var str = GenerateFragment(d, g);
            Assert.AreEqual(txt, str);

            d.First(ClassClassId); // Has subclasses
            Assert.AreEqual("Class", d.Context[ClassClassId].GenObject.Attributes[0]);
            d.First(SubClassClassId);
            Assert.AreEqual("SubClass", d.Context[SubClassClassId].GenObject.Attributes[0]);
            g.GenObject = d.Context[SubClassClassId].GenObject;
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
            var r = new GenProfileFragment(new GenProfileParams(GenData.GenDataDef));

            var b = new GenBlock(new GenFragmentParams(f, r, r));
            var p0 = new GenPlaceholderFragment(new GenPlaceholderFragmentParams(f, r, b, f.GetId("Parent.Name")));
            var t0 = new GenTextFragment(new GenTextFragmentParams(f, r, b, ","));
            b.Body.Add(p0);
            b.Body.Add(t0);

            var g = new GenLookup(new GenLookupParams(f, r, b, "Lookup.Name=Child.Lookup"));
            var p1 = new GenPlaceholderFragment(new GenPlaceholderFragmentParams(f, r, g, f.GetId("Lookup.Name")));
            var t1 = new GenTextFragment(new GenTextFragmentParams(f, r, g, ","));
            Assert.IsFalse(g.NoMatch);

            b.Body.Add(g);
            g.Body.Add(p1);
            g.Body.Add(t1);

            var parentId = f.Classes.IndexOf("Parent");
            var childId = f.Classes.IndexOf("Child");
            d.First(parentId);
            d.First(childId); // Valid lookup
            b.GenObject = d.Context[childId].GenObject;
            VerifyFragment(d, b, "GenBlock", FragmentType.Block, "Block",
                           "`{`Parent.Name`,`%Lookup.Name=Child.Lookup:`Lookup.Name`,`]`]", "Parent,Valid,", false, -1);

            d.First(parentId);
            d.Last(childId); // Invalid lookup
            b.GenObject = d.Context[childId].GenObject;
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
            ExecuteFunction(GenData, "QuoteString", "This\tTabbed string", "", @"'This\tTabbed string'");
            ExecuteFunction(GenData, "QuoteString", "This\r\nnew line string", "", @"'This\r\nnew line string'");
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
        /// Tests that the function to convert from camel case with lowercase words is created correctly
        /// </summary>
        [TestCase(Description = "Generator Function test - Un-Identifier Lower case")]
        public void GenFunctionUnIdentifierLc()
        {
            ExecuteFunction(GenData, "UnIdentifierLc", "I-am_anIdentifier IAm", "", "I am an identifier i am");
        }

        /// <summary>
        /// Tests that the function to convert from camel case is created correctly
        /// </summary>
        [TestCase(Description = "Generator Function test - Decapitalize")]
        public void GenFunctionDecapitalize()
        {
            ExecuteFunction(GenData, "Decapitalize", "DeCapitalize", "", "deCapitalize");
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
