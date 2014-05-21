// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenPlaceholderFragment : GenFragment
    {
        public GenPlaceholderFragment(GenDataDef genDataDef, GenContainerFragmentBase parentSegment,
            GenData genData = null, ProfileRoot profileRoot = null, GenObject genObject = null) :
            base(genDataDef, parentSegment, FragmentType.Placeholder, genData, profileRoot, genObject)
        {
        }

        public GenDataId Id { get; set; }

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