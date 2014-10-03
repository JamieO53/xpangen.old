// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;

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
        [Test(Description="Generator Parameter Scanner Test")]
        public void GenParameterScannerTest()
        {
            const string txt = "Property=Title[Title='Property Title',DataType=String,Read,Write,PrivateVar,Visibility=3]\r\n";
            GenDataDef.CreateMinimal();
            using (var scan = new ParameterScanner(txt))
            {
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
        }

        private readonly Dictionary<char, string> _escapeChars =
            new Dictionary<char, string>
                {
                    {'\t', @"\t"},
                    {'\n', @"\n"},
                    {'\r', @"\r"},
                    {'\\', @"\\"},
                    {'\f', @"\f"}
                };
        private readonly object[] _escapedChars = {'\t', '\n', '\r', '\\'};

        /// <summary>
        /// Tests that quoted strings with escape characters are scanned correctly
        /// </summary>
        [Test(Description = "Scanner Quoted String test with escaped characters")]
        [TestCaseSource("_escapedChars")]
        public void EscapedQuotedStringTest(char escapedChar)
        {
            var escapedString = _escapeChars[escapedChar];
            var txt = string.Format(@"'start{0}end'", escapedString); // ' is the quote character;
            using (var scan = new ParameterScanner(txt))
                Assert.AreEqual(string.Format("start{0}end", escapedChar), scan.ScanQuotedString());
        }

        private readonly Dictionary<char, string> _undefinedEscapeChars =
            new Dictionary<char, string>
                {
                    {'\f', @"\f"}
                };
        private readonly object[] _undefinedEscapedChars = { '\f' };

        /// <summary>
        /// Tests that quoted strings with undefined escape characters are scanned correctly
        /// </summary>
        [Test(Description = "Scanner Quoted String test with undefined escaped characters")]
        [TestCaseSource("_undefinedEscapedChars")]
        public void UndefinedEscapedQuotedStringTest(char escapedChar)
        {
            var escapedString = _undefinedEscapeChars[escapedChar];
            var txt = @"'start" + escapedString + "end'"; // ' is the quote character;
            using (var scan = new ParameterScanner(txt))
            {
                Assert.AreEqual(string.Format("start\\{0}end", escapedString.Substring(1)), scan.ScanQuotedString());
            }
        }

        /// <summary>
        /// Tests the extraction of the data definition from text data
        /// </summary>
        [Test(Description="Generator Text Parameters Definition extraction test")]
        public void GenTextParameterDefinitionExtractTest()
        {
            var f = GenParameters.ExtractDef(GenDataSaveText);
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests the extraction of the data definition from file data
        /// </summary>
        [Test(Description = "Generator File Parameters Definition extraction test")]
        public void GenFileParameterDefinitionExtractTest()
        {
            CreateGenDataSaveText("GenFileParameterDefinitionExtractTest.txt");
            GenDataDef f;
            using (var s = new FileStream("GenFileParameterDefinitionExtractTest.txt", FileMode.Open, FileAccess.ReadWrite))
            {
                f = GenParameters.ExtractDef(s);
                Assert.AreEqual(0, s.Position);
            }
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests the scanning of a parameter file using a file stream with no definition
        /// </summary>
        [Test(Description="Generator Parameter Scanner Test")]
        public void GenParameterTest()
        {
            CreateGenDataSaveText("GenParameterTest.txt");
            GenParameters d;
            using (var s = new FileStream("GenParameterTest.txt", FileMode.Open, FileAccess.ReadWrite))
                d = new GenParameters(s) { DataName = "GenParameterTest" };
            var f = d.AsDef();
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests that parameters are scanned correctly from literal text with no definition
        /// </summary>
        [Test(Description="Generator Text Parameters test")]
        public void GenTextParametersTest()
        {
            var d = new GenParameters(GenDataSaveText);
            var f = d.AsDef();
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests the scanning of a parameter file using a file stream with a definition
        /// </summary>
        [Test(Description="Generator Parameter Scanner Test")]
        public void GenDefParameterTest()
        {
            CreateGenDataSaveText("GenDefParameterTest.txt");
            var f0 = GenDataDef.CreateMinimal();
            GenParameters d;
            using (var s = new FileStream("GenDefParameterTest.txt", FileMode.Open, FileAccess.ReadWrite))
                d = new GenParameters(f0, s) { DataName = "GenDefParameterTest" };
            var f = d.AsDef();
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests that parameters are scanned correctly from literal text with a defenition
        /// </summary>
        [Test(Description="Generator Text Parameters test")]
        public void GenDefTextParametersTest()
        {
            var f0 = GenDataDef.CreateMinimal();
            var d = new GenParameters(f0, GenDataSaveText);
            var f = d.AsDef();
            VerifyAsDef(f);
        }

        /// <summary>
        /// Tests that the output profile is created correctly
        /// </summary>
        [Test(Description = "Generator Output Profile test")]
        public void OutputProfileTest()
        {
            var f0 = GenDataDef.CreateMinimal();
            var p = GenParameters.CreateProfile(f0);
            var p0 = GenDataDefProfile.CreateProfile(f0);
            Assert.AreEqual(p0,
                            p.ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary)
                             .Replace(">:", ":"));
            ValidateProfileData(p, f0);
        }

        /// <summary>
        /// Tests that the output profile is created correctly for data with a reference
        /// </summary>
        [Test(Description = "Generator Output Profile for data with a nested reference test")]
        public void ReferenceOutputProfileTest()
        {
            var dataGrandchildhild = SetUpParentChildData("Grandchild", "Greatgrandchild", "Greatgrandchild");
            var dataChild = SetUpParentChildReferenceData("Child", "Grandchild", "GrandchildDef", "Grandchild", dataGrandchildhild);
            var dataParent = SetUpParentChildReferenceData("Parent", "Child", "ChildDef", "Child", dataChild);
            var p = GenParameters.CreateProfile(dataParent.GenDataDef);
            Assert.AreEqual(GenDataDefProfile.CreateProfile(dataParent.GenDataDef),
                            p.ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary)
                             .Replace(">:", ":"));
        }

        /// <summary>
        /// Tests the scanning of a parameter file using a file stream with that is referenced
        /// </summary>
        [Test(Description = "Generator Parameter Scanner Reference Referred Test")]
        public void GenParameterReferenceReferredTest()
        {
            CreateGenDataSaveText("Grandchild.dcb", ReferenceGrandchildDefText);
            CreateGenDataSaveText("Grandchild.txt", ReferenceGrandchildText);
            GenParameters f;
            using (var s = new FileStream("Grandchild.dcb", FileMode.Open, FileAccess.ReadWrite))
                f = new GenParameters(s) { DataName = "Grandchild" };
            GenDataBase d;
            using (var s = new FileStream("Grandchild.txt", FileMode.Open, FileAccess.ReadWrite))
                d = new GenParameters(f.AsDef(), s) { DataName = "Grandchild" };
            Assert.AreEqual(3, d.GenDataDef.Classes.Count);
            var grandchild = GetFirstObject(d);
            Assert.AreEqual("Grandchild", grandchild.Attributes[0]);
            var greatgrandchild = GetFirstObjectOfSubClass(grandchild, "greatgrandchild");
            Assert.IsNotNull(greatgrandchild);
        }

        /// <summary>
        /// Tests the scanning of a parameter file using a file stream with no definition
        /// </summary>
        [Test(Description = "Generator Parameter Scanner Test")]
        public void GenParameterNestedReferenceTest()
        {
            CreateGenDataSaveText("Child.dcb", ReferenceChildDefText);
            CreateGenDataSaveText("Child.txt", ReferenceChildText);
            GenParameters f;
            using (var s = new FileStream("Child.dcb", FileMode.Open, FileAccess.ReadWrite))
                f = new GenParameters(s) { DataName = "Child" };
            GenDataBase d;
            using (var s = new FileStream("Child.txt", FileMode.Open, FileAccess.ReadWrite))
                d = new GenParameters(f.AsDef(), s) { DataName = "Child" };
            Assert.AreEqual(4, d.GenDataDef.Classes.Count);
            var child = GetFirstObject(d);
            Assert.AreEqual("Child", child.Attributes[0]);
            var grandchild = GetFirstObjectOfSubClass(child, "grandchild");
            Assert.IsNotNull(grandchild);
            Assert.That(d.Cache.ContainsKey("grandchild"));
        }

        /// <summary>
        /// Tests the generator data save functionality
        /// </summary>
        [Test(Description = "Generator data save tests")]
        public void GenDataSaveTests()
        {
            const string fileName = "GenDataSaveTest.txt";
            const string expected = GenDataSaveText;

            var f = GenDataDef.CreateMinimal();
            f.AddClassInstanceProperty(ClassClassId, "Title");
            var a = new GenAttributes(f, ClassClassId);
            var d = new GenDataBase(f);
            SetUpData(d);
            a.GenObject = GetFirstObject(d);
            a.SetString("Title", "Class object");
            a.SaveFields();

            GenParameters.SaveToFile(d, fileName);

            var file = File.ReadAllText(fileName);
            Assert.AreEqual(expected, file);
        }

        /// <summary>
        /// Tests the generator data save functionality
        /// </summary>
        [Test(Description = "Generator data with reference save tests")]
        public void ParentReferenceGenDataSaveTests()
        {
            const string fileNameDef = "ParentDef.dcb";
            const string expectedDef = ReferenceParentDefText;
            const string fileName = "Parent.dcb";
            const string expected = ReferenceParentText;

            var dataGrandchildhild = SetUpParentChildData("Grandchild", "Greatgrandchild", "Greatgrandchild");
            var dataChild = SetUpParentChildReferenceData("Child", "Grandchild", "GrandchildDef", "Grandchild", dataGrandchildhild);
            var dataParent = SetUpParentChildReferenceData("Parent", "Child", "ChildDef", "Child", dataChild);

            var genData = dataParent;

            GenParameters.SaveToFile(genData.GenDataDef.AsGenData().GenDataBase, fileNameDef);
            var file = File.ReadAllText(fileNameDef);
            Assert.AreEqual(expectedDef, file);

            GenParameters.SaveToFile(genData, fileName);
            file = File.ReadAllText(fileName);
            Assert.AreEqual(expected, file);
        }

        /// <summary>
        /// Tests the generator data save functionality
        /// </summary>
        [Test(Description = "Generator data with reference save tests")]
        public void ChildReferenceGenDataSaveTests()
        {
            const string fileNameDef = "ChildDef.dcb";
            const string expectedDef = ReferenceChildDefText;
            const string fileName = "Child.dcb";
            const string expected = ReferenceChildText;

            var dataGrandchildhild = SetUpParentChildData("Grandchild", "Greatgrandchild", "Greatgrandchild");
            var dataChild = SetUpParentChildReferenceData("Child", "Grandchild", "GrandchildDef", "Grandchild", dataGrandchildhild);

            var genData = dataChild;
            Assert.AreEqual("GrandchildDef", genData.GenDataDef.GetClassSubClasses(1)[0].Reference);

            var genDataDef = genData.GenDataDef.AsGenData();
            Assert.AreEqual("GrandchildDef", genDataDef.Context[2].GenObject.Attributes[1]);
            GenParameters.SaveToFile(genDataDef.GenDataBase, fileNameDef);
            var file = File.ReadAllText(fileNameDef);
            Assert.AreEqual(expectedDef, file);

            GenParameters.SaveToFile(genData, fileName);
            file = File.ReadAllText(fileName);
            Assert.AreEqual(expected, file);
        }

        /// <summary>
        /// Tests the generator data save functionality
        /// </summary>
        [Test(Description = "Generator data with reference save tests - referenced data")]
        public void GrandchildReferenceGenDataSaveTests()
        {
            const string fileNameDef = "GrandchildDef.dcb";
            const string expectedDef = ReferenceGrandchildDefText;
            const string fileName = "Grandchild.dcb";
            const string expected = ReferenceGrandchildText;

            var dataGrandchildhild = SetUpParentChildData("Grandchild", "Greatgrandchild", "Greatgrandchild");

            GenParameters.SaveToFile(dataGrandchildhild.GenDataDef.AsGenData().GenDataBase, fileNameDef);
            var file = File.ReadAllText(fileNameDef);
            Assert.AreEqual(expectedDef, file);

            GenParameters.SaveToFile(dataGrandchildhild, fileName);
            file = File.ReadAllText(fileName);
            Assert.AreEqual(expected, file);
        }

        /// <summary>
        /// Verifies that the Definition references in GeneratorDefinitionModel are loaded correctly
        /// </summary>
        [Test(Description = "Verifies that the Definition references in GeneratorDefinitionModel are loaded correctly")]
        public void GeneratorDefinitionModelDefinitionLoadTest()
        {
            var defData = GenDataBase.DataLoader.LoadData("ProgramDefinition");
            var def = defData.AsDef();
            var data = GenDataBase.DataLoader.LoadData(def, "GeneratorDefinitionModel");
            var definition = data.Cache["definition"];
            var d = GetLastObjectInSubClass(GetFirstObject(definition));
            var p = GetFirstObjectOfSubClass(d, "Property");
            Assert.AreEqual(7, p.ParentSubClass.Count);
            Assert.AreEqual("Property", def.GetClassName(5));
            Assert.AreEqual(7, def.GetClassProperties(5).Count);
            var a = GetFirstObjectOfSubClass(GetFirstObject(data), "Assembly");
            Assert.AreEqual("Definition", a.Attributes[0]);
            Assert.AreEqual("Definition.Class", a.SubClass[0].Definition.SubClass.ToString());
            var c = a.GetSubClass("Class")[0]; // Reference
            Assert.AreEqual("Class", c.Attributes[0]);
            c = GetNextObjectInSubClass(c);
            Assert.AreEqual("SubClass", c.Attributes[0]);
            c = GetNextObjectInSubClass(c);
            Assert.AreEqual("Property", c.Attributes[0]);
            Assert.AreEqual(7, c.SubClass[1].Count);
        }

        /// <summary>
        /// Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
            GenDataLoader.Register();
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
