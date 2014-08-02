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
            get { return Lookup.NoMatch != ""; } 
            set { Lookup.NoMatch = value ? "True" : ""; }
        }

        public new int ClassId { get; private set; }
        protected internal GenDataId Var1
        {
            get { return GenDataDef.GetId(Lookup.Class1 + "." + Lookup.Property1); }
            set
            {
                Lookup.Class1 = value.ClassName;
                Lookup.Property1 = value.PropertyName;
            } 
        }
        protected internal GenDataId Var2
        {
            get { return GenDataDef.GetId(Lookup.Class2 + "." + Lookup.Property2); }
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
            var sa = genLookupParams.Condition.Split('=');
            Var1 = GenDataDef.GetId(sa[0]);
            Var2 = GenDataDef.GetId(sa[1]);
            ClassId = Var1.ClassId;
        }

        public Lookup Lookup
        {
            get { return (Lookup) Fragment; }
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
    }
}