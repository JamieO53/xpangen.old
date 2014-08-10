// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Text;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenCondition : GenContainerFragmentBase
    {
        public GenCondition(GenConditionParams genConditionParams)
            : base(genConditionParams.SetFragmentType(FragmentType.Condition))
        {
            Var1 = genConditionParams.Var1;
            Var2 = genConditionParams.Var2;
            GenComparison = genConditionParams.GenComparison;
            UseLit = genConditionParams.UseLit;
            Lit = genConditionParams.Lit;
        }

        public GenDataId Var1
        {
            get
            {
                return GenDataDef.GetId(Condition.Class1 + "." + Condition.Property1);
            }
            set
            {
                Condition.Class1 = value.ClassName;
                Condition.Property1 = value.PropertyName;
            }
        }
        
        public GenDataId Var2
        {
            get
            {
                return GenDataDef.GetId(Condition.Class2 + "." + Condition.Property2);
            }
            set
            {
                Condition.Class2 = value.ClassName;
                Condition.Property2 = value.PropertyName;
            }
        }
        
        public string Lit { get { return Condition.Lit; } set { Condition.Lit = value; } }

        public GenComparison GenComparison
        {
            get
            {
                GenComparison c;
                Enum.TryParse(Condition.Comparison, out c);
                return c;
            }
            set { Condition.Comparison = value.ToString(); }
        }

        public bool UseLit
        {
            get { return Condition.UseLit != ""; }
            set { Condition.UseLit = value ? "True" : ""; }
        }

        public Condition Condition
        {
            get { return (Condition) Fragment; }
        }
        
        public override string ProfileLabel()
        {
            var s = new StringBuilder(Var1.Identifier);
            var x =
                ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary.GenComparisonText[
                    (int) GenComparison];
            s.Append(x);

            if (GenComparison != GenComparison.Exists && GenComparison != GenComparison.NotExists)
                s.Append(UseLit ? GenUtilities.StringOrName(Lit) : Var2.Identifier);

            return s.ToString();
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString()].Format;
            return string.Format(format, new object[]
                                             {
                                                 Var1.Identifier,
                                                 syntaxDictionary.GenComparisonText[(int) GenComparison],
                                                 (GenComparison == GenComparison.Exists ||
                                                  GenComparison == GenComparison.NotExists)
                                                     ? ""
                                                     : (UseLit
                                                            ? GenUtilities.StringOrName(Lit)
                                                            : Var2.Identifier),
                                                 Body.ProfileText(syntaxDictionary)
                                             }
                );
        }
    }
}