// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenLookup : GenContainerFragmentBase
    {
        private string Condition { get; set; }
        public bool NoMatch { get; set; }

        public new int ClassId { get; set; }
        protected GenDataId Var2 { get; set; }
        protected GenDataId Var1 { get; set; }

        public GenLookup(GenDataDef genDataDef, string condition, GenContainerFragmentBase parentSegment, 
            GenContainerFragmentBase parentContainer)
            : base(genDataDef, parentSegment, parentContainer, FragmentType.Lookup)
        {
            Body.ParentSegment = this;
            Condition = condition;
            var sa = condition.Split('=');
            Var1 = genDataDef.GetId(sa[0]);
            Var2 = genDataDef.GetId(sa[1]);
            ClassId = Var1.ClassId;
        }

        public override string ProfileLabel()
        {
            return (NoMatch ? "~" : "") + GenDataDef.GetIdentifier(Var1) + "=" + GenDataDef.GetIdentifier(Var2);
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString() + (NoMatch ? "2" : "1")].Format;
            return string.Format(format, new object[]
                                             {
                                                 GenDataDef.GetIdentifier(Var1),
                                                 GenDataDef.GetIdentifier(Var2),
                                                 Body.ProfileText(syntaxDictionary)
                                             }
                );
        }

        public override string Expand(GenData genData)
        {
            var result = "";
            if (NoMatch)
            {
                var contextData = genData.DuplicateContext();

                if (genData.Eol(Var2.ClassId))
                {
                    contextData.Reset(Var1.ClassId);
                    result = Body.Expand(contextData);
                }
                else
                {
                    var v = contextData.GetValue(Var2);
                    SearchFor(contextData, ClassId, Var1, v);
                    if (contextData.Eol(ClassId))
                        result = Body.Expand(contextData);
                }
            }
            else
            {
                if (!genData.Eol(Var2.ClassId))
                {
                    var contextData = genData.DuplicateContext();
                    var v = contextData.GetValue(Var2);
                    SearchFor(contextData, ClassId, Var1, v);
                    if (!contextData.Eol(ClassId))
                        result = Body.Expand(contextData);
                }
            }
            return result;
        }

        private static void SearchFor(GenData genData, int classId, GenDataId id, string value)
        {
            genData.First(classId);
            while (!genData.Eol(classId) && genData.GetValue(id) != value)
            {
                genData.Next(classId);
            }
        }
    }
}