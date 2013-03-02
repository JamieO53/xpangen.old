// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;
using org.xpangen.Generator.Profile;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeData
    {
        public bool Changed { get; set; }
        public bool Created { get; set; }
        public bool DataChanged { get; set; }
        public IGenTreeNavigator DataNavigator { get; set; }
        public GenData DefGenData
        {
            get { return GenDataStore.DefGenData; }
        }

        public GenData GenData
        {
            get { return GenDataStore.GenData; }
        }
        public GenDataDef GenDataDef
        {
            get { return GenData != null ? GenData.GenDataDef : null; }
        }
        public GenList<Class> ClassList
        {
            get
            {
                var def = DefGenData ?? (GenDataDef != null ? GenDataDef.AsGenData() : null);
                if (def == null)
                    return null;
                
                var list = new GenList<Class>();
                def.First(1);
                while (!def.Eol(1))
                {
                    list.Add(new Class(def.GenDataDef) {GenObject = def.Context[1].Context});
                    def.Next(1);
                }
                return list;
            }
        }
        public GenSegment Profile { get; set; } 
        public IGenData GenDataStore { get; set; }
        public IGenSettings Settings { get; set; }
        public bool Testing { get; set; }
        public IGenValidator Validator { get; set; }

        public GeData()
        {
            GenDataStore = new GeGenData();
            Validator = new GeGridDataValidator(this);
        }
        public void GridKeyPress()
        {
            // todo: signature for key press event handler
            throw new NotImplementedException("GridKeyPress method not implemented");
        }
    }
}