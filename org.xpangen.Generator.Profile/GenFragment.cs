// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Profile;

namespace org.xpangen.Generator.Profile
{
    public abstract class GenFragment : Fragment
    {
        private FragmentType _fragmentType;

        /// <summary>
        ///     The fragment type.
        /// </summary>
        public new FragmentType FragmentType
        {
            get { return _fragmentType; }
            protected set
            {
                if (GenObject != null)
                    base.FragmentType = value.ToString();
                _fragmentType = value;
            }
        }

        public new GenObject GenObject
        {
            get { return base.GenObject; }
            set
            {
                base.GenObject = value;
                base.FragmentType = _fragmentType.ToString();
            }
        }
        
        /// <summary>
        ///     Is this a text fragment?
        /// </summary>
        public bool IsTextFragment
        {
            get
            {
                return FragmentType == FragmentType.Text ||
                       FragmentType == FragmentType.Placeholder ||
                       FragmentType == FragmentType.Null;
            }
        }

        public GenContainerFragmentBase ParentSegement { get; set; }

        /// <summary>
        ///     Create a new <see cref="GenFragment" /> object.
        /// </summary>
        /// <param name="genDataDef">The definition of the data being generated.</param>
        /// <param name="parentSegment">The class segment this fragment belongs to.</param>
        /// <param name="fragmentType">The type of fragment.</param>
        /// <param name="genData">The profile data.</param>
        /// <param name="profileRoot">The parent of all the profile fragments in the profile data.</param>
        /// <param name="genObject">The data for this fragment.</param>
        protected GenFragment(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, 
            FragmentType fragmentType, GenData genData = null, ProfileRoot profileRoot = null, 
            GenObject genObject = null) 
            : base(parentSegment != null ? parentSegment.GenData : genData)
        {
            GenDataDef = genDataDef;
            ParentSegement = parentSegment;
            FragmentType = fragmentType;
            base.GenObject = genObject;
            ProfileRoot = profileRoot;
        }

        protected ProfileRoot ProfileRoot { get; set; }

        /// <summary>
        ///     A label identifying the fragment for browsing.
        /// </summary>
        /// <returns>The fragment label.</returns>
        public abstract string ProfileLabel();

        /// <summary>
        ///     Profile text equivalent to the fragment.
        /// </summary>
        /// <param name="syntaxDictionary">The dictionary defining the syntax of the profile text.</param>
        /// <returns>The fragment's profile text.</returns>
        public abstract string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary);

        /// <summary>
        ///     Expands the fragment.
        /// </summary>
        /// <param name="genData">The generator data.</param>
        /// <returns>The expanded fragment.</returns>
        public abstract string Expand(GenData genData);

        /// <summary>
        ///     Generates the fragment to the writer.
        /// </summary>
        /// <param name="prefix">Generated before the fragment, if the fragment is not empty.</param>
        /// <param name="genData">The generator data.</param>
        /// <param name="writer">The writer for the generated output.</param>
        /// <returns>The generated fragment was not empty.</returns>
        public virtual bool Generate(GenFragment prefix, GenData genData, GenWriter writer)
        {
            var expanded = Expand(genData);
            if (expanded != "" && prefix != null)
                prefix.Generate(null, genData, writer);
            writer.Write(expanded);
            return expanded != "";
        }
    }
}