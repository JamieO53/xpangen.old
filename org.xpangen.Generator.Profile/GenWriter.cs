using System.IO;

namespace org.xpangen.Generator.Profile
{
    public class GenWriter
    {
        private string _fileName;
        public Stream Stream { get; private set; }
        private StreamWriter Writer { get; set; }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == value) return;
                if (!(Stream is FileStream)) return;
                
                _fileName = value;
                Writer.Dispose();
                Stream.Dispose();
                Stream = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite);
                Writer = new StreamWriter(Stream);
            }
        }

        public GenWriter(Stream stream)
        {
            Stream = stream;
            Writer = new StreamWriter(stream);
        }

        public void Write(string text)
        {
            Writer.Write(text);
        }

        public void Flush()
        {
            Writer.Flush();
        }
    }
}
