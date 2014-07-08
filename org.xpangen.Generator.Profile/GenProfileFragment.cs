﻿// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenProfileFragment : GenContainerFragmentBase
    {
        public GenProfileFragment(GenDataDef genDataDef, GenData genData = null)
            : this(genDataDef, genData == null ? DefaultProfileData(genDataDef) : new ProfileDefinition(genData))
        {
        }

        private GenProfileFragment(GenDataDef genDataDef, ProfileDefinition model)
            : base(
                genDataDef, null, FragmentType.Profile, model.GenData, model.ProfileRootList[0],
                (GenObject)model.ProfileRootList[0].FragmentBodyList[0].GenObject)
        {
            Model = model;
            ClassId = 0;
            Body.ParentSegment = this;
        }

        public ProfileDefinition Model { get; private set; }

        private static ProfileDefinition DefaultProfileData(GenDataDef genDataDef)
        {
            var model = new ProfileDefinition();
            var r = model.AddProfileRoot("Profile");
            var b = r.AddFragmentBody("ProfileBody");
            b.AddProfile("Profile");
            return model;
        }

        public override string ProfileLabel()
        {
            return "Profile";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            return Body.ProfileText(syntaxDictionary);
        }

        public override string Expand(GenData genData)
        {
            return Body.Expand(genData);
        }

        public override bool Generate(GenFragment prefix, GenData genData, GenWriter writer)
        {
            return Body.Generate(prefix, genData, writer);
        }
    }
}