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
        protected static GenData SetUpData()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            var c = CreateGenObject(d, d.Root, "Class", "Class");
            CreateGenObject(d, c, "SubClass", "SubClass");
            CreateGenObject(d, c, "Property", "Property");
            CreateGenObject(d, c, "Property", "Property2");
            return d;
        }

        protected static GenData SetUpComparisonData()
        {
            var f = GenDataDef.CreateMinimal();
            f.AddClassInstanceProperty(PropertyClassId, "NameLT");
            f.AddClassInstanceProperty(PropertyClassId, "NameEQ");
            f.AddClassInstanceProperty(PropertyClassId, "NameGT");
            f.AddClassInstanceProperty(PropertyClassId, "NameBlank");

            var d = new GenData(f);
            SetUpData(d);

            d.First(ClassClassId);
            Assert.AreEqual("Class", d.Context[ClassClassId].GenObject.Attributes[0]);
            var a = new GenAttributes(f, ClassClassId) { GenObject = d.Context[ClassClassId].GenObject };
            a.SetString("NameLT", "Clasa");
            a.SetString("NameEQ", "Class");
            a.SetString("NameGT", "Clasz");
            a.SetString("NameBlank", "");
            a.SaveFields();

            d.Last(ClassClassId);
            //d.Prior(ClassClassId);
            Assert.AreEqual("Property", d.Context[ClassClassId].GenObject.Attributes[0]);
            d.First(PropertyClassId);
            a.GenObject = d.Context[PropertyClassId].GenObject;
            a.SetString("NameLT", "Nama");
            a.SetString("NameEQ", "Name");
            a.SetString("NameGT", "Namz");
            a.SetString("NameBlank", "");
            a.SaveFields();

            return d;
        }

        protected static GenData SetUpNumericComparisonData()
        {
            var f = GenDataDef.CreateMinimal();
            f.AddClassInstanceProperty(PropertyClassId, "Number");

            var d = new GenData(f);
            var a = new GenAttributes(f, PropertyClassId);
            SetUpData(d);

            d.First(PropertyClassId);
            a.GenObject = d.Context[PropertyClassId].GenObject;
            a.SetNumber("Number", 15);
            a.SaveFields();

            return d;
        }

        protected static void TestCondition(GenData genData, string condIn, string condOut, string profileLabel,
                                            bool expected)
        {
            var genDataDef = genData.GenDataDef;
            if (condOut == "") condOut = condIn;
            if (profileLabel == "") profileLabel = condIn;

            var root = new GenProfileFragment(new GenProfileParams(genDataDef));
            const string r = "Condition holds";
            var exp = expected ? r : "";

            var c = ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary.ParseCondition(genDataDef, condIn);
            var g = new GenCondition(new GenConditionParams(genDataDef, root, c));

            var t = new GenTextFragment(new GenTextFragmentParams(genDataDef, g, r));
            g.Body.Add(t);
            VerifyFragment(genData, g, "GenCondition", FragmentType.Condition, profileLabel,
                           String.Format("`?{0}:{1}`]", condOut, r), exp, false, null, g.Fragment.GenDataBase.GenDataDef);
        }

        protected static void TestIdentifierComparison(GenData genData, string comparison, bool expectedLt, bool expectedEq,
                                             bool expectedGt)
        {
            var condIn = "Property.Name" + comparison + "Property.Name";
            TestComparisonConditions(genData, expectedLt, expectedEq, expectedGt, condIn, "LT", "EQ", "GT");
        }

        protected static void TestNumericComparison(GenData genData, string comparison, bool expectedLt, bool expectedEq,
                                             bool expectedGt)
        {
            var condIn = "Property.Number" + comparison;
            TestComparisonConditions(genData, expectedLt, expectedEq, expectedGt, condIn, "1", "15", "1500");
        }

        protected static void TestComparison(GenData genData, string comparison, bool expectedLt, bool expectedEq,
                                             bool expectedGt)
        {
            var condIn = "Property.Name" + comparison;
            TestComparisonConditions(genData, expectedLt, expectedEq, expectedGt, condIn, "Nama", "Name", "Namz");
        }

        private static void TestComparisonConditions(GenData genData, bool expectedLt, bool expectedEq, bool expectedGt,
                                                     string condIn, string valueLt, string valueEq, string valueGt)
        {
            TestCondition(genData, condIn + valueLt, "", "", expectedLt); // Property.Name?Nama
            TestCondition(genData, condIn + valueEq, "", "", expectedEq); // Property.Name?Name
            TestCondition(genData, condIn + valueGt, "", "", expectedGt); // Property.Name?Namz
        }

        protected static void VerifyFragment(GenData genData, GenFragment genFragment, string expectedClass, FragmentType expectedType, string profileLabel, string profileText, string expected, bool isText, string parentClassName, GenDataDef profileDataDef)
        {
            Assert.AreEqual(expectedClass, genFragment.GetType().Name, "Fragment Class");
            Assert.AreEqual(expectedType, genFragment.FragmentType, "Fragment Type");
            Assert.AreEqual(isText, genFragment.IsTextFragment, "Is text fragment?");
            if (genFragment.GenObject == null) genFragment.GenObject = genData.Context[3].GenObject;
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

        protected static string GenerateFragment(GenData genData, GenFragment fragment)
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

        protected static void ExecuteFunction(GenData genData, string functionName, string variableName, string variableValue, string expected)
        {
            var r = new GenProfileFragment(new GenProfileParams(genData.GenDataDef));
            var g = new GenFunction(new GenFunctionParams(genData.GenDataDef, r, functionName));
            r.Body.Add(g);
            var b = SetFunctionParameters(genData, g, variableName, variableValue);
            VerifyFragment(genData, g, "GenFunction", FragmentType.Function, functionName,
                           "`@" + functionName + ':' + b + "`]", expected, false, null, r.Profile.GenDataBase.GenDataDef);
        }

        private static string SetFunctionParameters(GenData genData, GenFunction genFunction, string variableName, string variableValue)
        {
            if (variableName == "")
                return "";

            var p0 = new GenTextFragment(new GenTextFragmentParams(genData.GenDataDef, genFunction, variableName));
            genFunction.Body.Add(p0);
            if (variableValue == "")
                return variableName;

            var p1 = new GenTextFragment(new GenTextFragmentParams(genData.GenDataDef, genFunction, variableValue));
            genFunction.Body.Add(p1);
            return variableName + " " + variableValue;
        }

        protected void CheckFunctionVarValue(GenData genData, string expected)
        {
            ExecuteFunction(genData, "Get", "Var", "", expected);
        }

        private static GenData SetUpSegmentSeparatorData(string display)
        {
            var fm = GenDataDef.CreateMinimal();
            var dm = new GenData(fm);
            var td = CreateClass(dm, "TestData");
            dm.First(ClassClassId);
            CreateProperty(dm, "Name", td);
            CreateProperty(dm, "Display", td);
            var f = dm.AsDef();
            var a = new GenAttributes(f, 1);
            var d = new GenData(f);
            a.GenObject = CreateGenObject(d, d.Root, "TestData", "One");
            a.SetString("Name", "One");
            a.SetString("Display", display[0] == '1' ? "True" : "");
            a.SaveFields();
            a.GenObject = CreateGenObject(d, d.Root, "TestData", "Two");
            a.SetString("Name", "Two");
            a.SetString("Display", display[1] == '1' ? "True" : "");
            a.SaveFields();
            a.GenObject = CreateGenObject(d, d.Root, "TestData", "Three");
            a.SetString("Name", "Three");
            a.SetString("Display", display[2] == '1' ? "True" : "");
            a.SaveFields();
            return d;
        }

        private static GenSegment SetUpSegmentSeparatorFragment(GenData d, GenCardinality cardinality)
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
            g.GenObject = d.Root;//.SubClass[0][0];
            Assert.AreEqual(expected, GenFragmentExpander.Expand(d.GenDataDef, ((GenFragment) g).GenObject, g.Fragment));
            var str = GenerateFragment(d, g);
            Assert.AreEqual(expected, str);
        }
    }
}