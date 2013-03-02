// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using System.Text;

namespace org.xpangen.Generator.Scanner
{
    /// <summary>
    /// Scan reader from a data stream
    /// 
    /// The data is buffered to achieve a balance between memory usage and speed
    /// </summary>
    public class ScanStreamReader : IScanReader
    {
        private StreamReader _streamReader;

        public ScanStreamReader(Stream stream)
        {
            _streamReader = new StreamReader(stream, true);
            Encoding = _streamReader.CurrentEncoding;
            SkipChar();
        }

        /// <summary>
        /// The current scan character
        /// 
        /// = ctrl-Z at the end of the data
        /// </summary>
        public char Current { get; private set; }

        /// <summary>
        /// Is the scan at the end of the data?
        /// </summary>
        public bool Eof { get; private set; }

        /// <summary>
        /// The encoding of the stream being scanned
        /// </summary>
        public Encoding Encoding { get; private set; }
        
        /// <summary>
        /// Skip to the next character being scanned
        /// 
        /// At Eof the current character is set to Ctrl-Z
        /// </summary>
        public void SkipChar()
        {
            Eof = _streamReader.EndOfStream;
            Current = Eof ? ScanReader.EofChar : (char) _streamReader.Read();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposing || _streamReader == null) return;
            _streamReader.BaseStream.Dispose();
            _streamReader.Dispose();
            _streamReader = null;
        }

        ~ScanStreamReader()
        {
            Dispose(false);
        }
    }
}
