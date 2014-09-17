using System;
using NUnit.Framework;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Test
{
    [TestFixture]
    public class GenProfileFunctionTests : GenProfileFragmentsTestBase
    {
        private GenDataDef GenDataDef { get; set; }

        private GenData GenData { get; set; }

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