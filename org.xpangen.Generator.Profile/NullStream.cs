using System;
using System.IO;

namespace org.xpangen.Generator.Profile
{
    /// <summary>
    /// This class produces on output, but accepts write commands
    /// </summary>
    public class NullStream: Stream
    {
        private long _length;
        private long _position;

        public NullStream()
        {
            _length = 0;
        }
        
        public override void Flush()
        {
            // Ignored
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    _position = offset;
                    break;
                case SeekOrigin.Current:
                    _position += offset;
                    break;
                case SeekOrigin.End:
                    _position -= offset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("origin");
            }
            return _position;
        }

        public override void SetLength(long value)
        {
            _length = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Not supported: Unreadable stream");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _length += count;
            _position += count;
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return true; }
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
            get { return _position; }
            set { _position = value; }
        }
    }
}
