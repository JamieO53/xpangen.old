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
            throw new NotSupportedException("Not supported: Unseekable stream");
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
            Position += count;
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

        public override long Position { get; set; }
    }
}
