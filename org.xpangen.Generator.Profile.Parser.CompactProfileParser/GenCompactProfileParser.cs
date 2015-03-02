// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Diagnostics.Contracts;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;
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

        private GenCompactProfileParser(GenDataDef genDataDef, CompactProfileScanner scan)
            : base(new GenProfileParams(genDataDef))
        {
            Scan = scan;
            try
            {
                _compactPrimaryBodyParser = new CompactPrimaryBodyParser(this);
                _compactSecondaryBodyParser = new CompactSecondaryBodyParser(this);
                ScanBody(ClassId, Body, this, Profile);
            }
            finally
            {
                Scan.Dispose();
            }
            _parameterSeparator = new CharSet(" " + Scan.Delimiter);
        }

        private void ScanBody(int classId, GenSegBody body, GenContainerFragmentBase parentContainer,
            ContainerFragment containerFragment)
        {
            var saveClassId = GenDataDef.CurrentClassId;
            GenDataDef.CurrentClassId = classId;
            if (!Scan.Eof)
            {
                TokenType t;
                CompactPrimaryBodyParser.ScanPartialBody(classId, body, parentContainer, containerFragment, out t);
                if (t == TokenType.Secondary)
                {
                    Scan.SkipChar();
                    CompactSecondaryBodyParser.ScanPartialBody(classId, body, parentContainer, containerFragment, out t);
                }

                if (t != TokenType.Close)
                {
                    if (Scan.Eof && classId != 0)
                        //throw new Exception("<<<<Missing Segment end bracket>>>>");
                        OutputText(parentContainer, true, "<<<<Missing Segment end bracket>>>>");
                    if (!Scan.Eof)
                        //throw new Exception("<<<<Unknown text in profile>>>>");
                        OutputText(parentContainer, true, "<<<<Unknown text in profile>>>>");
                }
                else
                {
                    if (Scan.CheckChar(']'))
                        Scan.SkipChar();
                }
            }
            else if (classId != 0)
                //throw new Exception("<<<<<Missing Segment end bracket>>>>>");
                OutputText(parentContainer, true, "<<<<Unknown text in profile>>>>");
            GenDataDef.CurrentClassId = saveClassId;
        }

        internal GenFragment ScanFragment(int classId, ref TokenType nextToken, out string s, GenContainerFragmentBase parentContainer, FragmentBody fragmentBody, ref GenTextBlock textBlock, bool isPrimary)
        {
            Contract.Ensures(Contract.OldValue(nextToken) == TokenType.Name ||
                             Contract.Result<GenFragment>() != null);
            var oldToken = nextToken;
            GenFragment frag = null;
            s = "";
            switch (oldToken)
            {
                case TokenType.Segment:
                    frag = ScanSegment(parentContainer, isPrimary);
                    break;
                case TokenType.Block:
                    frag = ScanBlock(classId, parentContainer, isPrimary);
                    break;
                case TokenType.Lookup:
                    frag = ScanLookup(parentContainer, isPrimary);
                    break;
                case TokenType.Condition:
                    frag = ScanCondition(classId, parentContainer, isPrimary);
                    break;
                case TokenType.Function:
                    frag = ScanFunction(parentContainer, isPrimary);
                    break;
                case TokenType.Name:
                    ScanPlaceholder(parentContainer, ref textBlock, isPrimary);
                    break;
                case TokenType.Annotation:
                    frag = ScanAnnotation(classId, parentContainer, isPrimary);
                    break;
                default:
                    OutputText(parentContainer, isPrimary, "Unknown token type: " + nextToken);
                    throw new GeneratorException("Unknown token type: " + nextToken + "; " + Scan.Buffer);
            }
            s = Scan.ScanText();
            return frag;
        }

        private void OutputText(GenContainerFragmentBase parentContainer,
            bool isPrimary, string text)
        {
            GenFragment frag = new GenBlock(new GenFragmentParams(GenDataDef, parentContainer));
            ((GenBlock) frag).Body.Add(
                new GenTextFragment(new GenTextFragmentParams(GenDataDef, parentContainer, text, isPrimary)));
        }

        private void ScanPlaceholder(GenContainerFragmentBase parentContainer,
            ref GenTextBlock textBlock, bool isPrimary)
        {
            AddText(parentContainer, ref textBlock, GenDataDef.GetId(Scan.ScanName()), GenDataDef, isPrimary);
            if (Scan.CheckChar(Scan.Delimiter))
                Scan.SkipChar();
        }

        private GenFragment ScanFunction(GenContainerFragmentBase parentContainer, bool isPrimary)
        {
            var s = Scan.ScanFunctionName();
            var func =
                new GenFunction(new GenFunctionParams(GenDataDef, parentContainer,
                    s, FragmentType.Function, isPrimary));
            GenFragment frag = func;
            ScanBlockParams(func.Body, func, func.Function, func.Function.Body());
            return frag;
        }

        private GenFragment ScanCondition(int classId, GenContainerFragmentBase parentContainer, bool isPrimary)
        {
            var s = Scan.ScanCondition();
            var c =
                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary.ParseCondition(
                    GenDataDef, s);
            var cond =
                new GenCondition(new GenConditionParams(GenDataDef, parentContainer, c, isPrimary));

            GenFragment frag = cond;
            ScanBody(classId, cond.Body, cond, cond.Condition);
            return frag;
        }

        private GenFragment ScanLookup(GenContainerFragmentBase parentContainer, bool isPrimary)
        {
            var s = Scan.ScanLookup();
            var lookup =
                new GenLookup(new GenLookupParams(GenDataDef, parentContainer, s, isPrimary));
            GenFragment frag = lookup;
            ScanBody(lookup.ClassId, lookup.Body, lookup, lookup.Lookup);
            return frag;
        }

        private GenFragment ScanSegment(GenContainerFragmentBase parentContainer, bool isPrimary)
        {
            var s = Scan.ScanSegmentClass();
            var seg =
                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary.ParseSegmentHeading(
                    GenDataDef, s, parentContainer, isPrimary);
            GenFragment frag = seg;
            ScanBody(seg.ClassId, seg.Body, seg, seg.Segment);
            return frag;
        }

        private void ScanBlockParams(GenSegBody body, GenContainerFragmentBase parentContainer, Function function, 
            FragmentBody fragmentBody)
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
                        var frag = ScanFragment(GenDataDef.CurrentClassId, ref t, out s, parentContainer, fragmentBody, 
                            ref textBlock, true);
                        body.Add(frag ?? textBlock);

                        // Skip blank parameter separators
                        s = s.TrimStart();
                        if (s != "")
                            body.Add(
                                new GenTextFragment(new GenTextFragmentParams(GenDataDef, parentContainer,
                                    s)));
                        t = Scan.ScanTokenType();
                    }
                }
                else
                {
                    // Parameter starts without a delimiter
                    var block = new GenBlock(new GenFragmentParams(GenDataDef, parentContainer));

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
                                        new GenTextFragment(new GenTextFragmentParams(GenDataDef,
                                            parentContainer, w)));
                                w = s.Substring(0, i - 1); // Text between initial two delimiters
                                block.Body.Add(
                                    new GenPlaceholderFragment(new GenPlaceholderFragmentParams(GenDataDef, parentContainer, GenDataDef.GetId(w))));

                                s = s.Substring(i + 1);
                                i = s.IndexOf(Scan.Delimiter);
                            }
                            else
                            {
                                // No matching delimiter: output delimiter with text
                                block.Body.Add(
                                    new GenTextFragment(new GenTextFragmentParams(GenDataDef,
                                        parentContainer, w + Scan.Delimiter + s)));
                                s = "";
                            }
                        }
                        else
                        {
                            // No text after initial delimiter: output delimiter with text
                            block.Body.Add(
                                new GenTextFragment(new GenTextFragmentParams(GenDataDef, parentContainer,
                                    w + Scan.Delimiter)));
                            i = -1;
                        }
                    }

                    if (s != "" || block.Body.Count == 0)
                        // Text without placeholders
                        block.Body.Add(
                            new GenTextFragment(new GenTextFragmentParams(GenDataDef, block, s)));

                    body.Add(block);

                    if (Scan.CheckChar(Scan.Delimiter))
                        Scan.SkipChar();

                    t = Scan.ScanTokenType();
                }
            }

            if (t == TokenType.Close)
                Scan.SkipChar();
        }

        private GenBlock ScanBlock(int classId, GenContainerFragmentBase parentContainer, bool isPrimary = true)
        {
            var frag = new GenBlock(new GenFragmentParams(GenDataDef, parentContainer, isPrimary));
            if (Scan.CheckChar('{'))
                Scan.SkipChar();
            ScanBody(classId, frag.Body, frag, frag.Block);
            return frag;
        }

        private GenAnnotation ScanAnnotation(int classId, GenContainerFragmentBase parentContainer, bool isPrimary = true)
        {
            var frag = new GenAnnotation(new GenFragmentParams(GenDataDef, parentContainer, isPrimary));
            if (Scan.CheckChar('-'))
                Scan.SkipChar();
            ScanBody(classId, frag.Body, frag, frag.Annotation);
            return frag;
        }

        internal static void AddText(GenContainerFragmentBase parentContainer, FragmentBody fragmentBody, ref GenTextBlock textBlock, string s, GenDataDef genDataDef, bool isPrimary)
        {
            CheckTextBlock(parentContainer, ref textBlock, genDataDef, isPrimary);
            textBlock.Body.Add(
                new GenTextFragment(new GenTextFragmentParams(genDataDef, textBlock, s)));
        }

        private void AddText(GenContainerFragmentBase parentContainer, 
            ref GenTextBlock textBlock, GenDataId id, GenDataDef genDataDef, bool isPrimary)
        {
            if (id.ClassId == -1 || id.PropertyId == -1)
                throw new GeneratorException(Scan.Buffer.ToString() + id, GenErrorType.ProfileError);
            CheckTextBlock(parentContainer, ref textBlock, genDataDef, isPrimary);
            textBlock.Body.Add(
                new GenPlaceholderFragment(new GenPlaceholderFragmentParams(GenDataDef, textBlock, id)));
        }

        private static void CheckTextBlock(GenContainerFragmentBase parentContainer,
            ref GenTextBlock textBlock, GenDataDef genDataDef, bool isPrimary)
        {
            if (textBlock == null)
            {
                textBlock =
                    new GenTextBlock(new GenFragmentParams(genDataDef, parentContainer,
                        FragmentType.TextBlock, isPrimary));
                if (isPrimary)
                    parentContainer.Body.Add(textBlock);
                else
                    parentContainer.Body.AddSecondary(textBlock);
            }
        }
    }
}