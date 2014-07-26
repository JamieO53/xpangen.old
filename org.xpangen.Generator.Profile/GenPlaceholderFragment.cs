// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenPlaceholderFragment : GenFragment
    {
        public GenPlaceholderFragment(GenPlaceholderFragmentParams genPlaceholderFragmentParams) :
            base(genPlaceholderFragmentParams.SetFragmentType(FragmentType.Placeholder))
        {
            Id = genPlaceholderFragmentParams.Id;
        }

        public GenDataId Id {
            get
            {
                return GenDataDef.GetId(Placeholder.Class + "." + Placeholder.Property);
            } 
            set {
                Placeholder.Class = value.ClassName;
                Placeholder.Property = value.PropertyName;
            } 
        }

        public Placeholder Placeholder { get { return (Placeholder) Fragment; } set { Fragment = value; } }
        
        public override string ProfileLabel()
        {
            return Id.Identifier;
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString()].Format;
            return string.Format(format, new object[]
                                             {
                                                 GenDataDef.Classes[Id.ClassId].Name,
                                                 GenDataDef.Classes[Id.ClassId].Properties[Id.PropertyId]
                                             }
                );
        }

        public override string Expand(GenData genData)
        {
            return genData.GetValue(Id);
        }
    }
}