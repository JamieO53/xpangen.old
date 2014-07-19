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
    /// The compact profile parser
    /// </summary>
    public class GenCompactProfileParser : GenProfileFragment
    {
        public readonly CharSet ParameterSeparator;
        private CompactProfileScanner Scan { get; set; }

        public GenCompactProfileParser(GenData genData, string filePath, string textToScan)
            : this(
                genData,
                string.IsNullOrEmpty(filePath)
                    ? new CompactProfileScanner(textToScan)
                    : new CompactProfileScanner(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                )
        {
        }

        private GenCompactProfileParser(GenData genData, CompactProfileScanner scan) : base(genData.GenDataDef)
        {
            
            Scan = scan;
            try
            {
                ScanBody(ClassId, Body, this);
            }
            finally
            {
                Scan.Dispose();
            }
            ParameterSeparator = new CharSet(" " + Scan.Delimiter);
        }

        private void ScanBody(int classId, GenSegBody body, GenContainerFragmentBase parentSegment)
        {
            var saveClassId = GenDataDef.CurrentClassId;
            GenDataDef.CurrentClassId = classId;
            var s = Scan.ScanText();
            GenTextBlock textBlock = null;
            if (s.Length > 0)
            {
                textBlock = new GenTextBlock(GenDataDef, parentSegment);
                textBlock.Body.Add(new GenTextFragment(GenDataDef, parentSegment) { Text = s });
            }
            if (!Scan.Eof)
            {
                var t = Scan.ScanTokenType();
                while (!Scan.Eof && t != TokenType.Close && t != TokenType.Unknown)
                {
                    if (t != TokenType.Name && textBlock != null)
                    {
                        body.Add(textBlock);
                        textBlock = null;
                    }
                    var frag = ScanFragment(classId, ref t, out s, parentSegment);
                    if (t == TokenType.Name)
                    {
                        if (textBlock == null) textBlock = new GenTextBlock(GenDataDef, parentSegment);
                        textBlock.Body.Add(frag);
                        if (s.Length > 0)
                            textBlock.Body.Add(new GenTextFragment(GenDataDef, parentSegment) { Text = s });
                    }
                    else
                    {
                        body.Add(frag);
                        if (s.Length > 0)
                        {
                            textBlock = new GenTextBlock(GenDataDef, parentSegment);
                            textBlock.Body.Add(new GenTextFragment(GenDataDef, parentSegment) { Text = s });
                        }
                    }
                    t = Scan.ScanTokenType();
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
                if (textBlock != null)
                    body.Add(textBlock);
            }
            else
                if (classId != 0)
                    throw new Exception("<<<<<Missing Segment end bracket>>>>>");
                else if (textBlock != null)
                    body.Add(textBlock);
            GenDataDef.CurrentClassId = saveClassId;
        }

        private GenFragment ScanFragment(int classId, ref TokenType nextToken, out string s, GenContainerFragmentBase parentSegment)
        {
            GenFragment frag;
            s = "";
            try
            {
                switch (nextToken)
                {
                    case TokenType.Null:
                        frag = new GenNullFragment(GenDataDef, parentSegment);
                        break;
                    case TokenType.Segment:
                        s = Scan.ScanSegmentClass();

                        var seg =
                            ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary.ParseSegmentHeading(
                                GenDataDef, s, parentSegment);
                        frag = seg;
                        ScanBody(seg.ClassId, seg.Body, parentSegment);
                        break;
                    case TokenType.Block:
                        frag = ScanBlock(classId, parentSegment);
                        break;
                    case TokenType.Lookup:
                        s = Scan.ScanLookup();
                        var lookup = new GenLookup(GenDataDef, s, parentSegment);
                        frag = lookup;
                        ScanBody(lookup.ClassId, lookup.Body, parentSegment);
                        break;
                    case TokenType.Condition:
                        s = Scan.ScanCondition();
                        var cond = new GenCondition(GenDataDef, parentSegment);
                        ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary.ParseCondition(cond,
                                                                                                             GenDataDef,
                                                                                                             s);
                        frag = cond;
                        ScanBody(classId, cond.Body, parentSegment);
                        break;
                    case TokenType.Function:
                        s = Scan.ScanFunctionName();
                        var func = new GenFunction(GenDataDef, parentSegment) { FunctionName = s };
                        frag = func;
                        ScanBlockParams(func.Body, parentSegment);
                        break;
                    case TokenType.NoMatch:
                        s = Scan.ScanLookup();
                        var noMatch = new GenLookup(GenDataDef, s, parentSegment) { NoMatch = true };
                        frag = noMatch;
                        ScanBody(noMatch.ClassId, noMatch.Body, parentSegment);
                        break;
                    case TokenType.Name:
                        frag = new GenPlaceholderFragment(GenDataDef, parentSegment) { Id = GenDataDef.GetId(Scan.ScanName()) };
                        if (Scan.CheckChar(Scan.Delimiter))
                            Scan.SkipChar();
                        break;
                    default:
                        frag = new GenNullFragment(GenDataDef, parentSegment);
                        break;
                }
                s = Scan.ScanText();
            }
            catch (Exception e)
            {
                frag = new GenBlock(GenDataDef, parentSegment);
                ((GenBlock)frag).Body.Add(new GenTextFragment(GenDataDef, parentSegment) { Text = e.Message });
            }
            return frag;
        }

        private void ScanBlockParams(GenSegBody body, GenContainerFragmentBase parentSegment)
        {
            Scan.ScanWhile(ScanReader.WhiteSpace);

            if (Scan.CheckChar(Scan.Delimiter))
                Scan.SkipChar();
            var t = Scan.ScanTokenType();

            while (t != TokenType.Close)
            {
                string s;
                int i;
                if (t != TokenType.Unknown && t != TokenType.Name)
                {
                    // Parameter starts with a delimiter
                    if (t != TokenType.Close && t != TokenType.Unknown)
                    {
                        // Scan contained block
                        var frag = ScanFragment(GenDataDef.CurrentClassId, ref t, out s, parentSegment);
                        body.Add(frag);

                        // Skip blank parameter separators
                        s = s.TrimStart();
                        if (s != "")
                            body.Add(new GenTextFragment(GenDataDef, parentSegment) { Text = s });
                        t = Scan.ScanTokenType();
                    }
                }
                else
                {
                    // Parameter starts without a delimiter
                    var block = new GenBlock(GenDataDef, parentSegment);

                    s = Scan.CheckChar('\'') ? Scan.ScanQuotedString() : Scan.ScanUntil(ParameterSeparator);

                    while (Scan.CheckChar(' '))
                        Scan.SkipChar();

                    // Scan for Text and Placeholders
                    i = s.IndexOf(Scan.Delimiter);
                    while (i != -1)
                    {
                        var w = s.Substring(0, i - 1);  // Text up to first delimiter
                        s = s.Substring(i + 1);         // Text after first delimeter
                        if (s != "")
                        {
                            // Some text after the first delimiter
                            i = s.IndexOf(Scan.Delimiter); // Position of next delimiter
                            if (i != -1)
                            {
                                if (w != "")
                                    // Add Text up to first delimiter
                                    block.Body.Add(new GenTextFragment(GenDataDef, parentSegment) {Text = w});
                                w = s.Substring(0, i - 1); // Text between initial two delimiters
                                block.Body.Add(new GenPlaceholderFragment(GenDataDef, parentSegment) { Id = GenDataDef.GetId(w) });

                                s = s.Substring(i + 1);
                                i = s.IndexOf(Scan.Delimiter);
                            }
                            else
                            {
                                // No matching delimiter: output delimiter with text
                                block.Body.Add(new GenTextFragment(GenDataDef, parentSegment) { Text = w + Scan.Delimiter + s });
                                s = "";
                            }
                        }
                        else
                        {
                            // No text after initial delimiter: output delimiter with text
                            block.Body.Add(new GenTextFragment(GenDataDef, parentSegment) { Text = w + Scan.Delimiter });
                            i = -1;
                        }
                    }

                    if (s != "" || block.Body.Count == 0)
                        // Text without placeholders
                        block.Body.Add(new GenTextFragment(GenDataDef, parentSegment) { Text = s });

                    body.Add(block);

                    if (Scan.CheckChar(Scan.Delimiter))
                        Scan.SkipChar();

                    t = Scan.ScanTokenType();
                }
            }

            if (t == TokenType.Close)
                Scan.SkipChar();

        }

        private GenBlock ScanBlock(int classId, GenContainerFragmentBase parentSegment)
        {
            var frag = new GenBlock(GenDataDef, parentSegment);
            if (Scan.CheckChar('{'))
                Scan.SkipChar();
            ScanBody(classId, frag.Body, parentSegment);
            return frag;
        }
    }
}