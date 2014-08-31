// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using System.Text;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Scanner;

namespace org.xpangen.Generator.Parameter
{
    public class GenParameters : GenData
    {
        /// <summary>
        ///     Create a new generator parameters object from the given text. The generator data definition is derived from the text.
        /// </summary>
        /// <param name="text">The text data being loaded.</param>
        public GenParameters(string text) : this(ExtractDef(text), text)
        {
        }

        /// <summary>
        ///     Create a new generator parameters object from the stream text. The generator data definition is derived from the text.
        /// </summary>
        /// <param name="stream">The stream data being loaded.</param>
        public GenParameters(Stream stream) : this(ExtractDef(stream), stream)
        {
        }

        /// <summary>
        ///     Create a new generator parameters object from the given text.
        /// </summary>
        /// <param name="genDataDef">The definition of the generator data.</param>
        /// <param name="text">The text data being loaded.</param>
        public GenParameters(GenDataDef genDataDef, string text) : base(genDataDef)
        {
            Scan = new ParameterScanner(text);
            Parse();
        }

        /// <summary>
        ///     Create a new generrator parameters object from the stream text.
        /// </summary>
        /// <param name="genDataDef">The definition of the generator data.</param>
        /// <param name="stream">The stream data being loaded.</param>
        public GenParameters(GenDataDef genDataDef, Stream stream) : base(genDataDef)
        {
            Scan = new ParameterScanner(stream);
            Parse();
            First(1);
            GenDataBase.Changed = false;
        }

        /// <summary>
        ///     Save the generator data to the specified file name.
        /// </summary>
        /// <param name="genData">The generator data to be saved.</param>
        /// <param name="fileName">The file path to which the data is to be saved.</param>
        public static void SaveToFile(GenData genData, string fileName)
        {
            var profile = CreateProfile(genData.GenDataDef);
            using (var writer = new GenWriter(null) {FileName = fileName})
            {
                profile.GenObject = genData.Root;
                GenFragmentGenerator.Generate(genData, writer, ((GenFragment) profile).GenObject, profile.Fragment);
            }
        }

        public static GenProfileFragment CreateProfile(GenDataDef genDataDef)
        {
            var def = new StringBuilder();
            def.Append("Definition=");
            def.AppendLine(genDataDef.DefinitionName);
            var profile = new GenProfileFragment(new GenProfileParams(genDataDef));
            profile.Body.Add(new GenTextFragment(new GenTextFragmentParams(genDataDef, profile, profile, "")));

            ClassDefinition(genDataDef, 0, def);
            ClassProfile(genDataDef, 0, profile, profile, profile);
            ((GenTextFragment) profile.Body.Fragment[0]).Text = def + ".\r\n";
            return profile;
        }

        public static void ClassDefinition(GenDataDef genDataDef, int classId, StringBuilder def)
        {
            if (classId != 0)
            {
                def.Append("Class=" + genDataDef.Classes[classId].Name);
                if (genDataDef.Classes[classId].Inheritors.Count > 0)
                {
                    var sep = '[';
                    foreach (var inheritor in genDataDef.Classes[classId].Inheritors)
                    {
                        def.Append(sep);
                        def.Append(inheritor.Name);
                        sep = ',';
                    }
                    def.AppendLine("]");
                    if (genDataDef.Classes[classId].InstanceProperties.Count > 0)
                    {
                        if (genDataDef.Classes[classId].InstanceProperties.Count == 1)
                            def.AppendLine("Field=" + genDataDef.Classes[classId].InstanceProperties[0]);
                        else
                        {
                            def.Append("Field={" + genDataDef.Classes[classId].InstanceProperties[0]);
                            for (var i = 1; i < genDataDef.Classes[classId].InstanceProperties.Count; i++)
                                def.Append("," + genDataDef.Classes[classId].InstanceProperties[i]);
                            def.AppendLine("}");
                        }
                    }
                }
                else
                {
                    def.AppendLine();
                    var f = new StringBuilder();
                    var sep = "";
                    var defClass = genDataDef.Classes[classId];
                    for (var i = 0; i < defClass.InstanceProperties.Count; i++)
                    {
                        if (!defClass.IsInherited ||
                            defClass.Parent.InstanceProperties.IndexOf(defClass.InstanceProperties[i]) == -1)
                        {
                            f.Append(sep);
                            f.Append(defClass.InstanceProperties[i]);
                            sep = ",";
                        }
                    }

                    var fields = f.ToString();
                    if (fields != string.Empty)
                        if (!fields.Contains(","))
                            def.AppendLine("Field=" + fields);
                        else
                            def.AppendLine("Field={" + fields + "}");
                }

                if (genDataDef.Classes[classId].SubClasses.Count == 1)
                {
                    def.Append("SubClass=" + genDataDef.Classes[classId].SubClasses[0].SubClass.Name);
                    if (!String.IsNullOrEmpty(genDataDef.Classes[classId].SubClasses[0].SubClass.Reference))
                        def.AppendLine("[Reference='" + genDataDef.Classes[classId].SubClasses[0].SubClass.Reference +
                                       "']");
                    else
                        def.AppendLine();
                }
                else if (genDataDef.Classes[classId].SubClasses.Count > 1)
                {
                    def.Append("SubClass={" + genDataDef.Classes[classId].SubClasses[0].SubClass.Name);
                    if (!String.IsNullOrEmpty(genDataDef.Classes[classId].SubClasses[0].SubClass.Reference))
                        def.AppendLine("[Reference='" + genDataDef.Classes[classId].SubClasses[0].SubClass.Reference +
                                       "']");
                    for (var i = 1; i < genDataDef.Classes[classId].SubClasses.Count; i++)
                    {
                        def.Append("," + genDataDef.Classes[classId].SubClasses[i].SubClass.Name);
                        if (!String.IsNullOrEmpty(genDataDef.Classes[classId].SubClasses[0].SubClass.Reference))
                            def.AppendLine("[Reference='" + genDataDef.Classes[classId].SubClasses[0].SubClass.Reference +
                                           "']");
                    }
                    def.AppendLine("}");
                }
            }

            for (var i = 0; i < genDataDef.Classes[classId].Inheritors.Count; i++)
                ClassDefinition(genDataDef, genDataDef.Classes[classId].Inheritors[i].ClassId, def);
            
            for (var i = 0; i < genDataDef.Classes[classId].SubClasses.Count; i++)
                if (String.IsNullOrEmpty(genDataDef.Classes[classId].SubClasses[i].SubClass.Reference))
                    ClassDefinition(genDataDef, genDataDef.Classes[classId].SubClasses[i].SubClass.ClassId, def);
        }

        private static void ClassProfile(GenDataDef genDataDef, int classId, GenContainerFragmentBase profile,
            GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer)
        {
            GenSegment classProfile = null;
            if (classId != 0)
            {
                GenTextBlock textBlock = null;
                var sb = new StringBuilder();
                classProfile = new GenSegment(
                    new GenSegmentParams(genDataDef, parentSegment, parentContainer, genDataDef.Classes[classId].Name,
                        genDataDef.Classes[classId].IsInherited
                            ? GenCardinality.Inheritance
                            : GenCardinality.All));

                if (!genDataDef.Classes[classId].IsAbstract)
                    sb.Append(genDataDef.Classes[classId].Name);

                if (genDataDef.Classes[classId].Properties.Count > 0 && !genDataDef.Classes[classId].IsAbstract)
                {
                    var j = 0;
                    if (
                        String.Compare(genDataDef.Classes[classId].Properties[0], "Name",
                            StringComparison.OrdinalIgnoreCase) ==
                        0)
                    {
                        sb.Append("=");
                        AddText(genDataDef, ref textBlock, classProfile, sb);
                        AddText(genDataDef, ref textBlock, classProfile, genDataDef.GetId(genDataDef.Classes[classId].Name, "Name"));
                        j = 1;
                    }
                    if (genDataDef.Classes[classId].Properties.Count > j)
                    {
                        AddText(genDataDef, ref textBlock, classProfile, "[");
                        textBlock = null;
                        var sep = "";
                        for (var i = j; i < genDataDef.Classes[classId].Properties.Count; i++)
                        {
                            var condExists =
                                new GenCondition(new GenConditionParams(genDataDef, classProfile, classProfile,
                                    new ConditionParameters
                                    {
                                        GenComparison = GenComparison.Exists,
                                        Var1 = genDataDef.GetId(genDataDef.Classes[classId].Name,
                                                                genDataDef.Classes[classId].Properties[i])
                                    }));
                            condExists.Body.Add(
                                new GenTextFragment(new GenTextFragmentParams(genDataDef, parentSegment, condExists,
                                    sep + genDataDef.Classes[classId].Properties[i])));
                            var condNotTrue =
                                new GenCondition(new GenConditionParams(genDataDef, classProfile, condExists,
                                    new ConditionParameters
                                    {
                                        GenComparison = GenComparison.Ne,
                                        Var1 = genDataDef.GetId(genDataDef.Classes[classId].Name,
                                                                genDataDef.Classes[classId].Properties[i]),
                                        Lit = "True",
                                        UseLit = true
                                    }));
                            condNotTrue.Body.Add(
                                new GenTextFragment(new GenTextFragmentParams(genDataDef, parentSegment, condNotTrue,
                                    "=")));
                            var functionQuote =
                                new GenFunction(new GenFunctionParams(genDataDef, parentSegment, condNotTrue,
                                    "StringOrName"));
                            var param = new GenBlock(new GenFragmentParams(genDataDef, parentSegment, functionQuote));
                            param.Body.Add(
                                new GenPlaceholderFragment(new GenPlaceholderFragmentParams(genDataDef, parentSegment,
                                    param,
                                    genDataDef.GetId(genDataDef.Classes[classId].Name,
                                                     genDataDef.Classes[classId].Properties[i]))));
                            functionQuote.Body.Add(param);
                            condNotTrue.Body.Add(functionQuote);
                            condExists.Body.Add(condNotTrue);

                            classProfile.Body.Add(condExists);
                            sep = ",";
                        }
                        AddText(genDataDef, ref textBlock, classProfile, "]\r\n");
                    }
                    else
                        AddText(genDataDef, ref textBlock, classProfile, "\r\n");
                }

                profile.Body.Add(classProfile);
            }

            for (var i = 0; i < genDataDef.Classes[classId].Inheritors.Count; i++)
                ClassProfile(genDataDef, genDataDef.Classes[classId].Inheritors[i].ClassId, classProfile ?? profile,
                    classProfile, classProfile);

            if (!genDataDef.Classes[classId].IsAbstract)
                SubClassProfiles(genDataDef, classId, profile, parentSegment, classProfile ?? profile, classProfile);
            if (genDataDef.Classes[classId].IsInherited)
                SubClassProfiles(genDataDef, genDataDef.Classes[classId].Parent.ClassId, profile, classProfile,
                    classProfile ?? profile, classProfile);
        }

        private static void AddText(GenDataDef genDataDef, ref GenTextBlock textBlock, GenContainerFragmentBase classProfile, StringBuilder sb)
        {
            AddText(genDataDef, ref textBlock, classProfile, sb.ToString());
            sb.Clear();
        }

        private static void AddText(GenDataDef genDataDef, ref GenTextBlock textBlock, GenContainerFragmentBase classProfile, GenDataId id)
        {
            CheckTextBlock(genDataDef, ref textBlock, classProfile);
            textBlock.Body.Add(new GenPlaceholderFragment(new GenPlaceholderFragmentParams(genDataDef, classProfile, textBlock, id)));
        }

        private static void AddText(GenDataDef genDataDef, ref GenTextBlock textBlock, GenContainerFragmentBase classProfile, string s)
        {
            CheckTextBlock(genDataDef, ref textBlock, classProfile);
            textBlock.Body.Add(new GenTextFragment(new GenTextFragmentParams(genDataDef, classProfile, textBlock, s)));
        }

        private static void CheckTextBlock(GenDataDef genDataDef, ref GenTextBlock textBlock,
            GenContainerFragmentBase classProfile)
        {
            if (textBlock == null)
            {
                textBlock =
                    new GenTextBlock(new GenFragmentParams(genDataDef, classProfile, classProfile,
                        FragmentType.TextBlock));
                classProfile.Body.Add(textBlock);
            }

        }

        private static void SubClassProfiles(GenDataDef genDataDef, int classId,
            GenContainerFragmentBase profile, GenContainerFragmentBase parentSegment,
            GenContainerFragmentBase parentContainer, GenSegment classProfile)
        {
            for (var i = 0; i < genDataDef.Classes[classId].SubClasses.Count; i++)
                if (String.IsNullOrEmpty(genDataDef.Classes[classId].SubClasses[i].SubClass.Reference))
                    ClassProfile(genDataDef, genDataDef.Classes[classId].SubClasses[i].SubClass.ClassId,
                        classProfile ?? profile, parentSegment, parentContainer);
                else
                {
                    var refClass =
                        new GenSegment(new GenSegmentParams(genDataDef, parentSegment, parentContainer,
                            genDataDef.Classes[classId].SubClasses[i].SubClass.Name, GenCardinality.Reference));
                    (classProfile ?? profile).Body.Add(refClass);
                }
        }

        private ParameterScanner Scan { get; set; }

        /// <summary>
        ///     Extracts the generator data definition from the text.
        /// </summary>
        /// <param name="text">The text data being loaded.</param>
        /// <returns>The extracted generator data definition.</returns>
        public static GenDataDef ExtractDef(string text)
        {
            var scan = new ScanReader(text);
            return ExtractDef(scan);
        }

        /// <summary>
        ///     Extracts the generator data definition from the stream text.
        /// </summary>
        /// <param name="stream">The stream text data being loaded.</param>
        /// <returns>The extracted generator data definition.</returns>
        public static GenDataDef ExtractDef(Stream stream)
        {
            var scan = new ScanReader(stream);
            var f = ExtractDef(scan);
            stream.Seek(0, SeekOrigin.Begin);
            return f;
        }

        private static GenDataDef ExtractDef(ScanReader reader)
        {
            var f = new GenDataDef();
            var classId = -1;
            var className = "";
            var token = reader.ScanWhile(ScanReader.Identifier);
            while (token != "")
            {
                switch (token)
                {
                    case "Definition":
                        reader.ScanWhile(ScanReader.WhiteSpace);
                        if (reader.CheckChar('=')) reader.SkipChar();
                        reader.ScanWhile(ScanReader.WhiteSpace);
                        f.DefinitionName = reader.ScanWhile(ScanReader.QualifiedIdentifier);
                        break;
                    case "Class":
                        reader.ScanWhile(ScanReader.WhiteSpace);
                        if (reader.CheckChar('=')) reader.SkipChar();
                        reader.ScanWhile(ScanReader.WhiteSpace);
                        className = reader.ScanWhile(ScanReader.Identifier);
                        classId = f.AddClass(className);
                        if (f.Classes.Count == 2)
                            f.AddSubClass("", className);
                        reader.ScanWhile(ScanReader.WhiteSpace);
                        if (reader.CheckChar('['))
                        {
                            do
                            {
                                reader.SkipChar();
                                reader.ScanWhile(ScanReader.WhiteSpace);
                                var inheritorClassName = reader.ScanWhile(ScanReader.Identifier);
                                f.AddInheritor(className, inheritorClassName);
                                reader.ScanWhile(ScanReader.WhiteSpace);
                            } while (reader.CheckChar(','));
                            Assert(reader.CheckChar(']'), "Definition Error for class " + className + ": ] expected");
                            reader.SkipChar();
                        }
                        break;
                    case "Field":
                        reader.ScanWhile(ScanReader.WhiteSpace);
                        if (reader.CheckChar('=')) reader.SkipChar();
                        if (reader.CheckChar('{'))
                        {
                            do
                            {
                                reader.SkipChar();
                                reader.ScanWhile(ScanReader.WhiteSpace);
                                var field = reader.ScanWhile(ScanReader.Identifier);
                                f.Classes[classId].AddInstanceProperty(field);
                                reader.ScanWhile(ScanReader.WhiteSpace);
                            } while (reader.CheckChar(','));
                            Assert(reader.CheckChar('}'), "Definition Error for class " + className + " fields list: } expected");
                            reader.SkipChar();
                        }
                        else
                        {
                            reader.ScanWhile(ScanReader.WhiteSpace);
                            var field = reader.ScanWhile(ScanReader.Identifier);
                            f.Classes[classId].AddInstanceProperty(field);
                        }
                        break;
                    case "SubClass":
                        reader.ScanWhile(ScanReader.WhiteSpace);
                        if (reader.CheckChar('=')) reader.SkipChar();
                        if (reader.CheckChar('{'))
                        {
                            while (!reader.CheckChar('}'))
                            {
                                reader.SkipChar();
                                ParseDefSubClass(reader, f, className);
                            }
                            reader.SkipChar();
                        }
                        else
                        {
                            ParseDefSubClass(reader, f, className);
                            reader.ScanWhile(ScanReader.WhiteSpace);
                        }
                        break;
                }
                reader.ScanWhile(ScanReader.WhiteSpace);
                token = reader.ScanWhile(ScanReader.Identifier);
            }
            return f;
        }

        private static void ParseDefSubClass(ScanReader reader, GenDataDef f, string className)
        {
            reader.ScanWhile(ScanReader.WhiteSpace);
            var sub = reader.ScanWhile(ScanReader.AlphaNumeric);
            if (reader.CheckChar('['))
            {
                reader.SkipChar();
                reader.ScanWhile(ScanReader.WhiteSpace);
                var field = reader.ScanWhile(ScanReader.Identifier);
                Assert(field.Equals("Reference", StringComparison.InvariantCultureIgnoreCase), "Data definition reference expected: " + field);
                reader.ScanWhile(ScanReader.WhiteSpace);
                Assert(reader.CheckChar('='), "Data definition [Reference=definition] expected: " + field);
                reader.SkipChar();
                reader.ScanWhile(ScanReader.WhiteSpace);
                var value = reader.CheckChar('\'')
                                ? reader.ScanQuotedString()
                                : reader.ScanWhile(ScanReader.Identifier);
                f.AddSubClass(className, sub, value);
                reader.ScanWhile(ScanReader.WhiteSpace);
                Assert(reader.CheckChar(']'), "Data definition ] expected");
                reader.SkipChar();
            }
            else
                f.AddSubClass(className, sub);
            reader.ScanWhile(ScanReader.WhiteSpace);
        }

        private void Parse()
        {
            var s = Scan.ScanUntilChar('.');
            if (Scan.Eof)
                Scan.Rescan(s);
            else
                Scan.SkipChar();

            Scan.ScanObject();
            First(0);
            while (!Scan.Eof)
            {
                var recordType = Scan.RecordType;
                var subClassId = GetBaseClassId(recordType);
                var subClassIdx = GenDataDef.IndexOfSubClass(0, subClassId);
                var className = Scan.RecordType;
                if (subClassIdx != -1)
                    LoadSubClass(Context[0].GenObject, subClassId, subClassIdx);
                else
                    // Error: Invalid data type at the global level - skip it
                    while (!Scan.Eof && Scan.RecordType == className)
                        Scan.ScanObject();
            }
            Cache.Merge();
        }

        private int GetClassId(string recordType)
        {
            var classId = GenDataDef.Classes.IndexOf(recordType);
            Assert(classId != -1, "Unknown record type: " + recordType);
            return classId;
        }

        private int GetBaseClassId(string recordType)
        {
            var classId = GenDataDef.Classes.IndexOf(recordType);
            Assert(classId != -1, "Unknown record type: " + recordType);
            var baseClassId = classId;
            while (GenDataDef.Classes[baseClassId].IsInherited)
                baseClassId = GenDataDef.Classes[baseClassId].Parent.ClassId;
            return baseClassId;
        }

        private void LoadSubClass(GenObject parent, int baseSubClassId, int subClassIdx)
        {
            Assert(Scan.HasProgressed, "Parameter reader is in a loop");
            while (!Scan.Eof && baseSubClassId == GetBaseClassId(Scan.RecordType))
            {
                if (Scan.Attribute("Name") != "" || Scan.Attribute("Reference") == "")
                {
                    var parentSubClass = parent.SubClass[subClassIdx] as GenSubClass;
                    var classId = GetClassId(Scan.RecordType);
                    var child = new GenObject(parent, parentSubClass, classId);
                    var newClassId = GetClassId(Scan.RecordType);
                    for (var i = 0; i < GenDataDef.Classes[newClassId].Properties.Count; i++)
                    {
                        var s = Scan.Attribute(GenDataDef.Classes[newClassId].Properties[i]);
                        child.Attributes[i] = s;
                    }
                    parent.SubClass[subClassIdx].Add(child);
                    Scan.ScanObject();
                    if (!Scan.Eof && baseSubClassId != GetBaseClassId(Scan.RecordType))
                    {
                        int idx;
                        while (!Scan.Eof &&
                               (idx = GenDataDef.IndexOfSubClass(baseSubClassId, GetBaseClassId(Scan.RecordType))) != -1)
                            LoadSubClass(child, GetBaseClassId(Scan.RecordType), idx);
                    }
                }
                else
                {
                    var reference = Scan.Attribute("Reference");
                    parent.SubClass[subClassIdx].Reference = reference;
                    var referenceData = DataLoader.LoadData(reference);
                    var definition = referenceData.GenDataDef.DefinitionName;
                    GenDataDef.Cache.Internal(string.IsNullOrEmpty(definition) ? reference + "Def" : definition,
                                              referenceData.GenDataDef);
                    Cache.Internal(reference, referenceData);
                    Scan.ScanObject();
                }
            }
        }

        public static FileStream CreateStream(string filePath)
        {
            var path = filePath.Replace('/', '\\') + (Path.GetExtension(filePath) == "" ? ".dcb" : "");
            if (!File.Exists(path) && Path.GetDirectoryName(path) == "" && Directory.Exists("data"))
            {
                path = "data\\" + Path.GetFileName(path);
            }
            if (!File.Exists(path))
                throw new ArgumentException("The generator data file does not exist: " + path, "filePath");
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}