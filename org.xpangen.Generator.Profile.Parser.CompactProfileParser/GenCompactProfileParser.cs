// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Scanner;
using org.xpangen.Generator.Scanner;

namespace org.xpangen.Generator.Profile.Parser.CompactProfileParser
{
    /// <summary>
    ///     The compact profile parser
    /// </summary>
    public class GenCompactProfileParser : GenProfileFragment
    {
        private readonly CharSet _parameterSeparator;
        private readonly CompactPrimaryBodyParser _compactPrimaryBodyParser;
        private readonly CompactSecondaryBodyParser _compactSecondaryBodyParser;
        internal CompactProfileScanner Scan { get; private set; }

        private CompactPrimaryBodyParser CompactPrimaryBodyParser
        {
            get { return _compactPrimaryBodyParser; }
        }

        private CompactSecondaryBodyParser CompactSecondaryBodyParser
        {
            get { return _compactSecondaryBodyParser; }
        }

        public GenCompactProfileParser(GenData genData, string filePath, string textToScan, char delimiter = '`')
            : this(genData.GenDataDef, filePath, textToScan, delimiter)
        {
            GenObject = genData.Root;
        }

        public GenCompactProfileParser(GenDataDef genDataDef, string filePath, string textToScan, char delimiter = '`')
            : this(
                genDataDef,
                string.IsNullOrEmpty(filePath)
                    ? new CompactProfileScanner(textToScan) {Delimiter = delimiter}
                    : new CompactProfileScanner(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                      { Delimiter = delimiter }
                )
        {
        }

        private GenCompactProfileParser(GenDataDef genDataDef, CompactProfileScanner scan) : base(genDataDef)
        {
            Scan = scan;
            try
            {
                _compactSecondaryBodyParser = new CompactSecondaryBodyParser(this);
                _compactPrimaryBodyParser = new CompactPrimaryBodyParser(this);
                ScanBody(ClassId, Body, this, this);
            }
            finally
            {
                Scan.Dispose();
            }
            _parameterSeparator = new CharSet(" " + Scan.Delimiter);
        }

        private void ScanBody(int classId, GenSegBody body, GenContainerFragmentBase parentSegment,
            GenContainerFragmentBase parentContainer)
        {
            var saveClassId = GenDataDef.CurrentClassId;
            GenDataDef.CurrentClassId = classId;
            if (!Scan.Eof)
            {
                TokenType t;
                CompactPrimaryBodyParser.ScanPartialBody(classId, body, parentSegment, parentContainer, out t);
                if (t == TokenType.Secondary)
                {
                    Scan.SkipChar();
                    CompactSecondaryBodyParser.ScanPartialBody(classId, body, parentSegment, parentContainer, out t);
                }

                if (t != TokenType.Close)
                {
                    if (Scan.Eof && classId != 0)
                        throw new Exception("<<<<Missing Segment end bracket>>>>");
                    if (!Scan.Eof)
                        throw new Exception("<<<<Unknown text in profile>>>>");
                }
                else
                {
                    if (Scan.CheckChar(']'))
                        Scan.SkipChar();
                }
            }
            else if (classId != 0)
                throw new Exception("<<<<<Missing Segment end bracket>>>>>");
            GenDataDef.CurrentClassId = saveClassId;
        }

        internal GenFragment ScanFragment(int classId, ref TokenType nextToken, out string s, 
            GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, 
            ref GenTextBlock textBlock, bool isPrimary)
        {
            GenFragment frag;
            s = "";
            try
            {
                switch (nextToken)
                {
                    case TokenType.Segment:
                        s = Scan.ScanSegmentClass();

                        var seg =
                            ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary.ParseSegmentHeading(
                                GenDataDef, s, parentSegment, parentContainer, isPrimary);
                        frag = seg;
                        ScanBody(seg.ClassId, seg.Body, seg, seg);
                        break;
                    case TokenType.Block:
                        frag = ScanBlock(classId, parentSegment, parentContainer, isPrimary);
                        break;
                    case TokenType.Lookup:
                        s = Scan.ScanLookup();
                        var lookup =
                            new GenLookup(new GenLookupParams(GenDataDef, parentSegment, parentContainer, s, isPrimary));
                        frag = lookup;
                        ScanBody(lookup.ClassId, lookup.Body, lookup, lookup);
                        break;
                    case TokenType.Condition:
                        s = Scan.ScanCondition();
                        var c =
                            ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary.ParseCondition(
                                GenDataDef, s);
                        var cond =
                            new GenCondition(new GenConditionParams(GenDataDef, parentSegment, parentContainer, c,
                                isPrimary));

                        frag = cond;
                        ScanBody(classId, cond.Body, parentSegment, cond);
                        break;
                    case TokenType.Function:
                        s = Scan.ScanFunctionName();
                        var func =
                            new GenFunction(new GenFragmentParams(GenDataDef, parentSegment, parentContainer,
                                FragmentType.Function, isPrimary)) {FunctionName = s};
                        frag = func;
                        ScanBlockParams(func.Body, parentSegment, func);
                        break;
                    case TokenType.Name:
                        AddText(parentSegment, parentContainer, ref textBlock, GenDataDef.GetId(Scan.ScanName()), GenDataDef, isPrimary);
                        frag = null;
                        if (Scan.CheckChar(Scan.Delimiter))
                            Scan.SkipChar();
                        break;
                    default:
                        throw new GeneratorException("Unknown token type: " + nextToken);
                }
                s = Scan.ScanText();
            }
            catch (Exception e)
            {
                frag = new GenBlock(new GenFragmentParams(GenDataDef, parentSegment, parentContainer));
                ((GenBlock) frag).Body.Add(
                    new GenTextFragment(new GenTextFragmentParams(GenDataDef, parentSegment, parentContainer, e.Message, isPrimary)));
            }
            return frag;
        }

        private void ScanBlockParams(GenSegBody body, GenContainerFragmentBase parentSegment,
            GenContainerFragmentBase parentContainer)
        {
            Scan.ScanWhile(ScanReader.WhiteSpace);

            if (Scan.CheckChar(Scan.Delimiter))
                Scan.SkipChar();
            var t = Scan.ScanTokenType();

            while (t != TokenType.Close)
            {
                string s;
                if (t != TokenType.Unknown && t != TokenType.Name)
                {
                    // Parameter starts with a delimiter
                    if (t != TokenType.Close && t != TokenType.Unknown)
                    {
                        // Scan contained block
                        GenTextBlock textBlock = null;
                        var frag = ScanFragment(GenDataDef.CurrentClassId, ref t, out s, parentSegment, parentContainer, ref textBlock, true);
                        body.Add(frag ?? textBlock);

                        // Skip blank parameter separators
                        s = s.TrimStart();
                        if (s != "")
                            body.Add(
                                new GenTextFragment(new GenTextFragmentParams(GenDataDef, parentSegment, parentContainer,
                                    s)));
                        t = Scan.ScanTokenType();
                    }
                }
                else
                {
                    // Parameter starts without a delimiter
                    var block = new GenBlock(new GenFragmentParams(GenDataDef, parentSegment, parentContainer));

                    s = Scan.CheckChar('\'') ? Scan.ScanQuotedString() : Scan.ScanUntil(_parameterSeparator);

                    while (Scan.CheckChar(' '))
                        Scan.SkipChar();

                    // Scan for Text and Placeholders
                    var i = s.IndexOf(Scan.Delimiter);
                    while (i != -1)
                    {
                        var w = s.Substring(0, i - 1); // Text up to first delimiter
                        s = s.Substring(i + 1); // Text after first delimeter
                        if (s != "")
                        {
                            // Some text after the first delimiter
                            i = s.IndexOf(Scan.Delimiter); // Position of next delimiter
                            if (i != -1)
                            {
                                if (w != "")
                                    // Add Text up to first delimiter
                                    block.Body.Add(
                                        new GenTextFragment(new GenTextFragmentParams(GenDataDef, parentSegment,
                                            parentContainer, w)));
                                w = s.Substring(0, i - 1); // Text between initial two delimiters
                                block.Body.Add(
                                    new GenPlaceholderFragment(new GenPlaceholderFragmentParams(GenDataDef,
                                        parentSegment, parentContainer, GenDataDef.GetId(w))));

                                s = s.Substring(i + 1);
                                i = s.IndexOf(Scan.Delimiter);
                            }
                            else
                            {
                                // No matching delimiter: output delimiter with text
                                block.Body.Add(
                                    new GenTextFragment(new GenTextFragmentParams(GenDataDef, parentSegment,
                                        parentContainer, w + Scan.Delimiter + s)));
                                s = "";
                            }
                        }
                        else
                        {
                            // No text after initial delimiter: output delimiter with text
                            block.Body.Add(
                                new GenTextFragment(new GenTextFragmentParams(GenDataDef, parentSegment, parentContainer,
                                    w + Scan.Delimiter)));
                            i = -1;
                        }
                    }

                    if (s != "" || block.Body.Count == 0)
                        // Text without placeholders
                        block.Body.Add(
                            new GenTextFragment(new GenTextFragmentParams(GenDataDef, parentSegment, block, s)));

                    body.Add(block);

                    if (Scan.CheckChar(Scan.Delimiter))
                        Scan.SkipChar();

                    t = Scan.ScanTokenType();
                }
            }

            if (t == TokenType.Close)
                Scan.SkipChar();
        }

        private GenBlock ScanBlock(int classId, GenContainerFragmentBase parentSegment,
            GenContainerFragmentBase parentContainer, bool isPrimary = true)
        {
            var frag = new GenBlock(new GenFragmentParams(GenDataDef, parentSegment, parentContainer, isPrimary));
            if (Scan.CheckChar('{'))
                Scan.SkipChar();
            ScanBody(classId, frag.Body, parentSegment, frag);
            return frag;
        }

        internal static void AddText(GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, 
            ref GenTextBlock textBlock, string s, GenDataDef genDataDef, bool isPrimary)
        {
            CheckTextBlock(parentSegment, parentContainer, ref textBlock, genDataDef, isPrimary);
            textBlock.Body.Add(
                new GenTextFragment(new GenTextFragmentParams(genDataDef, parentSegment, textBlock, s)));
        }

        private void AddText(GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, 
            ref GenTextBlock textBlock, GenDataId id, GenDataDef genDataDef, bool isPrimary)
        {
            CheckTextBlock(parentSegment, parentContainer, ref textBlock, genDataDef, isPrimary);
            textBlock.Body.Add(
                new GenPlaceholderFragment(new GenPlaceholderFragmentParams(GenDataDef, parentSegment, textBlock, id)));
        }

        private static void CheckTextBlock(GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer,
            ref GenTextBlock textBlock, GenDataDef genDataDef, bool isPrimary)
        {
            if (textBlock == null)
            {
                textBlock =
                    new GenTextBlock(new GenFragmentParams(genDataDef, parentSegment, parentContainer,
                        FragmentType.TextBlock, isPrimary));
                if (isPrimary)
                    parentContainer.Body.Add(textBlock);
                else
                    parentContainer.Body.AddSecondary(textBlock);
            }
        }
    }
}