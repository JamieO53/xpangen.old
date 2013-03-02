// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using System.Text;
using org.xpangen.Generator.Scanner;

namespace org.xpangen.Generator.Profile.Scanner
{
    /// <summary>
    /// Scanner for the compact profile syntax.
    /// </summary>
    public class CompactProfileScanner: ScanReader
    {
        private static readonly CharSet ConditionStarter = new CharSet("[{%&?@]");
        // ToDo: Can this be done by matching ? only?
        private static readonly CharSet LookupStarter = new CharSet("[{%&?@]");
        // ToDo: Can this be done by matching % or & only?
        private static readonly CharSet SegmentStarter = new CharSet("[{%&?@]");
        // ToDo: Can this be done by matching [ only?

        /// <summary>
        /// Creates a new profile scanner for the given text.
        /// </summary>
        /// <param name="text">The text to be scanned.</param>
        public CompactProfileScanner(string text) : base(text)
        {
            Delimiter = '`';
        }

        /// <summary>
        /// Creates a new profile scanner for streamed text.
        /// </summary>
        /// <param name="stream">The text stream to be scanned.</param>
        public CompactProfileScanner(Stream stream) : base(stream)
        {
            Delimiter = '`';
        }

        public char Delimiter { get; set; }

        /// <summary>
        /// Identifies the next token type in the scan text.
        /// </summary>
        public TokenType ScanTokenType()
        {
            TokenType result;
            if (CheckChar('['))
                result = TokenType.Segment;
            else if (CheckChar('{'))
                result = TokenType.Block;
            else if (CheckChar('%'))
                result = TokenType.Lookup;
            else if (CheckChar('&'))
                result = TokenType.NoMatch;
            else if (CheckChar('?'))
                result = TokenType.Condition;
            else if (CheckChar('@'))
                result = TokenType.Function;
            else if (CheckChar(']'))
                result = TokenType.Close;
            else if (CheckChar(Delimiter))
                result = TokenType.Delimiter;
            else if (CheckCharSet(AlphaNumeric))
                result = TokenType.Name;
            else
                result = TokenType.Unknown;

            if (result == TokenType.Delimiter)
                SkipChar();
            return result;
        }

        /// <summary>
        /// Scan the condition comparison from the current position
        /// </summary>
        public string ScanCondition()
        {
            if (CheckCharSet(ConditionStarter))
                SkipChar();
            var s = ScanUntilChar(':');
            SkipChar();
            return s;
        }

        /// <summary>
        /// Scan the lookup comparison from the current position
        /// </summary>
        public string ScanLookup()
        {
            if (CheckCharSet(LookupStarter))
                SkipChar();
            var s = ScanUntilChar(':');
            SkipChar();
            return s;
        }

        /// <summary>
        /// Get the segement class name and cardinality from the current position
        /// </summary>
        public string ScanSegmentClass()
        {
            if (CheckCharSet(SegmentStarter))
                SkipChar();
            var s = ScanUntilChar(':');
            SkipChar();
            return s;
        }

        /// <summary>
        /// Scan a possibly qualified name from the current position
        /// </summary>
        public string ScanName()
        {
            var s = ScanWhile(Identifier);
            if (CheckChar('.'))
            {
                SkipChar();
                return s + "." + ScanWhile(Identifier);
            }
            return s;
        }

        /// <summary>
        /// Scan the Function Name
        /// </summary>
        /// <returns></returns>
        public string ScanFunctionName()
        {
            return ScanSegmentClass();
        }

        /// <summary>
        /// Scans to the next fragment character. The parser delimiter character is its own escape, and a pair is replaced by a singleton in the result.
        /// </summary>
        /// <returns>The scanned text.</returns>
        public string ScanText()
        {
            var s = new StringBuilder(ScanUntilChar(Delimiter));
            if (CheckChar(Delimiter)) SkipChar();

            while (CheckChar(Delimiter))
            {
                SkipChar();
                s.Append(Delimiter);
                s.Append(ScanUntilChar(Delimiter));
                SkipChar();
            }
            return s.ToString();
        }
    }
}
