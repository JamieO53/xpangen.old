// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using org.xpangen.Generator.Data;

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
                DefGenData = GenData.DataLoader.LoadData(filePath);
                var f = DefGenData.AsDef();
                f.Definition = Path.GetFileNameWithoutExtension(filePath);
                var references = f.Cache.References;
                for (var i = 0; i < references.Count; i++)
                {
                    var reference = references[i].Path;
                    if (!DefGenData.Cache.Contains(reference))
                        DefGenData.Cache.Internal(reference, GenData.DataLoader.LoadData(reference));
                }
                GenData = new GenData(f);
            }
        }

        public void SetData(string filePath)
        {
            if (filePath == "")
                GenData = null;
            else
            {
                GenData = DefGenData == null
                              ? GenData.DataLoader.LoadData(filePath)
                              : GenData.DataLoader.LoadData(DefGenData.AsDef(), filePath);
            }
        }
    }
}