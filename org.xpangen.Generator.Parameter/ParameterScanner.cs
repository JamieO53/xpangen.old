// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Scanner;

namespace org.xpangen.Generator.Parameter
{
    /// <summary>
    /// A scanner for loading parameter data.
    /// </summary>
    public class ParameterScanner : ScanReader
    {
        /// <summary>
        /// The record type being loaded - corresponds to a class in the generator data definition.
        /// </summary>
        public string RecordType { get; private set; }

        private static readonly CharSet Spacing = new CharSet("\r\n ");

        private readonly TextList _fields = new TextList();
        private readonly TextList _names = new TextList();
        private readonly TextList _values = new TextList();
        private int _index = -1;
        public bool AtEnd { get; private set; }

        public ParameterScanner(string text) : base(text)
        {
        }

        public ParameterScanner(Stream stream) : base(stream)
        {
        }

        public void ScanObject()
        {
            if (_index > -1)
            {
                _values[0] = _names[_index];
                _index++;
                if (_index >= _names.Count)
                {
                    _index = -1;
                    _names.Clear();
                    return;
                }
            }

            ScanWhile(Spacing);
            AtEnd = Eof || CheckChar('.');
            if (AtEnd)
            {
                SkipChar();
                ScanWhile(Spacing);
                return;
            }

            _fields.Clear();
            _values.Clear();
            RecordType = ScanWhile(Identifier);
            ScanWhile(Spacing);
            if (CheckChar('='))
            {
                SkipChar();
                if (CheckChar('{'))
                {
                    while (!Eof && !CheckChar('}'))
                    {
                        SkipChar(); // { or ,
                        ScanWhile(Spacing);
                        var name = StringOrIdentifier();
                        _names.Add(name);
                        ScanWhile(Spacing);
                    }
                    SkipChar();
                    _fields.Add("Name");
                    _values.Add(_names[0]);
                    _index = 1;
                }
                else
                {
                    var name = StringOrIdentifier();
                    ScanWhile(Spacing);
                    _fields.Add("Name");
                    _values.Add(name);
                    if (CheckChar('['))
                        ScanAttributes();
                    if(Eof)
                        Rescan(" "); // Force processing of current record
                }
            }
            else if (CheckChar('['))
                ScanAttributes();
        }

        public string Attribute(string fieldName)
        {
            var i = _fields.IndexOf(fieldName);
            return i == -1 ? "" : _values[i];
        }

        private void ScanAttributes()
        {
            do
            {
                SkipChar(); // [ or ,
                ScanWhile(Spacing);
                var field = ScanWhile(Identifier);
                string value;
                ScanWhile(Spacing);
                if (CheckChar('='))
                {
                    SkipChar();
                    ScanWhile(Spacing);
                    value = StringOrIdentifier();
                }
                else
                    value = "True";

                ScanWhile(Spacing);
                _fields.Add(field);
                _values.Add(value);
                if (!CheckChar(',') && !CheckChar(']'))
                    throw new Exception("Invalid character encountered while scanning attributes (" + this + "): " + Current);
            } while (!Eof && !CheckChar(']'));
            SkipChar();
        }

        private string StringOrIdentifier()
        {
            return CheckChar('\'') ? ScanQuotedString() : ScanWhile(Identifier);
        }

        public override string ToString()
        {
            return RecordType + (Attribute("Name") != "" ? "=" + Attribute("Name") : "");
        }
    }
}
