// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// The Lookup fragment data
    /// </summary>
    public class Lookup : GenNamedApplicationBase
    {
        public Lookup()
        {
        }

        public Lookup(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// Generated name of the fragment
        /// </summary>
        public override string Name
        {
            get { return AsString("Name"); }
            set
            {
                if (Name == value) return;
                SetString("Name", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// Is the body expanded if the lookup fails?
        /// </summary>
        public string NoMatch
        {
            get { return AsString("NoMatch"); }
            set
            {
                if (NoMatch == value) return;
                SetString("NoMatch", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The class of the object being sought
        /// </summary>
        public string Class1
        {
            get { return AsString("Class1"); }
            set
            {
                if (Class1 == value) return;
                SetString("Class1", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The property on the object being sought to match
        /// </summary>
        public string Property1
        {
            get { return AsString("Property1"); }
            set
            {
                if (Property1 == value) return;
                SetString("Property1", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The class of the object with the search value
        /// </summary>
        public string Class2
        {
            get { return AsString("Class2"); }
            set
            {
                if (Class2 == value) return;
                SetString("Class2", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The property on the object with the search value
        /// </summary>
        public string Property2
        {
            get { return AsString("Property2"); }
            set
            {
                if (Property2 == value) return;
                SetString("Property2", value);
                if (!DelayedSave) SaveFields();
            }
        }

    }
}