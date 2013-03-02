// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Text;

namespace org.xpangen.Generator.Scanner
{
    /// <summary>
    /// Defines the standard interface for scan readers
    /// </summary>
    public interface IScanReader : IDisposable
    {
        /// <summary>
        /// The current scan character
        /// 
        /// = ctrl-Z at the end of the data
        /// </summary>
        char Current { get; }
        /// <summary>
        /// Is the scan at the end of the data?
        /// </summary>
        bool Eof { get; }

        /// <summary>
        /// The encoding used if file stream is being scanned
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// Skip to the next character being scanned
        /// 
        /// At Eof the current character is set to Ctrl-Z
        /// </summary>
        void SkipChar();
    }
}