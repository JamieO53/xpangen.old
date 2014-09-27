// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeGenData : IGenData
    {
        public bool Changed { get { return GenDataBase != null && GenDataBase.Changed; } }
        public GenDataBase DefGenDataBase { get; private set; }
        public GenDataBase GenDataBase { get; private set; }
        public void SetBase(string filePath)
        {
            if (filePath == "")
                DefGenDataBase = null;
            else
            {
                DefGenDataBase = GenDataBase.DataLoader.LoadData(filePath);
                var f = DefGenDataBase.AsDef();
                f.DefinitionName = Path.GetFileNameWithoutExtension(filePath);
                GenDataBase = new GenDataBase(f);
            }
        }

        public void SetData(string filePath)
        {
            if (filePath == "")
                GenDataBase = null;
            else
            {
                GenDataBase = DefGenDataBase == null
                              ? GenDataBase.DataLoader.LoadData(filePath)
                              : GenDataBase.DataLoader.LoadData(DefGenDataBase.AsDef(), filePath);
            }
        }
    }
}