﻿// This Source Code Form is subject to the terms of the Mozilla Public
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

        private readonly TextList _fields = new TextList();
        private readonly TextList _names = new TextList();
        private readonly TextList _values = new TextList();
        private int _index = -1;
        public bool AtEnd { get; private set; }

        public bool HasProgressed
        {
            get 
            {
                var progress = Progress;
                Progress = false;
                return progress;
            }
        }

        private bool Progress { get; set; }

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

            ScanWhile(WhiteSpace);
            AtEnd = Eof || CheckChar('.');
            if (AtEnd)
            {
                SkipChar();
                ScanWhile(WhiteSpace);
                return;
            }

            _fields.Clear();
            _values.Clear();
            RecordType = ScanWhile(Identifier);
            if (RecordType == "")
                throw new GeneratorException("Empty record type: " + Buffer);
            ScanWhile(WhiteSpace);
            if (CheckChar('='))
            {
                SkipChar();
                if (CheckChar('{'))
                {
                    while (!Eof && !CheckChar('}'))
                    {
                        SkipChar(); // { or ,
                        ScanWhile(WhiteSpace);
                        var name = StringOrIdentifier();
                        _names.Add(name);
                        ScanWhile(WhiteSpace);
                    }
                    SkipChar();
                    _fields.Add("Name");
                    _values.Add(_names[0]);
                    _index = 1;
                }
                else
                {
                    var name = StringOrIdentifier();
                    ScanWhile(WhiteSpace);
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
            Progress = true;
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
                ScanWhile(WhiteSpace);
                var field = ScanWhile(Identifier);
                string value;
                ScanWhile(WhiteSpace);
                if (CheckChar('='))
                {
                    SkipChar();
                    ScanWhile(WhiteSpace);
                    value = StringOrIdentifier();
                }
                else
                    value = "True";

                ScanWhile(WhiteSpace);
                _fields.Add(field);
                _values.Add(value);
                if (!CheckChar(',') && !CheckChar(']'))
                    throw new GeneratorException(
                        "Invalid character encountered while scanning attributes (" + this + "): " + Buffer + Current,
                        GenErrorType.DataError);
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
