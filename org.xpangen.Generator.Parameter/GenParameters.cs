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
                profile.Generate(null, genData, writer);
            }
        }

        public static GenProfileFragment CreateProfile(GenDataDef genDataDef)
        {
            var def = new StringBuilder();
            def.Append("Definition=");
            def.AppendLine(genDataDef.Definition);
            var profile = new GenProfileFragment(genDataDef);
            profile.Body.Add(new GenTextFragment(genDataDef, profile));

            ClassDefinition(genDataDef, 0, def);
            ClassProfile(genDataDef, 0, profile, profile);
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
                    if (genDataDef.Classes[classId].Properties.Count > 0)
                    {
                        if (genDataDef.Classes[classId].Properties.Count == 1)
                            def.AppendLine("Field=" + genDataDef.Classes[classId].Properties[0]);
                        else
                        {
                            def.Append("Field={" + genDataDef.Classes[classId].Properties[0]);
                            for (var i = 1; i < genDataDef.Classes[classId].Properties.Count; i++)
                                def.Append("," + genDataDef.Classes[classId].Properties[i]);
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
                    for (var i = 0; i < defClass.Properties.Count; i++)
                    {
                        if (!defClass.IsInherited ||
                            defClass.Parent.Properties.IndexOf(defClass.Properties[i]) == -1)
                        {
                            f.Append(sep);
                            f.Append(defClass.Properties[i]);
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

        private static void ClassProfile(GenDataDef genDataDef, int classId, GenContainerFragmentBase profile, GenContainerFragmentBase parentSegment)
        {
            GenSegment classProfile = null;
            if (classId != 0)
            {
                classProfile = new GenSegment(genDataDef, genDataDef.Classes[classId].Name,
                                              genDataDef.Classes[classId].IsInherited
                                                  ? GenCardinality.Inheritance
                                                  : GenCardinality.All, parentSegment);

                if (!genDataDef.Classes[classId].IsAbstract)
                    classProfile.Body.Add(new GenTextFragment(genDataDef, parentSegment)
                                              {
                                                  Text =
                                                      genDataDef.Classes[classId]
                                                      .Name
                                              });

                if (genDataDef.Classes[classId].Properties.Count > 0 && !genDataDef.Classes[classId].IsAbstract)
                {
                    var j = 0;
                    if (
                        String.Compare(genDataDef.Classes[classId].Properties[0], "Name",
                                       StringComparison.OrdinalIgnoreCase) ==
                        0)
                    {
                        ((GenTextFragment) classProfile.Body.Fragment[0]).Text += "=";
                        classProfile.Body.Add(new GenPlaceholderFragment(genDataDef, parentSegment)
                                                  {
                                                      Id =
                                                          genDataDef.GetId(
                                                              genDataDef.Classes[classId].Name + ".Name")
                                                  });
                        j = 1;
                    }
                    if (genDataDef.Classes[classId].Properties.Count > j)
                    {
                        classProfile.Body.Add(new GenTextFragment(genDataDef, parentSegment) {Text = "["});
                        var sep = "";
                        for (var i = j; i < genDataDef.Classes[classId].Properties.Count; i++)
                        {
                            var condExists = new GenCondition(genDataDef, classProfile)
                                                 {
                                                     GenComparison = GenComparison.Exists,
                                                     Var1 = genDataDef.GetId(genDataDef.Classes[classId].Name + "." +
                                                                             genDataDef.Classes[classId].Properties[i])
                                                 };
                            condExists.Body.Add(new GenTextFragment(genDataDef, parentSegment)
                                                    {
                                                        Text =
                                                            sep +
                                                            genDataDef.Classes[classId].Properties[i]
                                                    });
                            var condNotTrue = new GenCondition(genDataDef, classProfile)
                                                  {
                                                      GenComparison = GenComparison.Ne,
                                                      Var1 = genDataDef.GetId(genDataDef.Classes[classId].Name + "." +
                                                                              genDataDef.Classes[classId].Properties[i]),
                                                      Lit = "True",
                                                      UseLit = true
                                                  };
                            var functionQuote = new GenFunction(genDataDef, parentSegment)
                                                    {
                                                        FunctionName =
                                                            "StringOrName"
                                                    };
                            var param = new GenBlock(genDataDef, parentSegment);
                            param.Body.Add(new GenPlaceholderFragment(genDataDef, parentSegment)
                                               {
                                                   Id =
                                                       genDataDef.GetId(
                                                           genDataDef.Classes[classId].Name + "." +
                                                           genDataDef.Classes[classId].Properties[i])
                                               });
                            functionQuote.Body.Add(param);
                            condNotTrue.Body.Add(new GenTextFragment(genDataDef, parentSegment) {Text = "="});
                            condNotTrue.Body.Add(functionQuote);
                            condExists.Body.Add(condNotTrue);

                            classProfile.Body.Add(condExists);
                            sep = ",";
                        }
                        classProfile.Body.Add(new GenTextFragment(genDataDef, parentSegment) {Text = "]\r\n"});
                    }
                    else
                        classProfile.Body.Add(new GenTextFragment(genDataDef, parentSegment) {Text = "\r\n"});
                }

                profile.Body.Add(classProfile);
            }

            for (var i = 0; i < genDataDef.Classes[classId].Inheritors.Count; i++)
                ClassProfile(genDataDef, genDataDef.Classes[classId].Inheritors[i].ClassId, classProfile ?? profile,
                             parentSegment);

            if (!genDataDef.Classes[classId].IsAbstract)
                SubClassProfiles(genDataDef, classId, profile, parentSegment, classProfile);
            if (genDataDef.Classes[classId].IsInherited)
                SubClassProfiles(genDataDef, genDataDef.Classes[classId].Parent.ClassId, profile, parentSegment, classProfile);
        }

        private static void SubClassProfiles(GenDataDef genDataDef, int classId,
                                             GenContainerFragmentBase profile, GenContainerFragmentBase parentSegment,
                                             GenSegment classProfile)
        {
            for (var i = 0; i < genDataDef.Classes[classId].SubClasses.Count; i++)
                if (String.IsNullOrEmpty(genDataDef.Classes[classId].SubClasses[i].SubClass.Reference))
                    ClassProfile(genDataDef, genDataDef.Classes[classId].SubClasses[i].SubClass.ClassId,
                                 classProfile ?? profile, parentSegment);
                else
                {
                    var refClass = new GenSegment(genDataDef, genDataDef.Classes[classId].SubClasses[i].SubClass.Name,
                                                  GenCardinality.Reference, parentSegment);
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
                        f.Definition = reader.ScanWhile(ScanReader.QualifiedIdentifier);
                        break;
                    case "Class":
                        reader.ScanWhile(ScanReader.WhiteSpace);
                        if (reader.CheckChar('=')) reader.SkipChar();
                        reader.ScanWhile(ScanReader.WhiteSpace);
                        className = reader.ScanWhile(ScanReader.AlphaNumeric);
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
                                var inheritorClassName = reader.ScanWhile(ScanReader.AlphaNumeric);
                                f.AddInheritor(className, inheritorClassName);
                                reader.ScanWhile(ScanReader.WhiteSpace);
                            } while (reader.CheckChar(','));
                            if (!reader.CheckChar(']'))
                                throw new GeneratorException(
                                    "Definition Error for class " + className + ": ] expected", GenErrorType.Assertion);
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
                                var field = reader.ScanWhile(ScanReader.AlphaNumeric);
                                f.Classes[classId].Properties.Add(field);
                                reader.ScanWhile(ScanReader.WhiteSpace);
                            } while (reader.CheckChar(','));
                            if (!reader.CheckChar('}'))
                                throw new GeneratorException(
                                    "Definition Error for class " + className + " fields list: } expected", GenErrorType.Assertion);
                            reader.SkipChar();
                        }
                        else
                        {
                            reader.ScanWhile(ScanReader.WhiteSpace);
                            var field = reader.ScanWhile(ScanReader.AlphaNumeric);
                            f.Classes[classId].Properties.Add(field);
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
                token = reader.ScanWhile(ScanReader.AlphaNumeric);
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
                if (!field.Equals("Reference", StringComparison.InvariantCultureIgnoreCase))
                    throw new GeneratorException("Data definition reference expected: " + field, GenErrorType.Assertion);
                reader.ScanWhile(ScanReader.WhiteSpace);
                if (!reader.CheckChar('='))
                    throw new GeneratorException("Data definition [Reference=definition] expected: " + field, GenErrorType.Assertion);
                reader.SkipChar();
                reader.ScanWhile(ScanReader.WhiteSpace);
                var value = reader.CheckChar('\'')
                                ? reader.ScanQuotedString()
                                : reader.ScanWhile(ScanReader.Identifier);
                f.AddSubClass(className, sub, value);
                reader.ScanWhile(ScanReader.WhiteSpace);
                if (!reader.CheckChar(']'))
                    throw new GeneratorException("Data definition ] expected", GenErrorType.Assertion);
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
                var subClassId = GetClassId(recordType);
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
            if (classId == -1)
                throw new GeneratorException("Unknown record type: " + recordType, GenErrorType.Assertion);
            var c = GenDataDef.Classes[classId];
            return c.IsInherited ? c.Parent.ClassId : classId;
        }

        private void LoadSubClass(GenObject parent, int subClassId, int subClassIdx)
        {
            while (!Scan.Eof && subClassId == GetClassId(Scan.RecordType))
            {
                if (Scan.Attribute("Name") != "" || Scan.Attribute("Reference") == "")
                {
                    var child = new GenObject(parent, parent.SubClass[subClassIdx] as GenSubClass, subClassId);
                    for (var i = 0; i < GenDataDef.Classes[subClassId].Properties.Count; i++)
                    {
                        var s = Scan.Attribute(GenDataDef.Classes[subClassId].Properties[i]);
                        child.Attributes[i] = s;
                    }
                    parent.SubClass[subClassIdx].Add(child);
                    Scan.ScanObject();
                    if (!Scan.Eof && subClassId != GetClassId(Scan.RecordType))
                    {
                        int idx;
                        while (!Scan.Eof &&
                               (idx = GenDataDef.IndexOfSubClass(subClassId, GetClassId(Scan.RecordType))) != -1)
                            LoadSubClass(child, GetClassId(Scan.RecordType), idx);
                    }
                }
                else
                {
                    var reference = Scan.Attribute("Reference");
                    parent.SubClass[subClassIdx].Reference = reference;
                    var referenceData = DataLoader.LoadData(reference);
                    var definition = referenceData.GenDataDef.Definition;
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