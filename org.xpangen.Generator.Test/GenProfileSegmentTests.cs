using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Test
{
    public class GenProfileSegmentTests : GenProfileFragmentsTestBase
    {
        private GenDataBase GenData { get; set; }
        /// <summary>
        /// Tests for the correct creation of a segment
        /// </summary>
        [TestCase("", GenCardinality.All, "Property,Property2,"),
        TestCase(">", GenCardinality.All, "Property,Property2,"),
        TestCase("'", GenCardinality.First, "Property,"),
        TestCase("+", GenCardinality.Tail, "Property2,"),
        TestCase(".", GenCardinality.Last, "Property2,"),
        TestCase("-", GenCardinality.Trunk, "Property,"),
        TestCase("<", GenCardinality.Back, "Property2,Property,"),
        TestCase("/", GenCardinality.AllDlm, "Property,Property2"),
        TestCase("\\", GenCardinality.BackDlm, "Property2,Property"),
        TestCase("@", GenCardinality.Reference, "Property[Reference='']\r\n")]
        public void GenSegmentTest(string cardinalityText, GenCardinality genCardinality, string expected)
        {
            var cardinality = GenCardinality.All;
            var dictionary = ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary;
            if (cardinalityText != "")
            {
                for (var i = 0; i < dictionary.GenCardinalityText.Length; i++)
                {
                    if (cardinalityText != dictionary.GenCardinalityText[i])
                        continue;
                    cardinality = (GenCardinality) i;
                    break;
                }
            }
            var profile = "`[Property" + dictionary.GenCardinalityText[(int) cardinality] + ":`Property.Name`" +
                          ((cardinality == GenCardinality.AllDlm || cardinality == GenCardinality.BackDlm)
                              ? "`;"
                              : "") + ",`]";
            var p = new GenCompactProfileParser(GenData.GenDataDef, "", "`[Class':" + profile + "`]");
            var g = (GenSegment) ((GenSegment) p.Body.Fragment[0]).Body.Fragment[0];
            g.GenObject = GetFirstObject(GenData);
            Assert.AreEqual(genCardinality.ToString(), ((Segment) g.Fragment).Cardinality);
            Assert.AreEqual("Property", g.Definition.Name);
            //GenData.First(1);
            VerifyFragment(GenData, g, "GenSegment", FragmentType.Segment, "Property", profile, expected, false, null,
                p.Profile.GenDataBase.GenDataDef);
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
        /// Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
            GenData = SetUpData();
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