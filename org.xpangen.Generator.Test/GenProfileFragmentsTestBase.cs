// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;

namespace org.xpangen.Generator.Test
{
    public class GenProfileFragmentsTestBase : GenDataTestsBase
    {
        //protected static readonly ProfileFragmentSyntaxDictionary CompactSyntaxDitionary = new CompactProfileFragmentSyntaxDictionary();

        protected static GenData SetUpData()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            d.CreateObject("", "Class").Attributes[0] = "Class";
            d.CreateObject("Class", "SubClass").Attributes[0] = "SubClass";
            d.CreateObject("Class", "Property").Attributes[0] = "Property";
            d.CreateObject("Class", "Property").Attributes[0] = "Property2";
            return d;
        }

        protected static GenData SetUpComparisonData()
        {
            var f = GenDataDef.CreateMinimal();
            f.Classes[PropertyClassId].Properties.Add("NameLT");
            f.Classes[PropertyClassId].Properties.Add("NameEQ");
            f.Classes[PropertyClassId].Properties.Add("NameGT");
            f.Classes[PropertyClassId].Properties.Add("NameBlank");

            var d = new GenData(f);
            SetUpData(d);

            d.First(ClassClassId);
            Assert.AreEqual("Class", d.Context[ClassClassId].GenObject.Attributes[0]);
            var a = new GenAttributes(f) { GenObject = d.Context[ClassClassId].GenObject };
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
            f.Classes[PropertyClassId].Properties.Add("Number");

            var d = new GenData(f);
            var a = new GenAttributes(f);
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

            const string r = "Condition holds";
            var exp = expected ? r : "";

            var g = new GenCondition(genDataDef, null);
            ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary.ParseCondition(g, genDataDef, condIn);

            var t = new GenTextFragment(genDataDef, null) {Text = r};
            g.Body.Add(t);
            VerifyFragment(genData, g, "GenCondition", FragmentType.Condition, profileLabel,
                           String.Format("`?{0}:{1}`]", condOut, r), exp, false, -1);
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

        protected static void VerifyFragment(GenData genData, GenFragment fragment, string expectedClass, FragmentType expectedType, string profileLabel, string profileText, string expected, bool isText, int parentClassId)
        {
            Assert.AreEqual(expectedClass, fragment.GetType().Name, "Fragment Class");
            Assert.AreEqual(expectedType, fragment.FragmentType, "Fragment Type");
            Assert.AreEqual(isText, fragment.IsTextFragment, "Is text fragment?");
            Assert.AreEqual(expected, fragment.Expand(genData), "Expanded fragment");
            Assert.AreEqual(profileLabel, fragment.ProfileLabel(), "Profile label");
            Assert.AreEqual(profileText, fragment.ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary), "Profile text");
            if (parentClassId >= 0)
                Assert.AreEqual(parentClassId, fragment.ParentSegement.ClassId, "Parent Class ID");
            var str = GenerateFragment(genData, fragment);
            Assert.AreEqual(expected, str);
        }

        protected static string GenerateFragment(GenData genData, GenFragment fragment)
        {
            string str;
            var s = new MemoryStream(100000);
            var w = new GenWriter(s);
            try
            {
                fragment.Generate(null, genData, w);
                s = (MemoryStream)w.Stream;
                w.Flush();
                s.Seek(0, SeekOrigin.Begin);
                var r = new StreamReader(s);
                str = r.ReadToEnd();
            }
            finally
            {
                s.Dispose();
            }
            return str;
        }

        protected static void ProcessSegment(GenData genData, string cardinalityText, GenCardinality genCardinality, string expected)
        {
            var root = new GenSegment(genData.GenDataDef, "", GenCardinality.All, null);
            var fa = new List<GenFragment>
                         {
                             new GenPlaceholderFragment(genData.GenDataDef, root) {Id = genData.GenDataDef.GetId("Property.Name")},
                             new GenTextFragment(genData.GenDataDef, root) {Text = ","}
                         };

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
            var g = new GenSegment(genData.GenDataDef, "Property", cardinality, root);
            root.Body.Add(g);
            Assert.AreEqual(genCardinality, g.GenCardinality);
            Assert.AreEqual("Property", g.Definition.Name);//genData.GenDataDef.Classes[g.ClassId].Name);
            Assert.AreSame(root, g.ParentSegement);
            foreach (var t in fa)
                g.Body.Add(t);
            foreach (var t in fa)
            {
                Assert.AreSame(g, t.ParentSegement);
            }
            VerifyFragment(genData, g, "GenSegment", FragmentType.Segment, "Property",
                           "`[Property" + dictionary.GenCardinalityText[(int)g.GenCardinality] + ":`Property.Name`,`]", expected, false, -1);
        }

        protected static void ExecuteFunction(GenData genData, string functionName, string variableName, string variableValue, string expected)
        {
            var g = new GenFunction(genData.GenDataDef){FunctionName = functionName};
            var b = SetFunctionParameters(genData, g, variableName, variableValue);
            VerifyFragment(genData, g, "GenFunction", FragmentType.Function, functionName,
                           "`@" + functionName + ':' + b + "`]", expected, false, -1);
        }

        private static string SetFunctionParameters(GenData genData, GenFunction genFunction, string variableName, string variableValue)
        {
            if (variableName == "")
                return "";
            
            var p0 = new GenTextFragment(genData.GenDataDef, genFunction) {Text = variableName};
            genFunction.Body.Add(p0);
            if (variableValue == "")
                return variableName;
            
            var p1 = new GenTextFragment(genData.GenDataDef, genFunction) {Text = variableValue};
            genFunction.Body.Add(p1);
            return variableName + " " + variableValue;
        }

        protected void CheckFunctionVarValue(GenData genData, string expected)
        {
            ExecuteFunction(genData, "Get", "Var", "", expected);
        }

        public static GenData SetUpSegmentSeparatorData(string display)
        {
            var fm = GenDataDef.CreateMinimal();
            var am = new GenAttributes(fm);
            var dm = new GenData(fm);
            am.GenObject = dm.Context[ClassClassId].CreateObject();
            am.SetString("Name", "TestData");
            am.SaveFields();
            dm.First(ClassClassId);
            am.GenObject = dm.Context[PropertyClassId].CreateObject();
            am.SetString("Name", "Name");
            am.SaveFields();
            am.GenObject = dm.Context[PropertyClassId].CreateObject();
            am.SetString("Name", "Display");
            am.SaveFields();
            var f = dm.AsDef();
            var a = new GenAttributes(f);
            var d = new GenData(f);
            a.GenObject = d.Context[1].CreateObject();
            a.SetString("Name", "One");
            a.SetString("Display", display[0] == '1' ? "True" : "");
            a.SaveFields();
            a.GenObject = d.Context[1].CreateObject();
            a.SetString("Name", "Two");
            a.SetString("Display", display[1] == '1' ? "True" : "");
            a.SaveFields();
            a.GenObject = d.Context[1].CreateObject();
            a.SetString("Name", "Three");
            a.SetString("Display", display[2] == '1' ? "True" : "");
            a.SaveFields();
            return d;
        }

        protected static GenSegment SetUpSegmentSeparatorFragment(GenDataDef f, GenCardinality cardinality, GenContainerFragmentBase parentSegment)
        {
            var g = new GenSegment(f, "TestData", cardinality, parentSegment);
            var cond = new GenCondition(f, null) {GenComparison = GenComparison.Exists, Var1 = f.GetId("TestData.Display")};
            g.Body.Add(cond);
            cond.Body.Add(new GenPlaceholderFragment(f, parentSegment) {Id = f.GetId("TestData.Name")});
            g.Body.Add(new GenTextFragment(f, parentSegment) {Text = ", "});
            return g;
        }

        /// <summary>
        /// Verifies the expansion and generation of segments with empty items
        /// </summary>
        /// <param name="display">Which items are to display.</param>
        /// <param name="expected">The expected output.</param>
        /// <param name="cardinality">The segement cardinality being output.</param>
        /// <param name="parentSegment"></param>
        protected static void VerifySegmentSeparator(string display, string expected, GenCardinality cardinality, GenContainerFragmentBase parentSegment)
        {
            var d = SetUpSegmentSeparatorData(display);
            var g = SetUpSegmentSeparatorFragment(d.GenDataDef, cardinality, parentSegment);
            Assert.AreEqual(expected, g.Expand(d));
            var str = GenerateFragment(d, g);
            Assert.AreEqual(expected, str);
        }
    }
}