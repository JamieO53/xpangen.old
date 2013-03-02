// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the generator parameter file functionality
    /// </summary>
	[TestFixture]
    public class GenParameterTests : GenDataTestsBase
    {
        /// <summary>
        /// Tests the parameter scanner functionality
        /// </summary>
        [TestCase(Description="Generator Parameter Scanner Test")]
        public void GenParameterScannerTest()
        {
            const string txt = "Property=Title[Title='Property Title',DataType=String,Read,Write,PrivateVar,Visibility=3]\r\n";
            GenDataDef.CreateMinimal();
            var scan = new ParameterScanner(txt);
            scan.ScanObject();
            Assert.IsFalse(scan.AtEnd);
            Assert.AreEqual("Property", scan.RecordType, "Property record type expected");
            Assert.AreEqual("Title", scan.Attribute("Name"), "Name attribute by default");
            Assert.AreEqual("Property Title", scan.Attribute("Title"), "Quoted attribute value");
            Assert.AreEqual("True", scan.Attribute("Read"), "Boolean attribute");
            Assert.AreEqual("", scan.Attribute("Missing"), "Missing attribute blank by default");
            scan.ScanObject();
            Assert.IsTrue(scan.Eof);
        }

        /// <summary>
        /// Tests the extraction of the data definition from text data
        /// </summary>
        [TestCase(Description="Generator Text Parameters Definition extraction test")]
        public void GenTextParameterDefinitionExtractTest()
        {
            var f = GenParameters.ExtractDef(GenDataSaveText);
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests the extraction of the data definition from file data
        /// </summary>
        [TestCase(Description = "Generator File Parameters Definition extraction test")]
        public void GenFileParameterDefinitionExtractTest()
        {
            CreateGenDataSaveText();
            GenDataDef f;
            using (var s = new FileStream("GenDataSaveData.txt", FileMode.Open, FileAccess.ReadWrite))
            {
                f = GenParameters.ExtractDef(s);
                Assert.AreEqual(0, s.Position);
            }
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests the scanning of a parameter file using a file stream with no definition
        /// </summary>
        [TestCase(Description="Generator Parameter Scanner Test")]
        public void GenParameterTest()
        {
            CreateGenDataSaveText();
            GenParameters d;
            using (var s = new FileStream("GenDataSaveData.txt", FileMode.Open, FileAccess.ReadWrite))
                d = new GenParameters(s);
            var f = d.AsDef();
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests that parameters are scanned correctly from literal text with no definition
        /// </summary>
        [TestCase(Description="Generator Text Parameters test")]
        public void GenTextParametersTest()
        {
            var d = new GenParameters(GenDataSaveText);
            var f = d.AsDef();
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests the scanning of a parameter file using a file stream with a definition
        /// </summary>
        [TestCase(Description="Generator Parameter Scanner Test")]
        public void GenDefParameterTest()
        {
            CreateGenDataSaveText();
            var f0 = GenDataDef.CreateMinimal();
            GenParameters d;
            using (var s = new FileStream("GenDataSaveData.txt", FileMode.Open, FileAccess.ReadWrite))
                d = new GenParameters(f0, s);
            var f = d.AsDef();
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests that parameters are scanned correctly from literal text with a defenition
        /// </summary>
        [TestCase(Description="Generator Text Parameters test")]
        public void GenDefTextParametersTest()
        {
            var f0 = GenDataDef.CreateMinimal();
            var d = new GenParameters(f0, GenDataSaveText);
            var f = d.AsDef();
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests the generator data save functionality
        /// </summary>
        [TestCase(Description = "Generator data save tests")]
        public void GenDataSaveTests()
        {
            const string fileName = "GenDataSaveTest.txt";
            const string expected = GenDataSaveText;

            var f = GenDataDef.CreateMinimal();
            f.Properties[ClassClassId].Add("Title");
            var a = new GenAttributes(f);
            var d = new GenData(f);
            SetUpData(d);
            d.First(ClassClassId);
            a.GenObject = d.Context[ClassClassId].Context;
            a.SetString("Title", "Class object");
            a.SaveFields();

            GenParameters.SaveToFile(d, fileName);

            var file = File.ReadAllText(fileName);
            Assert.AreEqual(expected, file);
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
