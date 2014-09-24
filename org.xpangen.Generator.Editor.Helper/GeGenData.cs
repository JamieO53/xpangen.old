// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeGenData : IGenData
    {
        public bool Changed { get { return GenData != null && GenData.Changed; } }
        public GenData DefGenData { get; private set; }
        public GenData GenData { get; private set; }
        public void SetBase(string filePath)
        {
            if (filePath == "")
                DefGenData = null;
            else
            {
                DefGenData = new GenData(GenData.DataLoader.LoadData(filePath));
                var f = DefGenData.AsDef();
                f.DefinitionName = Path.GetFileNameWithoutExtension(filePath);
                var references = f.Cache.References;
                foreach (GenDataDefReferenceCacheItem r in references)
                {
                    var reference = r.Path;
                    if (!DefGenData.Cache.Contains(reference))
                        DefGenData.Cache.Internal(reference, new GenData(GenData.DataLoader.LoadData(reference)));
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
                GenData = new GenData((DefGenData == null
                              ? GenData.DataLoader.LoadData(filePath)
                              : GenData.DataLoader.LoadData(DefGenData.AsDef(), filePath)));
            }
        }
    }
}