// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Application;
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

        private GenList<Class> ClassList
        {
            get
            {
                var def = DefGenData ?? (GenDataDef != null ? GenDataDef.AsGenData() : null);
                if (def == null)
                    return null;
                
                var list = new GenList<Class>();
                AddClasses(def, list);
                var references = def.Cache.References;
                for (var i = 0; i < references.Count; i++)
                {
                    var reference = references[i];
                    AddClasses(reference.GenData, list);
                }
                return list;
            }
        }

        private static void AddClasses(GenData def, GenList<Class> list)
        {
            def.First(1);
            while (!def.Eol(1))
            {
                list.Add(new Class(def) {GenObject = def.Context[1].GenObject});
                def.Next(1);
            }
        }

        public GenSegment Profile { get; set; } 
        public IGenData GenDataStore { get; set; }
        public IGenDataSettings Settings { get; set; }
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

        public GenApplicationBase FindClassDefinition(int classId)
        {
            var defClass = GenDataDef.Classes[classId];
            for (var i = 0; i < ClassList.Count; i++)
                if (ClassList[i].Name == defClass.Name)
                    return ClassList[i];
            return null;
        }
    }
}