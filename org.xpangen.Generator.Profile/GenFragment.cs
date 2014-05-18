// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public abstract class GenFragment : GenNamedApplicationBase
    {
        /// <summary>
        /// The generator data definition.
        /// </summary>
        public GenDataDef GenDataDef { get; set; }

        /// <summary>
        /// The fragment type.
        /// </summary>
        public FragmentType FragmentType { get; set; }

        /// <summary>
        /// Is this a text fragment?
        /// </summary>
        public bool IsTextFragment { get
        {
            return FragmentType == FragmentType.Text ||
                   FragmentType == FragmentType.Placeholder ||
                   FragmentType == FragmentType.Null;
        } }

        public GenContainerFragmentBase ParentSegement { get; set; }

        /// <summary>
        /// Create a new <see cref="GenFragment"/> object.
        /// </summary>
        /// <param name="genDataDef"></param>
        /// <param name="parentSegment"></param>
        protected GenFragment(GenDataDef genDataDef, GenContainerFragmentBase parentSegment)
        {
            GenDataDef = genDataDef;
            ParentSegement = parentSegment;
        }

        /// <summary>
        /// A label identifying the fragment for browsing.
        /// </summary>
        /// <returns>The fragment label.</returns>
        public abstract string ProfileLabel();

        /// <summary>
        /// Profile text equivalent to the fragment.
        /// </summary>
        /// <param name="syntaxDictionary">The dictionary defining the syntax of the profile text.</param>
        /// <returns>The fragment's profile text.</returns>
        public abstract string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary);

        /// <summary>
        /// Expands the fragment.
        /// </summary>
        /// <param name="genData">The generator data.</param>
        /// <returns>The expanded fragment.</returns>
        public abstract string Expand(GenData genData);

        /// <summary>
        /// Generates the fragment to the writer.
        /// </summary>
        /// <param name="prefix">Generated before the fragment, if the fragment is not empty.</param>
        /// <param name="genData">The generator data.</param>
        /// <param name="writer">The writer for the generated output.</param>
        /// <returns>The generated fragment was not empty.</returns>
        public virtual bool Generate(GenFragment prefix, GenData genData, GenWriter writer)
        {
            if (!(this is GenProfileFragment) && ParentSegement == null)
                throw new GeneratorException("Parent segment is not specified", GenErrorType.Assertion);
            var expanded = Expand(genData);
            if (expanded != "" && prefix != null)
                prefix.Generate(null, genData, writer);
            writer.Write(expanded);
            return expanded != "";
        }
    }
}
