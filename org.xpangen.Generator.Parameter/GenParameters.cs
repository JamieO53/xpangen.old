// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
        /// Create a new generator parameters object from the given text. The generator data definition is derived from the text.
        /// </summary>
        /// <param name="text">The text data being loaded.</param>
        public GenParameters(string text) : this(ExtractDef(text), text)
        {
        }

        /// <summary>
        /// Create a new generator parameters object from the stream text. The generator data definition is derived from the text.
        /// </summary>
        /// <param name="stream">The stream data being loaded.</param>
        public GenParameters(Stream stream) : this(ExtractDef(stream), stream)
        {
        }

        /// <summary>
        /// Create a new generator parameters object from the given text.
        /// </summary>
        /// <param name="genDataDef">The definition of the generator data.</param>
        /// <param name="text">The text data being loaded.</param>
        public GenParameters(GenDataDef genDataDef, string text) : base(genDataDef)
        {
            Scan = new ParameterScanner(text);
            Parse();
        }

        /// <summary>
        /// Create a new generrator parameters object from the stream text.
        /// </summary>
        /// <param name="genDataDef">The definition of the generator data.</param>
        /// <param name="stream">The stream data being loaded.</param>
        public GenParameters(GenDataDef genDataDef, Stream stream) : base(genDataDef)
        {
            Scan = new ParameterScanner(stream);
            Parse();
        }

        /// <summary>
        /// Save the generator data to the specified file name.
        /// </summary>
        /// <param name="genData">The generator data to be saved.</param>
        /// <param name="fileName">The file path to which the data is to be saved.</param>
        public static void SaveToFile(GenData genData, string fileName)
        {
            var def = new StringBuilder();
            var profile = new GenSegment(genData.GenDataDef, "", GenCardinality.All, null);
            profile.Body.Add(new GenTextFragment(genData.GenDataDef, profile));

            ClassProfile(genData.GenDataDef, 0, def, profile, null);
            ((GenTextFragment) profile.Body.Fragment[0]).Text = def + ".\r\n";
            // ToDo: Use Generate to avoid expanding in memory
            //File.WriteAllText(fileName, profile.Expand(genData));
            //var stream = new FileStream(fileName, FileMode.Create);
            using (var writer = new GenWriter(null) {FileName = fileName})
            {
                profile.Generate(null, genData, writer);
            }
        }

        private static void ClassProfile(GenDataDef genDataDef, int classId, StringBuilder def, GenSegment profile, GenContainerFragmentBase parentSegment)
        {
            GenSegment classProfile = null;
            if (classId != 0)
            {
                def.AppendLine("Class=" + genDataDef.Classes[classId]);
                classProfile = new GenSegment(genDataDef, genDataDef.Classes[classId], GenCardinality.All, parentSegment);
                classProfile.Body.Add(new GenTextFragment(genDataDef, parentSegment) {Text = genDataDef.Classes[classId]});

                if (genDataDef.Properties[classId].Count > 0)
                {
                    if (genDataDef.Properties[classId].Count == 1)
                        def.AppendLine("Field=" + genDataDef.Properties[classId][0]);
                    else
                    {
                        def.Append("Field={" + genDataDef.Properties[classId][0]);
                        for (var i = 1; i < genDataDef.Properties[classId].Count; i++)
                            def.Append("," + genDataDef.Properties[classId][i]);
                        def.AppendLine("}");
                    }

                    var j = 0;
                    if (String.Compare(genDataDef.Properties[classId][0], "Name", StringComparison.OrdinalIgnoreCase) ==
                        0)
                    {
                        ((GenTextFragment) classProfile.Body.Fragment[0]).Text += "=";
                        classProfile.Body.Add(new GenPlaceholderFragment(genDataDef, parentSegment)
                                                  {
                                                      Id =
                                                          genDataDef.GetId(
                                                              genDataDef.Classes[classId] + ".Name")
                                                  });
                        j = 1;
                    }
                    if (genDataDef.Properties[classId].Count > j)
                    {
                        classProfile.Body.Add(new GenTextFragment(genDataDef, parentSegment) {Text = "["});
                        var sep = "";
                        for (var i = j; i < genDataDef.Properties[classId].Count; i++)
                        {
                            var condExists = new GenCondition(genDataDef, classProfile)
                                                 {
                                                     GenComparison = GenComparison.Exists,
                                                     Var1 = genDataDef.GetId(genDataDef.Classes[classId] + "." +
                                                                             genDataDef.Properties[classId][i])
                                                 };
                            condExists.Body.Add(new GenTextFragment(genDataDef, parentSegment)
                                                    {
                                                        Text =
                                                            sep +
                                                            genDataDef.Properties[classId][i]
                                                    });
                            var condNotTrue = new GenCondition(genDataDef, classProfile)
                                                  {
                                                      GenComparison = GenComparison.Ne,
                                                      Var1 = genDataDef.GetId(genDataDef.Classes[classId] + "." +
                                                                              genDataDef.Properties[classId][i]),
                                                      Lit = "True",
                                                      UseLit = true
                                                  };
                            var functionQuote = new GenFunction(genDataDef) {FunctionName = "StringOrName"};
                            var param = new GenBlock(genDataDef, parentSegment);
                            param.Body.Add(new GenPlaceholderFragment(genDataDef, parentSegment)
                                               {
                                                   Id =
                                                       genDataDef.GetId(
                                                           genDataDef.Classes[classId] + "." +
                                                           genDataDef.Properties[classId][i])
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

                if (genDataDef.SubClasses[classId].Count == 1)
                    def.AppendLine("SubClass=" + genDataDef.Classes[genDataDef.SubClasses[classId][0]]);
                else if (genDataDef.SubClasses[classId].Count > 1)
                {
                    def.Append("SubClass={" + genDataDef.Classes[genDataDef.SubClasses[classId][0]]);
                    for (var i = 1; i < genDataDef.SubClasses[classId].Count; i++)
                        def.Append("," + genDataDef.Classes[genDataDef.SubClasses[classId][i]]);
                    def.AppendLine("}");
                }

                profile.Body.Add(classProfile);
            }

            for (var i = 0; i < genDataDef.SubClasses[classId].Count; i++)
                ClassProfile(genDataDef, genDataDef.SubClasses[classId][i], def, classProfile ?? profile, parentSegment);
        }

        private ParameterScanner Scan { get; set; }

        /// <summary>
        /// Extracts the generator data definition from the text.
        /// </summary>
        /// <param name="text">The text data being loaded.</param>
        /// <returns>The extracted generator data definition.</returns>
        public static GenDataDef ExtractDef(string text)
        {
            var scan = new ScanReader(text);
            return ExtractDef(scan);
        }

        /// <summary>
        /// Extracts the generator data definition from the stream text.
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
            var token = reader.ScanWhile(ScanReader.AlphaNumeric);
            while (token != "")
            {
                switch (token)
                {
                    case "Class":
                        if (reader.CheckChar('=')) reader.SkipChar();
                        className = reader.ScanWhile(ScanReader.AlphaNumeric);
                        classId = f.AddClass(className);
                        if (f.Classes.Count == 2)
                            f.SubClasses[0].Add(classId);
                        break;
                    case "Field":
                        if (reader.CheckChar('=')) reader.SkipChar();
                        if (reader.CheckChar('{'))
                        {
                            while (!reader.CheckChar('}'))
                            {
                                reader.SkipChar();
                                var field = reader.ScanWhile(ScanReader.AlphaNumeric);
                                f.Properties[classId].Add(field);
                            }
                            reader.SkipChar();
                        }
                        else
                        {
                            var field = reader.ScanWhile(ScanReader.AlphaNumeric);
                            f.Properties[classId].Add(field);
                        }
                        break;
                    case "SubClass":
                        if (reader.CheckChar('=')) reader.SkipChar();
                        if (reader.CheckChar('{'))
                        {
                            while (!reader.CheckChar('}'))
                            {
                                reader.SkipChar();
                                var sub = reader.ScanWhile(ScanReader.AlphaNumeric);
                                f.AddSubClass(className, sub);
                            }
                            reader.SkipChar();
                        }
                        else
                        {
                            var sub = reader.ScanWhile(ScanReader.AlphaNumeric);
                            f.AddSubClass(className, sub);
                        }
                        break;
                }
                reader.ScanWhile(ScanReader.WhiteSpace);
                token = reader.ScanWhile(ScanReader.AlphaNumeric);
            }
            return f;
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
                var className = Scan.RecordType;
                var subClassId = GenDataDef.Classes.IndexOf(className);
                var subClassIdx = GenDataDef.IndexOfSubClass(0, subClassId);
                if (subClassIdx != -1)
                    LoadSubClass(Context[0].Context, className, subClassId, subClassIdx);
                else
                    // Error: Invalid data type at the global level - skip it
                    while (!Scan.Eof && className == Scan.RecordType)
                        Scan.ScanObject();
            }
        }

        private void LoadSubClass(GenObject parent, string className, int subClassId, int subClassIdx)
        {
            while (!Scan.Eof && className == Scan.RecordType)
            {
                var child = new GenObject(parent, parent.SubClass[subClassIdx], subClassId);
                for (var i = 0; i < GenDataDef.Properties[subClassId].Count; i++)
                {
                    var s = Scan.Attribute(GenDataDef.Properties[subClassId][i]);
                    child.Attributes[i] = s;
                }
                parent.SubClass[subClassIdx].Add(child);
                Scan.ScanObject();
                if (!Scan.Eof && className != Scan.RecordType)
                {
                    int subSubClassIdx;
                    do
                    {
                        var subClassName = Scan.RecordType;
                        var subSubClassId = GenDataDef.Classes.IndexOf(subClassName);
                        subSubClassIdx = GenDataDef.IndexOfSubClass(subClassId, subSubClassId);
                        if (subSubClassIdx != -1)
                            LoadSubClass(child, subClassName, subSubClassId, subSubClassIdx);
                    } while (!Scan.Eof && subSubClassIdx != -1);
                }
            }
        }
    }
}