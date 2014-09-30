using System;
using System.IO;
using System.Text;

namespace org.xpangen.Generator.Profile
{
    /// <summary>
    /// Writes to a string with a predetermined length
    /// </summary>
    public class StringStream : Stream
    {
        private readonly StringBuilder _sb;
        private readonly long _length;

        public StringStream(int length)
        {
            _sb = new StringBuilder(length);
            _length = length;
        }
        public override void Flush()
        {
            _sb.Clear();
            Position = 0;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("Not supported: nonseekable stream");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("Not supported: the length is predetermined");
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Not supported: nonreadable stream");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < offset + count; i++)
                _sb.Append(buffer[offset]);
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return _length; }
        }

        public override long Position
        {
            get { return _sb.Length; }
            set { throw new NotSupportedException("Not supported: the position is the same as the length"); }
        }
    }
}
