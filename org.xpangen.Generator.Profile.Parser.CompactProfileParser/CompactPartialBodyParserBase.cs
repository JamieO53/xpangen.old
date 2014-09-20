// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Scanner;

namespace org.xpangen.Generator.Profile.Parser.CompactProfileParser
{
    public abstract class CompactPartialBodyParserBase
    {
        protected GenCompactProfileParser GenCompactProfileParser;
        protected bool IsPrimary;

        private CompactProfileScanner Scan
        {
            get { return GenCompactProfileParser.Scan; }
        }

        protected abstract void AddFragment(GenSegBody body, GenFragment frag);

        internal void ScanPartialBody(int classId, GenSegBody body, GenContainerFragmentBase parentContainer, out TokenType t)
        {
            GenTextBlock textBlock = null;
            var s = Scan.ScanText();
            if (Scan.Eof)
            {
                if (s.Length > 0)
                    GenCompactProfileParser.AddText(parentContainer, ref textBlock, s, GenDataDef, IsPrimary);
                t = TokenType.Unknown;
                return;
            }

            if (s.Length > 0)
                GenCompactProfileParser.AddText(parentContainer, ref textBlock, s, GenDataDef, IsPrimary);
            
            t = Scan.ScanTokenType();
            while (!Scan.Eof && t != TokenType.Close && t != TokenType.Secondary && t != TokenType.Unknown)
            {
                if (t != TokenType.Name && textBlock != null)
                    textBlock = null;
                var frag = GenCompactProfileParser.ScanFragment(classId, ref t, out s, parentContainer, ref textBlock, IsPrimary);
                if (t != TokenType.Name)
                    AddFragment(body, frag);
                if (s.Length > 0)
                    GenCompactProfileParser.AddText(parentContainer, ref textBlock, s, GenDataDef, IsPrimary);
                t = Scan.ScanTokenType();
            }
        }

        private GenDataDef GenDataDef
        {
            get { return GenCompactProfileParser.GenDataDef; }
        }
    }
}