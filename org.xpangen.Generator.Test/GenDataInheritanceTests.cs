// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
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
`[Abstract:`[Virtual1:Virtual1=`Virtual1.Name`[`?Virtual1.V1Field:V1Field`?Virtual1.V1Field<>True:=`@StringOrName:`{`Virtual1.V1Field``]`]`]`]]
`]`[Virtual2:Virtual2=`Virtual2.Name`[`?Virtual2.V2Field:V2Field`?Virtual2.V2Field<>True:=`@StringOrName:`{`Virtual2.V2Field``]`]`]`]]
`]`[Child:Child=`Child.Name`
`]`]`]";

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
