// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using System.Text;

namespace org.xpangen.Generator.Scanner
{
    /// <summary>
    /// Scans text data for varuious character classes
    /// </summary>
    public class ScanReader : IScanReader
    {
        private readonly RescanReader _reader = new RescanReader();

        /// <summary>
        /// Is the scan at the end of the data?
        /// </summary>
        public bool Eof
        {
            get { return _reader.Eof; }
        }

        /// <summary>
        /// The current scan character
        /// 
        /// = ctrl-Z at the end of the data
        /// </summary>
        public Char Current
        {
            get { return _reader.Current; }
        }

        /// <summary>
        /// The encoding used if file stream is being scanned
        /// </summary>
        public Encoding Encoding
        {
            get { return _reader.Encoding; }
        }

        public static readonly CharSet Alpha = new CharSet("a-zA-Z");
        public static readonly CharSet AlphaNumeric = new CharSet("a-zA-Z0-9");
        public static readonly CharSet Numeric = new CharSet("0-9");
        public static readonly CharSet Identifier = new CharSet("a-zA-Z0-9_");
        public static readonly CharSet WhiteSpace = new CharSet(@" \r\n\t");
        public const char EofChar = (char) 0x1a; // Ctrl-Z

        /// <summary>
        /// Creates a new <see cref="ScanReader"/> for text input.
        /// </summary>
        /// <param name="text">The text to be scanned.</param>
        public ScanReader(string text)
        {
            Rescan(text);
        }

        /// <summary>
        /// Creates a new <see cref="ScanReader"/> for streamed input.
        /// </summary>
        /// <param name="stream">The stream text to be scanned.</param>
        public ScanReader(Stream stream)
        {
            Rescan(stream);
        }

        /// <summary>
        /// Checks the value of the current character.
        /// </summary>
        /// <param name="c">The expected value of the current character.</param>
        /// <returns>Does the current character have the specified value?</returns>
        public bool CheckChar(char c)
        {
            return Current == c;
        }

        /// <summary>
        /// Checks the value of the current character.
        /// </summary>
        /// <param name="cs">The expected values of the current character.</param>
        /// <returns>Does the current character have one of the specified values?</returns>
        public bool CheckCharSet(CharSet cs)
        {
            return cs.Match(Current);
        }

        /// <summary>
        /// Save the current scanning state, and scan the contents of the scan
        /// reader provided.
        /// 
        /// Scanning resumes from the saved state when the scan reaches the end
        /// of the provided data.
        /// </summary>
        /// <param name="stream">The stream text to be scanned before resuming the previous scan.</param>
        private void Rescan(Stream stream)
        {
            _reader.Rescan(new ScanStreamReader(stream));
        }

        /// <summary>
        /// Save the current scanning state, and scan the contents of the scan
        /// reader provided.
        /// 
        /// Scanning resumes from the saved state when the scan reaches the end
        /// of the provided data.
        /// </summary>
        /// <param name="text">The text to be scanned before resuming the previous scan.</param>
        public void Rescan(string text)
        {
            _reader.Rescan(new ScanTextReader(text));
        }

        /// <summary>
        /// Scans for the given keyword. The scan is case insensitive.
        /// </summary>
        /// <param name="keyword">The expected keyword.</param>
        /// <returns>The matched keywork from the scan text or an empty string if unmatched.</returns>
        public string ScanKeyword(string keyword)
        {
            var s = "";
            foreach (var t in keyword)
                if (CheckChar(Char.ToLower(t)) || CheckChar(Char.ToUpper(t)))
                {
                    s += Current;
                    SkipChar();
                }
                else
                {
                    Rescan(s);
                    return "";
                }
            return s;
        }

        /// <summary>
        /// Scan a quoted string with the convention that embedded quote characters are escaped by doubling them. The next scan character is the quote character.
        /// </summary>
        /// <returns>The contents of the quoted string, with unescaped characters.</returns>
        public string ScanQuotedString()
        {
            var cs = new CharSet(Current + "\\");
            var c = Current;
            SkipChar();
            var name = ScanUntil(cs);
            SkipChar();
            while (!Eof)
            {
                if (Current == c)
                    SkipChar();
                else
                {
                    switch (Current)
                    {
                        case 't':
                            name += '\t';
                            break;
                        case 'n':
                            name += '\n';
                            break;
                        case 'r':
                            name += '\r';
                            break;
                        case '\\':
                            name += '\\';
                            break;
                        default:
                            name += @"\" + Current.ToString();
                            break;
                    }
                    SkipChar();
                }
                name += ScanUntil(cs);
            }
            return name;
        }

        /// <summary>
        /// Scans for the text between "Open" and "Close" keywords. If either keyword is unmatched, then the scan text is left unchanged and an empty string is returned.
        /// </summary>
        /// <param name="open">The opening keyword.</param>
        /// <param name="close">The closing keyword.</param>
        /// <returns>The text between the matched opening and closing keywords, or an empty string if unmatched.</returns>
        public string ScanSimpleBlock(string open, string close)
        {
            var s = ScanKeyword(open);
            return s != "" ? s + ScanUntilKeyword(close) + ScanKeyword(close) : "";
        }

        /// <summary>
        /// Scans for text until one of the specified characters is matched. The next character is one of those specified, or the end of the text.
        /// </summary>
        /// <param name="cs">The characters being sought.</param>
        /// <returns>The matched text.</returns>
        public string ScanUntil(CharSet cs)
        {
            var result = new StringBuilder();
            while (!Eof && !cs.Match(Current))
            {
                result.Append(Current);
                SkipChar();
            }
            return result.ToString();
        }

        /// <summary>
        /// Scans for the given character, checking for the specified character or its upper or lower case form, if any.
        /// </summary>
        /// <param name="c">The character being sought.</param>
        /// <returns>The text up to the specified character.</returns>
        public string ScanUntilCharCaseInsensitive(char c)
        {
            var cu = Char.ToUpper(c);
            var cl = Char.ToLower(c);
            if (cu == cl)
                return ScanUntilChar(c);

            var result= new StringBuilder();
            while (!Eof && cu != Current && cl != Current)
            {
                result.Append(Current);
                SkipChar();
            }
            return result.ToString();
        }

        /// <summary>
        /// Scans for the given character. The case is significant.
        /// </summary>
        /// <param name="c">The character being sought.</param>
        /// <returns>The text up to the specified character.</returns>
        public string ScanUntilChar(char c)
        {
            var result = new StringBuilder();
            while (!Eof && c != Current)
            {
                result.Append(Current);
                SkipChar();
            }
            return result.ToString();
        }

        /// <summary>
        /// Scans the text until the keyword is encountered. The match is case insensitive. If it cannot be matched then the text is unchanged.
        /// </summary>
        /// <param name="keyword">The keyword being sought.</param>
        /// <returns>The matched text, or an empty string if the keyword could not be matched.</returns>
        public string ScanUntilKeyword(string keyword)
        {
            var starter = keyword[0];
            var result = new StringBuilder(ScanUntilCharCaseInsensitive(starter), 10000);
            var match = ScanKeyword(keyword);
            while (!Eof && match == "")
            {
                result.Append(Current);
                SkipChar();
                result.Append(ScanUntilCharCaseInsensitive(starter));
                match = ScanKeyword(keyword);
            }
            Rescan(match);
            return result.ToString();
        }

        /// <summary>
        /// Scans for text while any of the specified characters is matched. The next character is not one of those specified, or the end of the text.
        /// </summary>
        /// <param name="cs">The characters being sought.</param>
        /// <returns>The matched text.</returns>
        public string ScanWhile(CharSet cs)
        {
            var result = new StringBuilder();
            while (!Eof && cs.Match(Current))
            {
                result.Append(Current);
                SkipChar();
            }
            return result.ToString();
        }

        /// <summary>
        /// Skip to the next character being scanned
        /// 
        /// At Eof the current character is set to Ctrl-Z
        /// </summary>
        public void SkipChar()
        {
            _reader.SkipChar();
        }

        /// <summary>
        /// Dispose of uncontrolled data - IDisposable interface
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            _reader.Dispose();
        }
        
        ~ScanReader()
        {
            Dispose(false);
        }
    }
}