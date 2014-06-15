// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the generator data functionality
    /// </summary>
    [TestFixture]
    public class GenDataInheritanceTests : GenDataTestsBase
    {
        private const string VirtualDefintionProfile = @"Definition=
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
`]`[Virtual2^:Virtual2=`Virtual2.Name`[`?Virtual2.V2Field:V2Field`?Virtual2.V2Field<>True:=`@StringOrName:`{`Virtual2.V2Field``]`]`]`]]
`]`[Child:Child=`Child.Name`
`]`]`]";

        private const string VirtualDefintionData = @"Definition=
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
            var df = SetUpVirtualDefintion();
            Assert.AreEqual(VirtualDefintionProfile, GenDataDefProfile.CreateProfile(df.GenData.AsDef()));
            CompareGenData(df.GenData, df.GenData.AsDef().AsGenData());
        }

        [TestCase(Description = "Tests the setting up of a definition with inheritance")]
        public void InheritanceDataProfileTest()
        {
            var df = SetUpVirtualDefintion();
            var p = GenParameters.CreateProfile(df.GenData.AsDef());
            var profileText = p.ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary).Replace(">:", ":");
            Assert.AreEqual(VirtualDefintionProfile, profileText);
        }

        [TestCase(Description = "Tests the saving of data with inheritance")]
        public void InheritanceDataSaveTest()
        {
            var d = PopulateInheritanceData();
            GenParameters.SaveToFile(d, "InheritanceData.dcb");
            var text = File.ReadAllText("InheritanceData.dcb");
            Assert.AreEqual(VirtualDefintionData, text);
        }

        [TestCase(Description = "Tests the loading of data with inheritance")]
        public void InheritanceDataLoadTest()
        {
            var d = PopulateInheritanceData();
            var x = new GenParameters(d.GenDataDef, VirtualDefintionData);
            CompareGenData(d, x);
        }

        private static GenData PopulateInheritanceData()
        {
            var f = SetUpVirtualDefintion().GenData.AsDef();
            var d = new GenData(f);
            var container = new GenAttributes(f) {GenObject = d.Context[1].CreateObject()};
            container.SetString("Name", "Container");
            container.SaveFields();
            d.First(1);
            var genObject = d.Context[2].CreateObject();
            genObject.ClassId = 3;
            var @abstract = new GenAttributes(f) {GenObject = genObject};
            @abstract.SetString("Name", "V1Instance1");
            @abstract.SetString("V1Field", "Value 1");
            @abstract.SaveFields();
            d.Last(2);
            var child = new GenAttributes(f) {GenObject = d.Context[5].CreateObject()};
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

        private static Definition SetUpVirtualDefintion()
        {
            var df = new Definition();
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

        private void CompareGenData(GenData expected, GenData actual)
        {
            //Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.DataName, actual.DataName);
            Assert.AreEqual(expected.Context.Count, actual.Context.Count);
            CompareContext(0, 0, expected, actual);
        }

        private void CompareContext(int expectedId, int actualId, GenData expected, GenData actual)
        {
            var expectedContext = expected.Context[expectedId];
            var actualContext = actual.Context[actualId];
            var expectedAttributes = new GenAttributes(expected.GenDataDef);
            var actualAttributes = new GenAttributes(actual.GenDataDef);
            Assert.AreEqual(expectedContext.Count, actualContext.Count, "Class " + expectedId + " objects");
            expected.First(expectedId); actual.First(actualId);
            while (!expected.Eol(expectedId) && !actual.Eol(actualId))
            {
                Assert.AreEqual(expectedContext.ToString(), actualContext.ToString());
                Assert.AreEqual(expectedContext.DefClass.ToString(), actualContext.DefClass.ToString());
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
                expectedAttributes.GenObject = expectedObject;
                actualAttributes.GenObject = actualObject;
                Assert.GreaterOrEqual(expectedObject.Attributes.Count, actualObject.Attributes.Count);
                for (var i = 0; i < actualObject.Definition.Properties.Count; i++)
                    Assert.AreEqual(expectedAttributes.AsString(actualObject.Definition.Properties[i]),
                                    actualAttributes.AsString(actualObject.Definition.Properties[i]),
                                    actualObject.Definition.Properties[i] + 
                                    " " + expectedAttributes.AsString("Name") +
                                    " vs " + actualAttributes.AsString("Name"));
                //Assert.AreEqual();
                Assert.AreEqual(expectedObject.SubClass.Count, actualObject.SubClass.Count);
                for (var i = 0; i < actualObject.SubClass.Count; i++)
                {
                    Assert.AreEqual(expectedObject.SubClass[i].ClassId, actualObject.SubClass[i].ClassId);
                    Assert.AreEqual(expectedObject.SubClass[i].Reference, actualObject.SubClass[i].Reference);
                    CompareContext(expectedObject.SubClass[i].ClassId,
                                   actual.GenDataDef.Classes.IndexOf(
                                       expected.GenDataDef.Classes[expectedObject.SubClass[i].ClassId].Name), expected,
                                   actual);
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
