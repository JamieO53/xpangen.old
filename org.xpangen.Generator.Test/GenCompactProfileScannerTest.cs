// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Profile.Scanner;
using org.xpangen.Generator.Scanner;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the Generator Profile scanner functionality
    /// </summary>
	[TestFixture]
    public class GenCompactProfileScannerTest
    {
        /// <summary>
        /// Tests that the scanner correctly handels the end of file condition
        /// </summary>
        [TestCase(Description="Scanner EOF test")]
        public void EofTest()
        {
            var scan = new CompactProfileScanner("");
            Assert.IsTrue(scan.Eof);
            scan.SkipChar();
            Assert.AreEqual(ScanReader.EofChar, scan.Current, "Scan past EOF");
        }

        /// <summary>
        /// Tests that quoted strings are scanned correctly
        /// </summary>
        [TestCase(Description="Scanner Quoted String test")]
        public void QuotedStringTest()
        {
            const string txt = "/quoted //string/unquoted string"; // / is the quote character;
            var scan = new CompactProfileScanner(txt);
            Assert.AreEqual("quoted /string", scan.ScanQuotedString());
            Assert.IsTrue(scan.CheckChar('u'), "Next character to scan");
        }

        /// <summary>
        /// Tests that the scanner correctly identifies the token type for the compact profile fragment syntax dictionary
        /// </summary>
        [TestCase(Description="Scanner Compact Token Type test")]
        public void CompactTokenTypeTest()
        {
            const string txt = "[{%;?@]`x~";
            var scan = new CompactProfileScanner(txt);
            Assert.AreEqual(TokenType.Segment, scan.ScanTokenType());
            scan.SkipChar();
            Assert.AreEqual(TokenType.Block, scan.ScanTokenType());
            scan.SkipChar();
            Assert.AreEqual(TokenType.Lookup, scan.ScanTokenType());
            scan.SkipChar();
            Assert.AreEqual(TokenType.Secondary, scan.ScanTokenType());
            scan.SkipChar();
            Assert.AreEqual(TokenType.Condition, scan.ScanTokenType());
            scan.SkipChar();
            Assert.AreEqual(TokenType.Function, scan.ScanTokenType());
            scan.SkipChar();
            Assert.AreEqual(TokenType.Close, scan.ScanTokenType());
            scan.SkipChar();
            Assert.AreEqual(TokenType.Delimiter, scan.ScanTokenType());
            // Automatically skipped
            Assert.AreEqual(TokenType.Name, scan.ScanTokenType());
            scan.SkipChar();
            Assert.IsFalse(scan.Eof);
            Assert.AreEqual(TokenType.Unknown, scan.ScanTokenType());
            scan.SkipChar();
            Assert.IsTrue(scan.Eof);
            Assert.AreEqual(TokenType.Unknown, scan.ScanTokenType());
        }

        /// <summary>
        /// Tests that text is correctly scanned
        /// </summary>
        [TestCase(Description="Scanner Text test")]
        public void TextTest()
        {
            const string txt = "text ``string`post-text string";
            var scan = new CompactProfileScanner(txt);
            Assert.AreEqual("text `string", scan.ScanText());
            Assert.AreEqual("p", scan.Current.ToString(), "Should now be beyond the delimeter");
        }

        /// <summary>
        /// Tests that a class segment is correctly scanned
        /// </summary>
        [TestCase(Description="Scanner segment class test")]
        public void SegmentClassTest()
        {
            const string txt = "`[Class>:Segment text`]";
            var scan = new CompactProfileScanner(txt);
            Assert.AreEqual(TokenType.Delimiter, scan.ScanTokenType(), "Verify and skip over delimiter");
            Assert.AreEqual(TokenType.Segment, scan.ScanTokenType());
            Assert.AreEqual("Class>", scan.ScanSegmentClass(), "Should be the segement class");
            Assert.AreEqual("S", scan.Current.ToString(), "Should now be on the body of the segment");
        }

        /// <summary>
        /// Tests that a condition is correctly scanned
        /// </summary>
        [TestCase(Description="Scanner Condition test")]
        public void ConditionTest()
        {
            const string txt = "`?Class.Name='Class':Condition text`]";
            var scan = new CompactProfileScanner(txt);
            Assert.AreEqual(TokenType.Delimiter, scan.ScanTokenType(), "Verify and skip over delimiter");
            Assert.AreEqual(TokenType.Condition, scan.ScanTokenType());
            Assert.AreEqual("Class.Name='Class'", scan.ScanCondition(), "Should be the condition");
            Assert.AreEqual("C", scan.Current.ToString(), "Should now be in the body of the condition");
        }

        /// <summary>
        /// Tests that a Lookup is correctly scanned
        /// </summary>
        [TestCase(Description="Scanner Lookup test")]
        public void LookupTest()
        {
            const string txt = "`%Class.Name=SubClass.Name:Lookup text`]";
            var scan = new CompactProfileScanner(txt);
            Assert.AreEqual(TokenType.Delimiter, scan.ScanTokenType(), "Verify and skip over delimiter");
            Assert.AreEqual(TokenType.Lookup, scan.ScanTokenType());
            Assert.AreEqual("Class.Name=SubClass.Name", scan.ScanLookup(), "Should be the lookup condition");
            Assert.AreEqual("L", scan.Current.ToString(), "Should now be in the body of the lookup");
        }

        /// <summary>
        /// Tests that the No Match Lookup is correctly scanned
        /// </summary>
        [TestCase(Description="Scanner No Match Lookup test")]
        public void NoMatchTest()
        {
            const string txt = "`%Class.Name=SubClass.Name:`;No Match text`]";
            var scan = new CompactProfileScanner(txt);
            Assert.AreEqual(TokenType.Delimiter, scan.ScanTokenType(), "Verify and skip over delimiter");
            Assert.AreEqual(TokenType.Lookup, scan.ScanTokenType());
            Assert.AreEqual("Class.Name=SubClass.Name", scan.ScanLookup(), "Should be the lookup condition");
            Assert.AreEqual(TokenType.Delimiter, scan.ScanTokenType(), "Verify and skip over delimiter");
            Assert.AreEqual(TokenType.Secondary, scan.ScanTokenType(), "Should be on the secondary token");
            scan.SkipChar();
            Assert.AreEqual("N", scan.Current.ToString(), "Should now be in the body of the lookup");
        }

        /// <summary>
        /// Tests that qualified names are correctly scanned
        /// </summary>
        [TestCase(Description="Scanner Full Name test")]
        public void FullNameTest()
        {
            const string txt = "`Class.Name`";
            var scan = new CompactProfileScanner(txt);
            Assert.AreEqual(TokenType.Delimiter, scan.ScanTokenType(), "Verify and skip over delimiter");
            Assert.AreEqual(TokenType.Name, scan.ScanTokenType());
            Assert.AreEqual("Class.Name", scan.ScanName(), "Should be the fully qualified name");
            Assert.AreEqual(TokenType.Delimiter, scan.ScanTokenType());
        }

        /// <summary>
        /// Tests that unqualified names are scanned correctly
        /// </summary>
        [TestCase(Description="Scanner Part Name test")]
        public void PartNameTest()
        {
            const string txt = "`Name`";
            var scan = new CompactProfileScanner(txt);
            Assert.AreEqual(TokenType.Delimiter, scan.ScanTokenType(), "Verify and skip over delimiter");
            Assert.AreEqual(TokenType.Name, scan.ScanTokenType());
            Assert.AreEqual("Name", scan.ScanName(), "Should be the unqualified name");
            Assert.AreEqual(TokenType.Delimiter, scan.ScanTokenType());
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
