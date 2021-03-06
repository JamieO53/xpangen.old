// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Test
{
    public class GenProfileFragmentsTestBase : GenDataTestsBase
    {
        protected static GenDataBase SetUpData()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenDataBase(f);
            var c = CreateGenObject(d.Root, "Class", "Class");
            CreateGenObject(c, "SubClass", "SubClass");
            CreateGenObject(c, "Property", "Property");
            CreateGenObject(c, "Property", "Property2");
            return d;
        }

        protected static GenDataBase SetUpComparisonData()
        {
            var f = GenDataDef.CreateMinimal();
            f.AddClassInstanceProperty(PropertyClassId, "NameLT");
            f.AddClassInstanceProperty(PropertyClassId, "NameEQ");
            f.AddClassInstanceProperty(PropertyClassId, "NameGT");
            f.AddClassInstanceProperty(PropertyClassId, "NameBlank");

            var d = new GenDataBase(f);
            SetUpData(d);

            var c = GetFirstObject(d);
            Assert.AreEqual("Class", c.Attributes[0]);
            var a = new GenAttributes(f, ClassClassId) { GenObject = c };
            a.SetString("NameLT", "Clasa");
            a.SetString("NameEQ", "Class");
            a.SetString("NameGT", "Clasz");
            a.SetString("NameBlank", "");
            a.SaveFields();

            c = GetLastObjectInSubClass(c);
            Assert.AreEqual("Property", c.Attributes[0]);
            a.GenObject = GetFirstObjectOfSubClass(c, "Property");
            a.SetString("NameLT", "Nama");
            a.SetString("NameEQ", "Name");
            a.SetString("NameGT", "Namz");
            a.SetString("NameBlank", "");
            a.SaveFields();

            return d;
        }

        protected static GenDataBase SetUpNumericComparisonData()
        {
            var f = GenDataDef.CreateMinimal();
            f.AddClassInstanceProperty(PropertyClassId, "Number");

            var d = new GenDataBase(f);
            var a = new GenAttributes(f, PropertyClassId);
            SetUpData(d);
            a.GenObject = GetFirstObjectOfSubClass(GetLastObjectInSubClass(GetFirstObject(d)), "Property");
            a.SetNumber("Number", 15);
            a.SaveFields();

            return d;
        }

        
        public class TestConditionParams
        {
            public TestConditionParams(string condIn, string condOut, string profileLabel, bool expected, string comment, bool identifier = false)
            {
                CondIn = condIn;
                CondOut = condOut;
                ProfileLabel = profileLabel;
                Expected = expected;
                Comment = comment;
                Identifier = identifier;
            }

            public string CondIn { get; private set; }

            public string CondOut { get; set; }

            public string ProfileLabel { get; set; }

            public bool Expected { get; private set; }
            public string Comment { get; set; }

            public bool Identifier { get; private set; }

            public override string ToString()
            {
                return Comment;
            }
        }

        protected static void TestCondition(GenDataBase genData, TestConditionParams testConditionParams)
        {
            var genDataDef = genData.GenDataDef;
            if (testConditionParams.CondOut == "") testConditionParams.CondOut = testConditionParams.CondIn;
            if (testConditionParams.ProfileLabel == "") testConditionParams.ProfileLabel = testConditionParams.CondIn;

            var root = new GenProfileFragment(new GenProfileParams(genDataDef));
            const string r = "Condition holds";
            var exp = testConditionParams.Expected ? r : "";

            var c = ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary.ParseCondition(genDataDef, testConditionParams.CondIn);
            var g = new GenCondition(new GenConditionParams(genDataDef, root, c));
            g.GenObject = GetFirstObjectOfSubClass(GetLastObjectInSubClass(GetFirstObject(genData)), "Property");
            var t = new GenTextFragment(new GenTextFragmentParams(genDataDef, g, r));
            g.Body.Add(t);
            VerifyFragment(genData, g, "GenCondition", FragmentType.Condition, testConditionParams.ProfileLabel,
                           String.Format("`?{0}:{1}`]", testConditionParams.CondOut, r), exp, false, null, g.Fragment.GenDataBase.GenDataDef);
        }

        protected static void TestIdentifierComparison(GenDataBase genData, string comparison, bool expectedLt, bool expectedEq,
                                             bool expectedGt)
        {
            var condIn = "Property.Name" + comparison + "Property.Name";
            TestComparisonConditions(genData, expectedLt, expectedEq, expectedGt, condIn, "LT", "EQ", "GT", true);
        }

        protected static void TestNumericComparison(GenDataBase genData, string comparison, bool expectedLt, bool expectedEq,
                                             bool expectedGt)
        {
            var condIn = "Property.Number" + comparison;
            TestComparisonConditions(genData, expectedLt, expectedEq, expectedGt, condIn, "1", "15", "1500");
        }

        protected static void TestComparison(GenDataBase genData, string comparison, bool expectedLt, bool expectedEq,
                                             bool expectedGt)
        {
            var condIn = "Property.Name" + comparison;
            TestComparisonConditions(genData, expectedLt, expectedEq, expectedGt, condIn, "Nama", "Name", "Namz");
        }

        private static void TestComparisonConditions(GenDataBase genData, bool expectedLt, bool expectedEq, bool expectedGt,
                                                     string condIn, string valueLt, string valueEq, string valueGt, bool identifier = false)
        {
            TestCondition(genData, new TestConditionParams(condIn + valueLt, "", "", expectedLt, "", identifier: identifier)); // Property.Name?Nama
            TestCondition(genData, new TestConditionParams(condIn + valueEq, "", "", expectedEq, "", identifier: identifier)); // Property.Name?Name
            TestCondition(genData, new TestConditionParams(condIn + valueGt, "", "", expectedGt, "", identifier: identifier)); // Property.Name?Namz
        }

        protected static void VerifyFragment(GenDataBase genData, GenFragment genFragment, string expectedClass, FragmentType expectedType, string profileLabel, string profileText, string expected, bool isText, string parentClassName, GenDataDef profileDataDef)
        {
            Assert.AreEqual(expectedClass, genFragment.GetType().Name, "Fragment Class");
            Assert.AreEqual(expectedType, genFragment.Fragment.FragmentType,  "Fragment Fragment Type");
            Assert.AreEqual(expectedType, genFragment.FragmentType, "Fragment Type");
            Assert.AreEqual(isText, genFragment.IsTextFragment, "Is text fragment?");
            if (genFragment.GenObject == null) genFragment.GenObject = GetFirstObjectOfSubClass(GetFirstObject(genData), "Property");
            var fragment = genFragment.Fragment;
            Assert.AreEqual(expected, GenFragmentExpander.Expand(genData.GenDataDef, genFragment.GenObject, fragment), "Expanded fragment");
            var genFragmentLabel =new GenFragmentLabel(fragment);
            Assert.AreEqual(profileLabel, genFragmentLabel.ProfileLabel(), "Profile label");
            Assert.AreEqual(profileText, genFragment.ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary), "Profile text");
            if (parentClassName != null)
                Assert.AreEqual(parentClassName, genFragment.Fragment.ClassName(), "Parent Class ID");
            var str = GenerateFragment(genData, genFragment);
            Assert.AreEqual(expected, str);
            var genDataDef = genData.GenDataDef;
            ValidateFragmentData(genDataDef, parentClassName, genFragment, profileDataDef);
        }

        protected static string GenerateFragment(GenDataBase genData, GenFragment fragment)
        {
            using (var s = new MemoryStream(100000))
            {
                var w = new GenWriter(s);
                GenFragmentGenerator.Generate(genData.GenDataDef, w, fragment.GenObject ?? genData.Root, fragment.Fragment);
                w.Flush();
                s.Seek(0, SeekOrigin.Begin);
                var r = new StreamReader(s);
                return r.ReadToEnd();
            }
        }

        protected static void ExecuteFunction(GenDataBase genData, string functionName, string variableName, string variableValue, string expected)
        {
            ExecuteFunction(genData, functionName, expected, new []{variableName, variableValue});
        }

        protected static void ExecuteFunction(GenDataBase genData, string functionName, string expected, string[] values)
        {
            var r = new GenProfileFragment(new GenProfileParams(genData.GenDataDef));
            var g = new GenFunction(new GenFunctionParams(genData.GenDataDef, r, functionName));
            r.Body.Add(g);
            var b = SetFunctionParameters(genData, g, values);
            VerifyFragment(genData, g, "GenFunction", FragmentType.Function, functionName,
                "`@" + functionName + ':' + b + "`]", expected, false, null, r.Profile.GenDataBase.GenDataDef);
        }

        private static string SetFunctionParameters(GenDataBase genData, GenFunction genFunction, string[] values)
        {
            foreach (var value in values)
                SetFunctionParameter(genData, genFunction, value);
            return string.Join(" ", values);
        }

        private static void SetFunctionParameter(GenDataBase genData, GenFunction genFunction, string value)
        {
            var p0 = new GenTextFragment(new GenTextFragmentParams(genData.GenDataDef, genFunction, value));
            genFunction.Body.Add(p0);
        }

        protected void CheckFunctionVarValue(GenDataBase genData, string expected)
        {
            ExecuteFunction(genData, "Get", expected, new []{"Var", ""});
        }

        private static GenDataBase SetUpSegmentSeparatorData(string display)
        {
            var fm = GenDataDef.CreateMinimal();
            var dm = new GenDataBase(fm);
            var td = CreateClass(dm, "TestData");
            CreateProperty("Name", td);
            CreateProperty("Display", td);
            var f = dm.AsDef();
            var a = new GenAttributes(f, 1);
            var d = new GenDataBase(f);
            a.GenObject = CreateGenObject(d.Root, "TestData", "One");
            a.SetString("Name", "One");
            a.SetString("Display", display[0] == '1' ? "True" : "");
            a.SaveFields();
            a.GenObject = CreateGenObject(d.Root, "TestData", "Two");
            a.SetString("Name", "Two");
            a.SetString("Display", display[1] == '1' ? "True" : "");
            a.SaveFields();
            a.GenObject = CreateGenObject(d.Root, "TestData", "Three");
            a.SetString("Name", "Three");
            a.SetString("Display", display[2] == '1' ? "True" : "");
            a.SaveFields();
            return d;
        }

        private static GenSegment SetUpSegmentSeparatorFragment(GenDataBase d, GenCardinality cardinality)
        {
            var profile = "`[TestData" + (cardinality == GenCardinality.AllDlm ? "/" : "\\") + ":`?TestData.Display:`TestData.Name``]`;, `]";
            var p = new GenCompactProfileParser(d.GenDataDef, "", profile);
            var g = (GenSegment) p.Body.Fragment[0];
            return g;
        }

        /// <summary>
        /// Verifies the expansion and generation of segments with empty items
        /// </summary>
        /// <param name="display">Which items are to display.</param>
        /// <param name="expected">The expected output.</param>
        /// <param name="cardinality">The segement cardinality being output.</param>
        protected static void VerifySegmentSeparator(string display, string expected, GenCardinality cardinality)
        {
            var d = SetUpSegmentSeparatorData(display);
            var g = SetUpSegmentSeparatorFragment(d, cardinality);
            g.GenObject = d.Root;
            Assert.AreEqual(expected, GenFragmentExpander.Expand(d.GenDataDef, ((GenFragment) g).GenObject, g.Fragment));
            var str = GenerateFragment(d, g);
            Assert.AreEqual(expected, str);
        }
    }
}