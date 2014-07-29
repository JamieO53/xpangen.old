// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using org.xpangen.Generator.Profile.Profile;
using org.xpangen.Generator.Profile.Scanner;
using org.xpangen.Generator.Scanner;

namespace org.xpangen.Generator.Profile.Parser.CompactProfileParser
{
    /// <summary>
    /// The compact profile parser
    /// </summary>
    class GenCompactProfileParserClone : Profile.ProfileDefinition
    {
        public readonly CharSet ParameterSeparator;
        private CompactProfileScanner Scan { get; set; }

        public GenCompactProfileParserClone(string filePath, string textToScan)
            : this(string.IsNullOrEmpty(filePath)
                ? new CompactProfileScanner(textToScan)
                : new CompactProfileScanner(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                )
        {
        }

        private GenCompactProfileParserClone(CompactProfileScanner scan)
        {
            this.Setup();
            Scan = scan;
            try
            {
                ScanBody("", this.Profile().Body());
            }
            finally
            {
                Scan.Dispose();
            }
            ParameterSeparator = new CharSet(" " + Scan.Delimiter);
        }

        private void ScanBody(string className, FragmentBody container)
        {
            var s = Scan.ScanText();
            TextBlock textBlock = null;
            textBlock = ProcessText(container, s, ref textBlock);
            if (!Scan.Eof)
            {
                var t = Scan.ScanTokenType();
                while (!Scan.Eof && t != TokenType.Close && t != TokenType.Unknown)
                {
                    textBlock = CheckText(t, ref textBlock);
                    ScanFragment(className, ref t, ref textBlock, out s, container);
                    t = Scan.ScanTokenType();
                }
            }
        }

        private static TextBlock CheckText(TokenType t, ref TextBlock textBlock)
        {
            if (t != TokenType.Name && textBlock != null)
                textBlock = null;
            return textBlock;
        }

        private static TextBlock ProcessText(FragmentBody container, string s, ref TextBlock textBlock)
        {
            if (s.Length > 0)
            {
                if (textBlock == null)
                    textBlock = container.AddTextBlock();
                textBlock.Body().AddText(textBlock.Name(FragmentType.Text), s);
            }
            return textBlock;
        }

        private GenFragmentParams ScanFragment(string className, ref TokenType tokenType, ref TextBlock textBlock, out string s, FragmentBody container)
        {
            throw new NotImplementedException();
        }
    }
}
