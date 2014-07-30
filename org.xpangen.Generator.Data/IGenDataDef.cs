// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    public interface IGenDataDef
    {
        GenDataDefClassList Classes { get; }
        int CurrentClassId { get; set; }
        string DefinitionName { get; set; }
        GenDataDefReferenceCache Cache { get; }
        int AddClass(string parent, string name);
        GenDataId GetId(string name);
        GenDataId GetId(string name, bool createIfMissing);
        int IndexOfSubClass(int classId, int subClassId);
        GenData AsGenData();
        void AddSubClass(string className, string subClassName);
        void AddSubClass(string className, string subClassName, string reference);
        void AddInheritor(string className, string inheritorName);
        int AddClass(string className);
        string GetIdentifier(GenDataId genDataId);
    }
}