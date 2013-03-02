// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    public class GenContext
    {
        private GenData GenData { get; set; }
        private IndexList Indices { get; set; }

        public GenContext(GenData genData)
        {
            GenData = genData;
            Indices = new IndexList();
        }


        public void SaveContext()
        {
            Indices.Clear();
            for (var i = 0; i < GenData.Context.Count; i++)
                Indices.Add(GenData.Context[i] != null ? GenData.Context[i].Index : 0);
        }

        public void RestoreContext()
        {
            for (var i = 0; i < GenData.Context.Count; i++)
            {
                if (GenData.Context[i] == null || GenData.Context[i].Index == Indices[i]) continue;
                GenData.Context[i].Index = Indices[i];
                GenData.SetSubClasses(i);
            }
        }
    }
}