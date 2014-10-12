// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Data;
namespace org.xpangen.Generator.Profile.Profile
{
    /// <summary>
    /// The root class of all profile fragments
    /// </summary>
    public class Fragment : GenNamedApplicationBase
    {
        public Fragment()
        {
            SubClasses.Add("Null");
            SubClasses.Add("Text");
            SubClasses.Add("Placeholder");
            SubClasses.Add("ContainerFragment");
            Properties.Add("Name");
            FragmentType ft;
            if (!Enum.TryParse(GetType().Name, out ft))
                throw new GeneratorException("Unknown fragment type", GenErrorType.Assertion);
            FragmentType = ft;
        }

        public Fragment(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        public string ClassName()
        {
            var fragment = this;
            while (true)
            {
                var segment = fragment as Segment;
                if (segment != null) return segment.Class;
                var lookup = fragment as Lookup;
                if (lookup != null) return lookup.Class1;
                if (fragment is Profile) return "";
                var parent = ((ContainerFragment)fragment.FragmentBody().Links["Parent"]);
                if (parent == null) return "";
                fragment = parent;
            }
        }

        public ProfileDefinition ProfileDefinition()
        {
            return FragmentBody().ProfileDefinition();
        }

        private FragmentBody FragmentBody()
        {
            return (FragmentBody) Parent;
        }

        public FragmentType FragmentType { get; set; }
        
        protected override void GenObjectSetNotification()
        {
        }
    }
}
