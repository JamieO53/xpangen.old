using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Scanner;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the scanner functionality
    /// </summary>
	[TestFixture]
    public class ScannerTest
    {
        /// <summary>
        /// Tests to ensure that empty input is handled correctly
        /// </summary>
        [TestCase(Description = "Empty data test")]
        public void EmptyTest()
        {
            const string text = "";
            using (var scan = new ScanReader(text))
            {
                var s = scan.ScanUntilChar('#');
                Assert.AreEqual("", s, "Empty string expected");
                CheckScannerEofConditions(scan);
            }
        }

        /// <summary>
        /// Tests scanning text from a file
        /// </summary>
        [TestCase(Description = "Stream test")]
        public void StreamTest()
        {
            const string input = "AbCd # 12345";
            const string output1 = "AbCd";
            const string output2 = "12345";
            const string fileName = "StreamTest.txt";

            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, input);
            var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            using (var scan = new ScanReader(stream))
            {
                Assert.AreEqual("Unicode (UTF-8)", scan.Encoding.EncodingName);
                Assert.AreEqual(output1, scan.ScanWhile(ScanReader.Alpha));
                CheckScannerNotEofConditions(scan);
                scan.ScanUntil(ScanReader.Numeric);
                Assert.AreEqual(output2, scan.ScanWhile(ScanReader.Numeric));
            }
        }

        /// <summary>
        /// Tests scanning while characters belong to a class
        /// </summary>
        [TestCase(Description="While test")]
        public void WhileTest()
        {
            const string input = "AbCd # 12345";
            const string output = "AbCd";

            using (var scan = new ScanReader(input))
            {
                Assert.AreEqual(output, scan.ScanWhile(ScanReader.Alpha));
                CheckScannerNotEofConditions(scan);
            }
        }

        /// <summary>
        /// Test scanning until characters belong to a class
        /// </summary>
        [TestCase(Description="Until test")]
        public void UntilTest()
        {
            const string input = "AbCd # 12345";
            const string output = "AbCd ";

            using (var scan = new ScanReader(input))
            {
                Assert.AreEqual(output, scan.ScanUntilChar('#'));
                CheckScannerNotEofConditions(scan);
            }
        }

        /// <summary>
        /// Tests for scanning to the end of the text
        /// </summary>
        [TestCase(Description="End Of File scan to end test")]
        public void EofScanToEndTest()
        {
            const string input = "12345";

            using (var scan = new ScanReader(input))
            {
                var s = scan.ScanWhile(ScanReader.AlphaNumeric);
                Assert.AreEqual("12345", s, "numeric value expected");
                CheckScannerEofConditions(scan);
            }
        }

        /// <summary>
        /// Tests to ensure that EOF logic for empty input is handled correctly
        /// </summary>
        [TestCase(Description = "End of File on empty data test")]
        public void EofEmptyTest()
        {
            const string text = "";
            using (var scan = new ScanReader(text))
            {
                CheckScannerEofConditions(scan);
                var s = scan.ScanUntilChar(ScanReader.EofChar);
                Assert.AreEqual("", s, "Empty string expected");
                CheckScannerEofConditions(scan);
                scan.SkipChar();
                CheckScannerEofConditions(scan);
            }
        }

        /// <summary>
        /// Tests scanning while characters belong to a class
        /// </summary>
        [TestCase(Description = "End of File character scan test")]
        public void EofCharTest()
        {
            const string input = "AbCd # 12345";

            using (var scan = new ScanReader(input))
            {
                Assert.AreEqual(input, scan.ScanUntilChar(ScanReader.EofChar));
                CheckScannerEofConditions(scan);
            }
        }

        /// <summary>
        /// Tests scanning while characters belong to a class
        /// </summary>
        [TestCase(Description = "Skip character scan test")]
        public void SkipCharTest()
        {
            const string input = "AbCd # 12345";

            using (var scan = new ScanReader(input))
            {
                scan.ScanUntilChar('#');
                Assert.IsTrue(scan.CheckChar('#'), "Scan to start point");
                scan.SkipChar();
                Assert.IsTrue(scan.CheckChar(' '), "Space after #");
                CheckScannerNotEofConditions(scan);
            }
        }

        /// <summary>
        /// Tests scanning with macro expansion
        /// </summary>
        [TestCase(Description="Macro test")]
        public void MacroTest()
        {
            const string input = "Prefix #Macro1 Suffix";
            const string output = "Prefix [1[2]] Suffix";
            var macros = new Dictionary<string, string> {{"Macro1", "[1#Macro2]"}, {"Macro2", "[2]"}};

            using (var scan = new ScanReader(input))
            {
                var s = scan.ScanUntilChar('#');
                while (scan.CheckChar('#'))
                {
                    scan.SkipChar();
                    var t = scan.ScanWhile(ScanReader.AlphaNumeric);
                    scan.Rescan(macros[t]);
                    s += scan.ScanUntilChar('#');
                }
                s += scan.ScanUntilChar(ScanReader.EofChar);
                Assert.AreEqual(output, s);
            }
        }

        /// <summary>
        /// Tests the scanning of a keyword
        /// </summary>
        [TestCase(Description="Scan keyword test")]
        public void ScanKeyword()
        {
            const string input = "[* *]";

            using (var scan = new ScanReader(input))
            {
                Assert.AreEqual("[*", scan.ScanKeyword("[*"), "Scan Keyword");
                Assert.IsTrue(scan.CheckChar(' '), "Space after keyword");
            }
        }

        /// <summary>
        /// Test scanning until a specified keyword is encountered
        /// </summary>
        [TestCase(Description="Scan for keyword")]
        public void ScanForKeyword()
        {
            const string input = "**]";

            using (var scan = new ScanReader(input))
            {
                Assert.AreEqual("*", scan.ScanUntilKeyword("*]"), "Scan Until Keyword");
                Assert.IsTrue(scan.CheckChar('*'), "First character of keyword");
            }
        }

        /// <summary>
        /// Tests the scanning of a simple block, with opening and closing keywords
        /// </summary>
        [TestCase(Description="Simple Block test")]
        public void SimpleBlockTest()
        {
            const string input = "[**]";

            using (var scan = new ScanReader(input))
            {
                Assert.AreEqual(input, scan.ScanSimpleBlock("[*", "*]"), "Scan Keyword");
            }
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

        /// <summary>
        /// Verify that the scanner state is correct when the end of the data has
        /// been reached
        /// </summary>
        /// <param name="scan"></param>
        private static void CheckScannerEofConditions(ScanReader scan)
        {
            Assert.IsTrue(scan.Eof);
            Assert.IsTrue(scan.CheckChar(ScanReader.EofChar), "Ctrl-Z expected at EOF");
        }

        /// <summary>
        /// Verify that the scanner state is correct when the end of the data has
        /// not been reached
        /// </summary>
        /// <param name="scan"></param>
        private static void CheckScannerNotEofConditions(ScanReader scan)
        {
            Assert.IsFalse(scan.Eof);
            Assert.IsFalse(scan.CheckChar(ScanReader.EofChar), "Ctrl-Z expected at EOF");
        }

    }
}
