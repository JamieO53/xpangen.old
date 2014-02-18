// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
                DefGenData = new GenParameters(GenParameters.CreateStream(filePath));
                var f = DefGenData.AsDef();
                f.Definition = Path.GetFileNameWithoutExtension(filePath);
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
                              ? new GenParameters(GenParameters.CreateStream(filePath))
                              : new GenParameters(DefGenData.AsDef(), GenParameters.CreateStream(filePath));
            }
        }
    }
}