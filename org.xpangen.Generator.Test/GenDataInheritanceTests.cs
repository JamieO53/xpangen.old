// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the generator data functionality
    /// </summary>
    [TestFixture]
    public class GenDataInheritanceTests : GenProfileFragmentsTestBase
    {
        private const string VirtualDefinition = @"Definition=Definition
Class=Class
Field={Name,Title,Inheritance}
SubClass={SubClass,Property}
Class=SubClass
Field={Name,Reference,Relationship}
Class=Property
Field={Name,Title,DataType,Default,LookupType,LookupDependence,LookupTable}
.
Class=Container[]
SubClass=Abstract[]
Property=Name[,DataType=Identifier]
Class=Abstract[Title='Abstract class',Inheritance=Abstract]
SubClass=Virtual1[,Relationship=Extends]
SubClass=Virtual2[,Relationship=Extends]
SubClass=Child[]
Property=Name[,DataType=Identifier]
Class=Virtual1[]
Property=V1Field[,DataType=String]
Class=Virtual2[]
Property=V2Field[,DataType=String]
Class=Child[]
Property=Name[,DataType=Identifier]
";

        private const string VirtualDefinitionProfile = @"Definition=VirtualDefinition
Class=Container
Field=Name
SubClass=Abstract
Class=Abstract[Virtual1,Virtual2]
Field=Name
SubClass=Child
Class=Virtual1
Field=V1Field
Class=Virtual2
Field=V2Field
Class=Child
Field=Name
.
`[Container:Container=`Container.Name`
`[Abstract:`[Virtual1^:Virtual1=`Virtual1.Name`[`?Virtual1.V1Field:V1Field`?Virtual1.V1Field<>True:=`@StringOrName:`{`Virtual1.V1Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`[Virtual2^:Virtual2=`Virtual2.Name`[`?Virtual2.V2Field:V2Field`?Virtual2.V2Field<>True:=`@StringOrName:`{`Virtual2.V2Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`]`]";

        private const string VirtualDefinitionData = @"Definition=VirtualDefinition
Class=Container
Field=Name
SubClass=Abstract
Class=Abstract[Virtual1,Virtual2]
Field=Name
SubClass=Child
Class=Virtual1
Field=V1Field
Class=Virtual2
Field=V2Field
Class=Child
Field=Name
.
Container=Container
Virtual1=V1Instance1[V1Field='Value 1']
Child=V1I1Child1
Child=V1I1Child2
Virtual2=V2Instance1[V2Field='Value 1']
Child=V2I1Child1
Child=V2I1Child2
Virtual1=V1Instance2[V1Field='Value 2']
Child=V1I2Child1
Child=V1I2Child2
Virtual2=V2Instance2[V2Field='Value 2']
Child=V2I2Child1
Child=V2I2Child2
";

        [TestCase(Description = "Tests the setting up of a definition with inheritance")]
        public void InheritanceDefinitionSetupTest()
        {
            var df = SetUpVirtualDefinition();
            var def = df.GenData.AsDef();
            Assert.AreEqual(VirtualDefinitionProfile, GenDataDefProfile.CreateProfile(def));
            var data = def.AsGenData();
            CompareGenData(df.GenData, data);
        }

        [TestCase(Description = "Tests the creation of a data profile with inheritance")]
        public void InheritanceDataProfileTest()
        {
            var df = SetUpVirtualDefinition();
            var p = GenParameters.CreateProfile(df.GenData.AsDef());
            var profileText = p.ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary).Replace(">:", ":");
            Assert.AreEqual(VirtualDefinitionProfile, profileText);
        }

        [TestCase(Description = "Tests the saving of data with inheritance")]
        public void InheritanceDataSaveTest()
        {
            var d = PopulateInheritanceData();
            var p = GenParameters.CreateProfile(d.GenDataDef);
            var text = p.Expand(d);
            Assert.AreEqual(VirtualDefinitionData, text);
        }

        [TestCase(Description = "Tests the expansion of data with inheritance")]
        public void InheritanceDataExpansionTest()
        {
            var d = PopulateInheritanceData();
            GenParameters.SaveToFile(d, "InheritanceData.dcb");
            var text = File.ReadAllText("InheritanceData.dcb");
            Assert.AreEqual(VirtualDefinitionData, text);
        }

        [TestCase(Description = "Tests the loading of data with inheritance")]
        public void InheritanceDataLoadTest()
        {
            var d = PopulateInheritanceData();
            var x = GenData.DataLoader.LoadData(d.GenDataDef, VirtualDataFile);
            CompareGenData(d, x);
        }

        [TestCase(Description = "Tests the loading of data with inheritance without a definition")]
        public void InheritanceDataLoadSansDefinitionTest()
        {
            var d = PopulateInheritanceData();
            var x = GenData.DataLoader.LoadData(VirtualDataFile);
            CompareGenData(d, x);
        }

        private const string InheritanceProfile = @"`[Container:Container=`Container.Name`
`[Virtual1:Virtual1=`Virtual1.Name`[`?Virtual1.V1Field:V1Field`?Virtual1.V1Field<>True:=`@StringOrName:`{`Virtual1.V1Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`[Virtual2:Virtual2=`Virtual2.Name`[`?Virtual2.V2Field:V2Field`?Virtual2.V2Field<>True:=`@StringOrName:`{`Virtual2.V2Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`]";

        private const string NestedInheritanceProfile = @"`[Parent:`[Container:Container=`Container.Name`
`[Virtual1:Virtual1=`Virtual1.Name`[`?Virtual1.V1Field:V1Field`?Virtual1.V1Field<>True:=`@StringOrName:`{`Virtual1.V1Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`[Virtual2:Virtual2=`Virtual2.Name`[`?Virtual2.V2Field:V2Field`?Virtual2.V2Field<>True:=`@StringOrName:`{`Virtual2.V2Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`]`]";

        private const string InheritanceProfileResult = @"Container=Container
Virtual1=V1Instance1[V1Field='Value 1']
Child=V1I1Child1
Child=V1I1Child2
Virtual1=V1Instance2[V1Field='Value 2']
Child=V1I2Child1
Child=V1I2Child2
Virtual2=V2Instance1[V2Field='Value 1']
Child=V2I1Child1
Child=V2I1Child2
Virtual2=V2Instance2[V2Field='Value 2']
Child=V2I2Child1
Child=V2I2Child2
";

        [TestCase(Description = "Tests the expansion of a class with inheritance")]
        public void InheritanceClassExpansionTest()
        {
            var d = PopulateInheritanceData();
            var p = new GenCompactProfileParser(d, "", InheritanceProfile);
            var text = p.Expand(d);
            Assert.AreEqual(InheritanceProfileResult, text);
        }

        [TestCase(Description = "Tests the generation of a class with inheritance")]
        public void InheritanceClassGenerationTest()
        {
            var d = PopulateInheritanceData();
            var p = new GenCompactProfileParser(d, "", InheritanceProfile);
            var text = GenerateFragment(d, p);
            Assert.AreEqual(InheritanceProfileResult, text);
        }

        private const string VirtualDefinitionFile = "TestData\\VirtualDefinition.dcb";
        private const string VirtualDataFile = "TestData\\VirtualData.dcb";
        private const string VirtualParentDefinitionFile = "TestData\\VirtualParentDefinition.dcb";
        private const string VirtualParentDataFile = "TestData\\VirtualParentData.dcb";

        private const string VirtualParentDefinition = @"Definition=Definition
Class=Class
Field={Name,Title,Inheritance}
SubClass={SubClass,Property}
Class=SubClass
Field={Name,Reference,Relationship}
Class=Property
Field={Name,Title,DataType,Default,LookupType,LookupDependence,LookupTable}
.
Class=Parent[]
SubClass=Container[Reference='TestData/VirtualDefinition']
Property=Name[,DataType=String]
";
        private const string VirtualParentData = @"Definition=VirtualParentDefinition
Class=Parent
Field=Name
SubClass=Container[Reference='TestData/VirtualDefinition']
.
Parent=Parent
Container[Reference='TestData\VirtualData']
";

        [TestCase(Description = "Tests the loading of nested data with inheritance")]
        public void NestedInheritanceDataLoadTest()
        {
            var data = LoadVirtualParentData();
            var expectedData = SetUpParentOfVirtualData();
            CompareGenData(expectedData, data);
        }

        private static GenData LoadVirtualParentData()
        {
            SetUpParametersFile(VirtualDefinitionFile, VirtualDefinition);
            SetUpParametersFile(VirtualDataFile, VirtualDefinitionData);
            SetUpParametersFile(VirtualParentDefinitionFile, VirtualParentDefinition);
            SetUpParametersFile(VirtualParentDataFile, VirtualParentData);
            var data = GenData.DataLoader.LoadData(VirtualParentDataFile);
            return data;
        }

        private static void SetUpParametersFile(string fileName, string text)
        {
            if (File.Exists(fileName)) File.Delete(fileName);
            File.WriteAllText(fileName, text);
        }

        [TestCase(Description = "Tests the saving of nested data with inheritance")]
        public void NestedInheritanceDataSaveTest()
        {
            SaveVirtualAndParentData();
            var def = File.ReadAllText(VirtualDefinitionFile);
            Assert.AreEqual(VirtualDefinition, def);
            var data = File.ReadAllText(VirtualDataFile);
            Assert.AreEqual(VirtualDefinitionData, data);
            def = File.ReadAllText(VirtualParentDefinitionFile);
            Assert.AreEqual(VirtualParentDefinition, def);
            data = File.ReadAllText(VirtualParentDataFile);
            Assert.AreEqual(VirtualParentData, data);
        }

        [TestCase(Description = "Tests the expansion of a nested class with inheritance")]
        public void NestedInheritanceClassExpansionTest()
        {
            var d = LoadVirtualParentData();
            var p = new GenCompactProfileParser(d, "", NestedInheritanceProfile);
            var text = p.Expand(d);
            Assert.AreEqual(InheritanceProfileResult, text);
        }

        private static void SaveVirtualAndParentData()
        {
            if (!Directory.Exists("TestData")) Directory.CreateDirectory("TestData");

            var df = SetUpVirtualDefinition();
            if (File.Exists(VirtualDefinitionFile)) File.Delete(VirtualDefinitionFile);
            GenParameters.SaveToFile(df.GenData, VirtualDefinitionFile);
            var d = PopulateInheritanceData();
            if (File.Exists(VirtualDataFile)) File.Delete(VirtualDataFile);
            GenParameters.SaveToFile(d, VirtualDataFile);

            var pdf = SetUpParentOfVirtualDefinition();
            if (File.Exists(VirtualParentDefinitionFile)) File.Delete(VirtualParentDefinitionFile);
            GenParameters.SaveToFile(pdf.GenData, VirtualParentDefinitionFile);
            var pd = SetUpParentOfVirtualData();
            if (File.Exists(VirtualParentDataFile)) File.Delete(VirtualParentDataFile);
            GenParameters.SaveToFile(pd, VirtualParentDataFile);
        }

        private static GenData PopulateInheritanceData()
        {
            var f = SetUpVirtualDefinition().GenData.AsDef();
            var d = new GenData(f) {DataName = "VirtualData"};
            var container = new GenAttributes(f, 1) {GenObject = d.Context[1].CreateObject()};
            container.SetString("Name", "Container");
            container.SaveFields();
            d.First(1);
            var genObject = d.Context[2].CreateObject();
            genObject.ClassId = 3;
            var @abstract = new GenAttributes(f, 3) {GenObject = genObject};
            @abstract.SetString("Name", "V1Instance1");
            @abstract.SetString("V1Field", "Value 1");
            @abstract.SaveFields();
            d.Last(2);
            var child = new GenAttributes(f, 5) {GenObject = d.Context[5].CreateObject()};
            child.SetString("Name", "V1I1Child1");
            child.SaveFields();
            child.GenObject = d.Context[5].CreateObject();
            child.SetString("Name", "V1I1Child2");
            child.SaveFields();
            genObject = d.Context[2].CreateObject();
            genObject.ClassId = 4;
            @abstract.GenObject = genObject;
            @abstract.SetString("Name", "V2Instance1");
            @abstract.SetString("V2Field", "Value 1");
            @abstract.SaveFields();
            d.Last(2);
            child.GenObject = d.Context[5].CreateObject();
            child.SetString("Name", "V2I1Child1");
            child.SaveFields();
            child.GenObject = d.Context[5].CreateObject();
            child.SetString("Name", "V2I1Child2");
            child.SaveFields();
            genObject = d.Context[2].CreateObject();
            genObject.ClassId = 3;
            @abstract.GenObject = genObject;
            @abstract.SetString("Name", "V1Instance2");
            @abstract.SetString("V1Field", "Value 2");
            @abstract.SaveFields();
            d.Last(2);
            child.GenObject = d.Context[5].CreateObject();
            child.SetString("Name", "V1I2Child1");
            child.SaveFields();
            child.GenObject = d.Context[5].CreateObject();
            child.SetString("Name", "V1I2Child2");
            child.SaveFields();
            genObject = d.Context[2].CreateObject();
            genObject.ClassId = 4;
            @abstract.GenObject = genObject;
            @abstract.SetString("Name", "V2Instance2");
            @abstract.SetString("V2Field", "Value 2");
            @abstract.SaveFields();
            d.Last(2);
            child.GenObject = d.Context[5].CreateObject();
            child.SetString("Name", "V2I2Child1");
            child.SaveFields();
            child.GenObject = d.Context[5].CreateObject();
            child.SetString("Name", "V2I2Child2");
            child.SaveFields();
            return d;
        }

        private static Definition SetUpVirtualDefinition()
        {
            var df = new Definition {GenData = {DataName = "VirtualDefinition"}};
            var c = df.AddClass("Container");
            c.AddProperty("Name", dataType: "Identifier");
            c.AddSubClass("Abstract");
            var a = df.AddClass("Abstract", "Abstract class", "Abstract");
            a.AddProperty("Name", dataType: "Identifier");
            a.AddSubClass("Virtual1").Relationship = "Extends";
            a.AddSubClass("Virtual2").Relationship = "Extends";
            a.AddSubClass("Child");
            var v1 = df.AddClass("Virtual1");
            v1.AddProperty("V1Field");
            var v2 = df.AddClass("Virtual2");
            v2.AddProperty("V2Field");
            var ch = df.AddClass("Child");
            ch.AddProperty("Name", dataType: "Identifier");
            return df;
        }

        private static Definition SetUpParentOfVirtualDefinition()
        {
            Assert.IsTrue(File.Exists(VirtualDefinitionFile));
            Assert.IsTrue(File.Exists(VirtualDataFile));
            var df = new Definition {GenData = {DataName = "VirtualParentDefintion"}};
            var c = df.AddClass("Parent");
            c.AddProperty("Name");
            c.AddSubClass("Container", "TestData/VirtualDefinition");
            return df;
        }

        private static GenData SetUpParentOfVirtualData()
        {
            Assert.IsTrue(File.Exists(VirtualDefinitionFile));
            Assert.IsTrue(File.Exists(VirtualParentDefinitionFile));
            Assert.IsTrue(File.Exists(VirtualDataFile));
            var f = GenData.DataLoader.LoadData(VirtualParentDefinitionFile).AsDef();
            var d = new GenData(f) { DataName = "VirtualParentData" };
            var container = new GenAttributes(f, 1) { GenObject = d.Context[1].CreateObject() };
            container.SetString("Name", "Parent");
            container.SaveFields();
            d.First(1);
            d.Context[1].GenObject.SubClass[0].Reference = "TestData\\VirtualData";
            return d;
        }

        private void CompareGenData(GenData expected, GenData actual)
        {
            //Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.DataName, actual.DataName);
            Assert.AreEqual(expected.Context.Count, actual.Context.Count);
            CompareContext(0, 0, expected, actual);
        }

        private static void CompareContext(int expectedId, int actualId, GenData expected, GenData actual)
        {
            var expectedContext = expected.Context[expectedId];
            var actualContext = actual.Context[actualId];
            Assert.AreEqual(expectedContext.ToString(), actualContext.ToString());
            Assert.AreEqual(expectedContext.Count, actualContext.Count, "Class " + expectedId + " objects");
            Assert.AreEqual(expectedContext.ClassId, actualContext.ClassId);
            Assert.AreEqual(expectedContext.RefClassId, actualContext.RefClassId);
            Assert.AreEqual(expectedContext.Reference, expectedContext.Reference);
            Assert.AreEqual(expectedContext.DefClass.ToString(), actualContext.DefClass.ToString());
            expected.First(expectedId); actual.First(actualId);
            while (!expected.Eol(expectedId) && !actual.Eol(actualId))
            {
                if (expectedContext.DefSubClass != null || actualContext.DefSubClass != null)
                {
                    Assert.IsNotNull(expectedContext.DefSubClass);
                    Assert.IsNotNull(actualContext.DefSubClass);
                }
                if (expectedContext.DefSubClass != null && actualContext.DefSubClass != null)
                    Assert.AreEqual(expectedContext.DefSubClass.ToString(),
                                    actualContext.DefSubClass.ToString());
                var expectedObject = expectedContext.GenObject;
                var actualObject = actualContext.GenObject;
                Assert.AreEqual(expectedObject.ClassId, actualObject.ClassId);
                var expectedAttributes = new GenAttributes(expected.GenDataDef, expectedObject.ClassId);
                var actualAttributes = new GenAttributes(actual.GenDataDef, actualObject.ClassId);
                expectedAttributes.GenObject = expectedObject;
                actualAttributes.GenObject = actualObject;
                Assert.GreaterOrEqual(expectedObject.Attributes.Count, actualObject.Attributes.Count);
                for (var i = 0; i < actualObject.Definition.Properties.Count; i++)
                    Assert.AreEqual(expectedAttributes.AsString(actualObject.Definition.Properties[i]),
                                    actualAttributes.AsString(actualObject.Definition.Properties[i]),
                                    actualObject.Definition.Properties[i] + 
                                    " " + expectedAttributes.AsString("Name") +
                                    " vs " + actualAttributes.AsString("Name"));

                Assert.AreEqual(expectedObject.SubClass.Count, actualObject.SubClass.Count);
                for (var i = 0; i < actualObject.SubClass.Count; i++)
                {
                    var expectedSubClassDef = expected.Context[expectedId].DefClass.SubClasses[i].SubClass;
                    var actualSubClassDef = actual.Context[expectedId].DefClass.SubClasses[i].SubClass;
                    Assert.AreEqual(expectedSubClassDef.ToString(), actualSubClassDef.ToString());
                    Assert.AreEqual(expectedSubClassDef.ClassId, actualSubClassDef.ClassId);
                    Assert.AreEqual(expectedSubClassDef.IsInherited, actualSubClassDef.IsInherited);
                    Assert.AreEqual(expectedSubClassDef.IsAbstract, actualSubClassDef.IsAbstract);
                    Assert.AreEqual(expectedObject.SubClass[i].ClassId, actualObject.SubClass[i].ClassId);
                    Assert.AreEqual(expectedObject.SubClass[i].Reference, actualObject.SubClass[i].Reference);
                    CompareContext(expectedSubClassDef.ClassId,
                                   actualSubClassDef.ClassId, expected, actual);
                }

                Assert.AreEqual(expectedContext.DefClass.Inheritors.Count, actualContext.DefClass.Inheritors.Count);
                for (var i = 0; i < actualContext.DefClass.Inheritors.Count; i++)
                {
                    var expectedDefInheritor = expectedContext.DefClass.Inheritors[i];
                    var actualDefInheritor = actualContext.DefClass.Inheritors[i];
                    Assert.AreEqual(expectedDefInheritor.ClassId, actualDefInheritor.ClassId);
                    Assert.Less(expectedId, expectedDefInheritor.ClassId);
                }
                expected.Next(expectedId); actual.Next(actualId);
            }
            Assert.AreEqual(expected.Eol(expectedId), actual.Eol(actualId));
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
