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
            set { Fragment = value; }
        }
        
        public override string ProfileLabel()
        {
            var s = new StringBuilder(GenDataDef.GetIdentifier(Var1));
            switch (GenComparison)
            {
                case GenComparison.Exists:
                    break;
                case GenComparison.NotExists:
                    s.Append("~");
                    break;
                case GenComparison.Eq:
                    s.Append("=");
                    break;
                case GenComparison.Ne:
                    s.Append("<>");
                    break;
                case GenComparison.Lt:
                    s.Append("<");
                    break;
                case GenComparison.Le:
                    s.Append("<=");
                    break;
                case GenComparison.Gt:
                    s.Append(">");
                    break;
                case GenComparison.Ge:
                    s.Append(">=");
                    break;
                default:
                    s.Append("<Unknown>");
                    break;
            }

            if (GenComparison != GenComparison.Exists && GenComparison != GenComparison.NotExists)
                s.Append(UseLit ? GenUtilities.StringOrName(Lit) : GenDataDef.GetIdentifier(Var2));

            return s.ToString();
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString()].Format;
            return string.Format(format, new object[]
                                             {
                                                 GenDataDef.GetIdentifier(Var1),
                                                 syntaxDictionary.GenComparisonText[(int) GenComparison],
                                                 (GenComparison == GenComparison.Exists ||
                                                  GenComparison == GenComparison.NotExists)
                                                     ? ""
                                                     : (UseLit
                                                            ? GenUtilities.StringOrName(Lit)
                                                            : GenDataDef.GetIdentifier(Var2)),
                                                 Body.ProfileText(syntaxDictionary)
                                             }
                );
        }

        public override string Expand(GenData genData)
        {
            Body.GenObject = GenObject;
            return Test(genData) ? Body.Expand(genData) : "";
        }

        internal bool Test(GenData genData)
        {
            string s1;
            string s2;
            GetOperands(genData, out s1, out s2);

            var i = String.Compare(s1, s2, StringComparison.Ordinal);

            switch (GenComparison)
            {
                case GenComparison.Exists:
                    return i != 0;
                case GenComparison.NotExists:
                    return i == 0;
                case GenComparison.Eq:
                    return i == 0;
                case GenComparison.Ne:
                    return i != 0;
                case GenComparison.Lt:
                    return i < 0;
                case GenComparison.Le:
                    return i <= 0;
                case GenComparison.Gt:
                    return i > 0;
                case GenComparison.Ge:
                    return i >= 0;
                default:
                    throw new Exception("<<<<Invalid condition comparison type>>>>");
            }
        }

        private void GetOperands(GenData genData, out string value1, out string value2)
        {
            value1 = genData.GetValue(Var1);
            value2 = UseLit
                         ? Lit
                         : (GenComparison == GenComparison.Exists || GenComparison == GenComparison.NotExists
                                ? ""
                                : genData.GetValue(Var2));

            if (GenComparison != GenComparison.Exists && GenComparison != GenComparison.NotExists &&
                value1.Length != value2.Length &&
                value1 != "" && value2 != "" &&
                GenUtilities.IsNumeric(value1) && GenUtilities.IsNumeric(value2))
                GenUtilities.PadShortNumericOperand(ref value1, ref value2);
        }
    }
}