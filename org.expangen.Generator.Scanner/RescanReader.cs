using System;
using System.Collections.Generic;
using System.Text;

namespace org.xpangen.Generator.Scanner
{
    /// <summary>
    /// A scan reader that implements rescan functionality
    /// </summary>
    public class RescanReader: IScanReader
    {
        private IScanReader _scanReader;
        private Stack<IScanReader> _stack;

        /// <summary>
        /// The current scan character
        /// 
        /// = ctrl-Z at the end of the data
        /// </summary>
        public char Current
        {
            get
            {
                return _scanReader != null ? _scanReader.Current : ScanReader.EofChar;
            }
        }

        /// <summary>
        /// Is the scan at the end of the data?
        /// </summary>
        public bool Eof
        {
            get { return  _scanReader == null || _scanReader.Eof; }
        }

        public Encoding Encoding { get; private set; }

        /// <summary>
        /// Save the current scanning state, and scan the contents of the scan
        /// reader provided.
        /// 
        /// Scanning resumes from the saved state when the scan reaches the end
        /// of the provided data.
        /// </summary>
        /// <param name="scanReader">The scan reader containing data being rescanned.</param>
        public void Rescan(IScanReader scanReader)
        {
            if (_scanReader != null)
            {
                if (_stack == null)
                    _stack = new Stack<IScanReader>();
                _stack.Push(_scanReader);
            }
            _scanReader = scanReader;
            if (Encoding == null)
                Encoding = scanReader.Encoding;
        }

        /// <summary>
        /// Skip to the next character being scanned
        /// 
        /// At Eof the current character is set to Ctrl-Z
        /// </summary>
        public void SkipChar()
        {

            if (_scanReader != null) 
                _scanReader.SkipChar();
            if (_scanReader == null || _scanReader.Current == ScanReader.EofChar)
                CheckEof();
        }

        /// <summary>
        /// Check if the active scan reader has reached the end of its data and if so
        /// dispose of it and restore the state as it was before its data was scanned.
        /// </summary>
        private void CheckEof()
        {
            if (_scanReader != null && _scanReader.Eof)
            {
                CloseCurrentReader();
                if (_stack != null && _stack.Count > 0)
                {
                    _scanReader = _stack.Pop();
                }
            }
        }

        private void CloseCurrentReader()
        {
            _scanReader.Dispose();
            _scanReader = null;
        }

        /// <summary>
        /// Dispose of uncontrolled data - IDisposable interface
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected void Dispose(bool disposing)
        {
            if (!disposing) return;
            while (_scanReader != null)
            {
                CloseCurrentReader();
                CheckEof();
            }
        }

        ~RescanReader()
        {
            Dispose(false);
        }
    }
}