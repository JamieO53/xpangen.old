// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenWriter : IDisposable
    {
        private string _fileName;
        private StreamWriter _writer;
        public Stream Stream { get; private set; }
        public int Position { get; private set; }

        private StreamWriter Writer
        {
            get
            {
                if (_writer == null)
                    throw new GeneratorException("No generator output has been specified.", GenErrorType.NoOutputFile);
                return _writer;
            }
            set { _writer = value; }
        }

        public string FileName
        {
            private get { return _fileName; }
            set
            {
                if (_fileName == value) return;
                _fileName = value;

                if (Stream != null && !(Stream is FileStream)) return;

                if (_writer != null) _writer.Dispose();
                if (Stream != null) Stream.Dispose();

                if (string.IsNullOrEmpty(_fileName)) return;

                EnsurePathExists(_fileName);
                Stream = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite);
                Writer = new StreamWriter(Stream);
            }
        }

        private static void EnsurePathExists(string fileName)
        {
            var directoryName = Path.GetDirectoryName(fileName) ?? "";
            if (directoryName != "" && !Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
        }

        public GenWriter(Stream stream)
        {
            Position = 0;
            Stream = stream;
            Writer = stream != null ? new StreamWriter(stream) : null;
        }

        public void Write(string text, TextPosition position = null)
        {
            var p = Position;
            if (!string.IsNullOrEmpty(ProvisionalText))
            {
                Position += ProvisionalText.Length;
                Writer.Write(ProvisionalText);
                ClearProvisionalText();
            }
            Position += text.Length;
            Writer.Write(text);
            if (position != null)
            {
                if (position.Offset == 0 && position.Length == 0) position.Offset = p;
                position.Length = Position - position.Offset;
            }
        }

        public void ClearProvisionalText()
        {
            ProvisionalText = "";
        }

        public void ProvisionalWrite(string text)
        {
            ProvisionalText = text;
        }

        private string ProvisionalText { get; set; }

        public void Flush()
        {
            Writer.Flush();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Writer != null) Writer.Dispose();
                if (Stream != null) Stream.Dispose();
            }
        }

        ~GenWriter()
        {
            Dispose(false);
        }
    }
}