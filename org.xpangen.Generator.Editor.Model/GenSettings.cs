using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace Generator.Editor.Model
{
    /// <summary>
    /// Container for generator settings
    /// </summary>
    public class GenSettings : GenApplicationBase
    {
        public GenSettings(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        /// <summary>
        /// The default location of the base files
        /// </summary>
        public string HomeDir
        {
            get { return AsString("HomeDir"); }
            set
            {
                if (HomeDir == value) return;
                SetString("HomeDir", value);
                SaveFields();
            }
        }

        public GenApplicationList<FileGroup> FileGroupList { get; private set; }
        public GenApplicationList<BaseFile> BaseFileList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            FileGroupList = new GenApplicationList<FileGroup>();
            var list = GenObject.SubClass[0];
            list.First();
            while (!list.Eol)
            {
                FileGroupList.Add(new FileGroup(GenDataDef) {GenObject = list.Context});
                list.Next();
            }

            BaseFileList = new GenApplicationList<BaseFile>();
            list = GenObject.SubClass[0];
            list.First();
            while (!list.Eol)
            {
                BaseFileList.Add(new BaseFile(GenDataDef) {GenObject = list.Context});
                list.Next();
            }

        }
    }
}
