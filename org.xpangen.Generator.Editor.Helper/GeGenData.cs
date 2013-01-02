using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeGenData : IGenData
    {
        public bool Changed { get; set; }
        public GenData DefGenData { get; set; }
        public GenData GenData { get; set; }
        public void SetBase(string filePath)
        {
            if (filePath == "")
                DefGenData = null;
            else
            {
                DefGenData = new GenParameters(CreateStream(filePath));
                GenData = new GenData(DefGenData.AsDef());
            }
        }

        public void SetData(string filePath)
        {
            if (filePath == "")
                GenData = null;
            else
            {
                GenData = DefGenData == null
                              ? new GenParameters(CreateStream(filePath))
                              : new GenParameters(DefGenData.AsDef(), CreateStream(filePath));
            }
        }

        private static FileStream CreateStream(string filePath)
        {
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}