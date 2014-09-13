// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Text;

namespace org.xpangen.Generator.Scanner
{
    /// <summary>
    /// Scan reader from a known string value
    /// </summary>
    public class ScanTextReader : IScanReader
    {
        /// <summary>
        /// The current scan character
        /// 
        /// = ctrl-Z at the end of the data
        /// </summary>
        public char Current { get; private set; }

        /// <summary>
        /// Is the scan at the end of the data?
        /// </summary>
        public bool Eof { get { return _i >= _text.Length; } }

        /// <summary>
        /// The encoding used if file stream is being scanned
        /// </summary>
        public Encoding Encoding { get; private set; }

        private string _text;
        private int _i = -1;
        
        /// <summary>
        /// Creates a scan reader from a known string value
        /// </summary>
        /// <param name="text"></param>
        public ScanTextReader(string text)
        {
            _text = text;
            Encoding = null;
            SkipChar();
        }

        /// <summary>
        /// Skip to the next character being scanned
        /// 
        /// At Eof the current character is set to Ctrl-Z
        /// </summary>
        public void SkipChar()
        {
            if (_i < _text.Length)
                _i++;
            Current = !Eof ? _text[_i] : ScanReader.EofChar;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            _text = null;
        }

        ~ScanTextReader()
        {
            Dispose(false);
        }

    }
}