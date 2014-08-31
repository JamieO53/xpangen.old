// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenLookup : GenContainerFragmentBase
    {
        private string Condition { get; set; }

        public bool NoMatch
        {
            get { return Body.SecondaryCount > 0; } 
        }

        public new int ClassId { get; private set; }

        private GenDataId Var1
        {
            get { return GenDataDef.GetId(Lookup.Class1, Lookup.Property1); }
            set
            {
                Lookup.Class1 = value.ClassName;
                Lookup.Property1 = value.PropertyName;
            } 
        }

        private GenDataId Var2
        {
            get { return GenDataDef.GetId(Lookup.Class2, Lookup.Property2); }
            set
            {
                Lookup.Class2 = value.ClassName;
                Lookup.Property2 = value.PropertyName;
            } 
        }

        public GenLookup(GenLookupParams genLookupParams)
            : base(genLookupParams.SetFragmentType(FragmentType.Lookup))
        {
            Body.ParentSegment = this;
            Condition = genLookupParams.Condition;
            Var1 = genLookupParams.Var1;
            Var2 = genLookupParams.Var2;
            ClassId = Var1.ClassId;
        }

        private Lookup Lookup
        {
            get { return (Lookup) Fragment; }
        }
        public override string ProfileLabel()
        {
            return (NoMatch ? "~" : "") + Var1.Identifier + "=" + Var2.Identifier;
        }

        //public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        //{
        //    var noMatch = Body.SecondaryCount > 0;
        //    var format = syntaxDictionary[FragmentType + (noMatch ? "2" : "1")].Format;
        //    if (!noMatch)
        //        return string.Format(format, new object[]
        //                                     {
        //                                         Var1.ToString(),
        //                                         Var2.ToString(),
        //                                         Body.ProfileText(syntaxDictionary)
        //                                     }
        //            );
        //    return string.Format(format, new object[]
        //                                 {
        //                                     Var1.ToString(),
        //                                     Var2.ToString(),
        //                                     Body.ProfileText(syntaxDictionary),
        //                                     Body.SecondaryProfileText(syntaxDictionary)
        //                                 }
        //        );
        //}
    }
}