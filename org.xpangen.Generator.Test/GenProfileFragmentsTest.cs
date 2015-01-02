// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the generator profile fragment functionality
    /// </summary>
	[TestFixture]
    public class GenProfileFragmentsTest : GenProfileFragmentsTestBase
    {
        private GenDataDef GenDataDef { get; set; }

        private GenDataBase GenData { get; set; }

        /// <summary>
        /// Tests for the correct creation of a text fragment
        /// </summary>
        [Test(Description="Generator text fragment test")]
        public void GenTextFragmentTest()
        {
            var r = new GenProfileFragment(new GenProfileParams(GenData.GenDataDef));
            var g = new GenTextFragment(new GenTextFragmentParams(GenDataDef, r, "Text fragment"));
            VerifyFragment(GenData, g, "GenTextFragment", FragmentType.Text, "Text", "Text fragment", "Text fragment",
                true, null, r.Profile.GenDataBase.GenDataDef);
        }

        /// <summary>
        /// Tests for the correct creation of an annotation fragment
        /// </summary>
        [Test(Description = "Generator Annotation fragment test")]
        public void GenAnnotationTest()
        {
            var r = new GenProfileFragment(new GenProfileParams(GenData.GenDataDef));

            var g = new GenAnnotation(new GenFragmentParams(GenDataDef, r));
            r.Body.Add(g);
            var p =
                new GenPlaceholderFragment(new GenPlaceholderFragmentParams(GenDataDef, g,
                    GenDataDef.GetId("Property.Name")));
            var t = new GenTextFragment(new GenTextFragmentParams(GenData.GenDataDef, g, ","));
            g.Body.Add(p);
            g.Body.Add(t);
            VerifyFragment(GenData, g, "GenAnnotation", FragmentType.Annotation, "Annotation", "`-`Property.Name`,`]", "", false, null, r.Profile.GenDataBase.GenDataDef);
        }

        /// <summary>
        /// Tests for the correct creation of a placeholder fragment
        /// </summary>
        [Test(Description="Generator placeholder test")]
        public void GenPlaceholderTest()
        {
            var r = new GenProfileFragment(new GenProfileParams(GenData.GenDataDef));
            var g =
                new GenPlaceholderFragment(new GenPlaceholderFragmentParams(GenDataDef, r,
                    GenDataDef.GetId("Property.Name")));
            g.GenObject = GetNextObjectInSubClass(GetFirstObjectOfSubClass(GetFirstObject(GenData), "Property"));
            VerifyFragment(GenData, g, "GenPlaceholderFragment", FragmentType.Placeholder, "Property.Name", "`Property.Name`",
                           "Property2", true, null, r.Profile.GenDataBase.GenDataDef);
        }

        /// <summary>
        /// Tests for the correct creation of a block fragment
        /// </summary>
        [Test(Description="Generator Block test")]
        public void GenBlockTest()
        {
            var r = new GenProfileFragment(new GenProfileParams(GenData.GenDataDef));

            var g = new GenBlock(new GenFragmentParams(GenDataDef, r));
            r.Body.Add(g);
            var p =
                new GenPlaceholderFragment(new GenPlaceholderFragmentParams(GenDataDef, g,
                    GenDataDef.GetId("Property.Name")));
            var t = new GenTextFragment(new GenTextFragmentParams(GenData.GenDataDef, g, ","));
            g.Body.Add(p);
            g.Body.Add(t);
            g.GenObject = GetNextObjectInSubClass(GetFirstObjectOfSubClass(GetFirstObject(GenData), "Property"));
            VerifyFragment(GenData, g, "GenBlock", FragmentType.Block, "Block", "`{`Property.Name`,`]", "Property2,", false, null, r.Profile.GenDataBase.GenDataDef);
        }

        /// <summary>
        /// Tests for the correct creation of a lookup fragment matching a name
        /// </summary>
        [Test(Description="Generator Lookup Match test")]
        public void GenLookupMatchTest()
        {
            var d = SetUpLookupData();
            var f = d.GenDataDef;
            var r = new GenProfileFragment(new GenProfileParams(GenData.GenDataDef));

            var g = new GenLookup(new GenLookupParams(f, r, "Class.Name=SubClass.Name"));
            var p = new GenPlaceholderFragment(new GenPlaceholderFragmentParams(f, g, f.GetId("Class.Name")));
            var t = new GenTextFragment(new GenTextFragmentParams(d.GenDataDef, g, ","));
            g.Body.Add(p);
            g.Body.Add(t);
            Assert.IsFalse(g.NoMatch);

            var c = GetLastObjectInSubClass(GetFirstObject(d)); // Has no subclasses
            var s = GetFirstObjectOfSubClass(c, "SubClass");
            g.GenObject = s;
            VerifyFragment(d, g, "GenLookup", FragmentType.Lookup, "Class.Name=SubClass.Name",
                           "`%Class.Name=SubClass.Name:`Class.Name`,`]", "", false, null, r.Profile.GenDataBase.GenDataDef);

            c = GetFirstObject(d); // Has subclasses
            s = GetFirstObjectOfSubClass(c, "SubClass");
            g.GenObject = s;//d.Context[SubClassClassId].GenObject;
            VerifyFragment(d, g, "GenLookup", FragmentType.Lookup, "Class.Name=SubClass.Name",
                           "`%Class.Name=SubClass.Name:`Class.Name`,`]", "SubClass,", false, null, r.Profile.GenDataBase.GenDataDef);
        }

        /// <summary>
        /// Tests for the correct creation of a lookup fragment where the given name does not match
        /// </summary>
        [Test(Description="Generator Lookup No Match test")]
        public void GenLookupNoMatchTest()
        {
            const string txt = "SubClass does not exist";
            const string profile = "`[Class:`[SubClass:`%Class.Name=SubClass.Name:`;" + txt + "`]`]`]";
            var d = SetUpLookupData();
            var f = d.GenDataDef;
            var p = new GenCompactProfileParser(f, "", profile);

            var g = ((GenSegment) ((GenSegment) p.Body.Fragment[0]).Body.Fragment[0]).Body.Fragment[0];
            Assert.AreEqual(FragmentType.Lookup, g.FragmentType);
            Assert.IsFalse(g.IsTextFragment);
            var genFragmentLabel = new GenFragmentLabel(g.Fragment);
            Assert.AreEqual("~Class.Name=SubClass.Name", genFragmentLabel.ProfileLabel());
            Assert.AreEqual("`%Class.Name=SubClass.Name:`;" + txt + "`]",
                            g.ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary));

            var c = GetLastObjectInSubClass(GetFirstObject(d)); // Has no subclasses
            Assert.AreEqual("Property", c.Attributes[0]);
            var s = GetFirstObjectOfSubClass(c, "SubClass");
            g.ParentContainer.GenObject = c;
            g.GenObject = s;
            Assert.AreEqual(txt, GenFragmentExpander.Expand(d.GenDataDef, g.GenObject, g.Fragment));
            var str = GenerateFragment(d, g);
            Assert.AreEqual(txt, str);

            c = GetFirstObject(d);
            Assert.AreEqual("Class", c.Attributes[0]);
            s = GetFirstObjectOfSubClass(c, "SubClass");
            Assert.AreEqual("SubClass", s.Attributes[0]);
            g.GenObject = s;
            str = GenerateFragment(d, g);
            Assert.AreEqual("", str);
        }

        /// <summary>
        /// Checks for the correct creation of a lookup in context fragment
        /// </summary>
        [Test(Description="Generator Lookup Context Test")]
        public void GenLookupContextTest()
        {
            var d = SetUpLookupContextData();
            var f = d.GenDataDef;
            var r = new GenProfileFragment(new GenProfileParams(GenData.GenDataDef));

            var b = new GenBlock(new GenFragmentParams(f, r));
            var p0 = new GenPlaceholderFragment(new GenPlaceholderFragmentParams(f, b, f.GetId("Parent.Name")));
            var t0 = new GenTextFragment(new GenTextFragmentParams(f, b, ","));
            b.Body.Add(p0);
            b.Body.Add(t0);

            var g = new GenLookup(new GenLookupParams(f, b, "Lookup.Name=Child.Lookup"));
            var p1 = new GenPlaceholderFragment(new GenPlaceholderFragmentParams(f, g, f.GetId("Lookup.Name")));
            var t1 = new GenTextFragment(new GenTextFragmentParams(f, g, ","));
            Assert.IsFalse(g.NoMatch);

            b.Body.Add(g);
            g.Body.Add(p1);
            g.Body.Add(t1);

            var parent = GenObject.GetContext(d.Root, "Parent");
            var child = GetFirstObjectOfSubClass(parent, "Child");
            b.GenObject = child;
            VerifyFragment(d, b, "GenBlock", FragmentType.Block, "Block",
                           "`{`Parent.Name`,`%Lookup.Name=Child.Lookup:`Lookup.Name`,`]`]", "Parent,Valid,", false, null, r.Profile.GenDataBase.GenDataDef);

            child = GetLastObjectInSubClass(child);
            b.GenObject = child;
            VerifyFragment(d, b, "GenBlock", FragmentType.Block, "Block",
                           "`{`Parent.Name`,`%Lookup.Name=Child.Lookup:`Lookup.Name`,`]`]", "Parent,", false, null, r.Profile.GenDataBase.GenDataDef);
        }

        public class TestConditionParamsList : List<TestConditionParams>
        {
            public TestConditionParamsList()
            {
                Add(new TestConditionParams("Property.Name Exists", "Property.Name", "Property.Name", true, "Test for Existence 1"));
                Add(new TestConditionParams("Property.Name", "", "", true, "Test for Existence 2"));
                Add(new TestConditionParams("Property.NameBlank", "", "", false, "Test for Existence 3"));
                Add(new TestConditionParams("Property.Name~", "", "", false, "Test for non-existence 1"));
                Add(new TestConditionParams("Property.NameBlank~", "", "", true, "Test for non-existence 2"));
            }
        }
        
        /// <summary>
        /// Tests for the correct creation of an attribute value existing condition fragment
        /// </summary>
        [Test(Description="Generator Existence Condition test"), TestCaseSource(typeof(TestConditionParamsList))]
        public void GenExistenceConditionTest(TestConditionParams conditionParams)
        {
            var d = SetUpComparisonData();

            TestCondition(d, conditionParams);
        }

        /// <summary>
        /// Tests for the correct creation of a condition matching a literal value
        /// </summary>
        [Test(Description = "Generator Literal Condition test")]
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
        [Test(Description="Generator Numeric Literal Condition Test")]
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
        [Test(Description="Generator Identifier Condition test")]
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
