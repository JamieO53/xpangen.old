// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Scanner
{
    public class ScanBuffer
    {
        private const int Size = 100;
        private readonly char[] _buffer = new char[Size];
        private int _length;

        public void Append(char c)
        {
            _buffer[_length%Size] = c;
            _length++;
        }

        public override string ToString()
        {
            string s = _length == 0
                ? ""
                : (_length <= Size
                    ? new string(_buffer, 0, _length)
                    : new string(_buffer, _length%Size, Size - _length%Size) +
                      new string(_buffer, 0, _length%Size));
            return s;
        }
    }
}
