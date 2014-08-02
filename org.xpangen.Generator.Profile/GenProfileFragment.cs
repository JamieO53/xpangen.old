﻿// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenProfileFragment : GenContainerFragmentBase
    {
        public GenProfileFragment(GenDataDef genDataDef)
            : base(new GenProfileParams(genDataDef))
        {
            ClassId = 0;
            Body.ParentSegment = this;
        }

        public Profile.Profile Profile
        {
            get { return (Profile.Profile)Fragment; } 
            set { Fragment = value; }
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
            Body.GenObject = (GenObject) Profile.GenObject;
            return Body.Expand(genData);
        }

        public virtual bool Generate(GenFragmentGenerator genFragmentGenerator)
        {
            Body.GenObject = (GenObject) Profile.GenObject;
            return Body.Generate(genFragmentGenerator.Prefix, genFragmentGenerator.GenData, genFragmentGenerator.Writer);
        }
    }

    public class GenProfileParams : GenFragmentParams
    {
        public GenProfileParams(GenDataDef genDataDef) : base(genDataDef, null, null)
        {
            ProfileDefinition = new ProfileDefinition();
            ProfileDefinition.Setup();
            Fragment = ProfileDefinition.Profile();
            FragmentType = FragmentType.Profile;
        }

        public ProfileDefinition ProfileDefinition { get; set; }
    }
}