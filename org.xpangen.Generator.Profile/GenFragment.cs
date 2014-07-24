﻿// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public abstract class GenFragment : GenBase
    {
        private FragmentType _fragmentType;
        private Fragment _fragment;
        private ContainerFragment _containerFragment;

        /// <summary>
        /// The fragment object holding the fragment's data
        /// </summary>
        protected Fragment Fragment
        {
            get { return _fragment; }
            set
            {
                _fragment = value;
                CheckFragmentType(value);
            }
        }

        private void CheckFragmentType(Fragment fragment)
        {
            Enum.TryParse(fragment.GetType().Name, out _fragmentType);
        }

        /// <summary>
        /// The container of this fragment
        /// </summary>
        protected ContainerFragment ContainerFragment
        {
            get { return _containerFragment; }
            set
            {
                _containerFragment = value;
                CheckFragment(FragmentType);
            }
        }

        /// <summary>
        ///     The fragment type.
        /// </summary>
        public FragmentType FragmentType
        {
            get { return _fragmentType; }
            protected set
            {
                _fragmentType = value;
                CheckFragment(value);
            }
        }

        private void CheckFragment(FragmentType fragmentType)
        {
            if (ContainerFragment == null || Fragment != null) return;
            switch (fragmentType)
            {
                case FragmentType.Profile:
                    Fragment = new ContainerFragment();
                    break;
                case FragmentType.Text:
                    Fragment = new Text();
                    break;
                case FragmentType.Placeholder:
                    Fragment = new Placeholder();
                    break;
                    //case FragmentType.Body:
                    //    break;
                case FragmentType.Segment:
                    Fragment = new Segment();
                    break;
                case FragmentType.Block:
                    Fragment = new Block();
                    break;
                case FragmentType.Lookup:
                    Fragment = new Lookup();
                    break;
                case FragmentType.Condition:
                    Fragment = new Condition();
                    break;
                case FragmentType.Function:
                    Fragment = new Function();
                    break;
                case FragmentType.TextBlock:
                    Fragment = new TextBlock();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("fragmentType");
            }
        }

        public GenDataDef GenDataDef { get; private set; }
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

        public GenContainerFragmentBase ParentSegment { get; set; }
        public GenContainerFragmentBase ParentContainer { get; set; }

        /// <summary>
        ///     Create a new <see cref="GenFragment" /> object.
        /// </summary>
        /// <param name="genDataDef">The definition of the data being generated.</param>
        /// <param name="parentSegment">The class segment this fragment belongs to.</param>
        /// <param name="parentContainer"></param>
        /// <param name="fragmentType">The type of fragment.</param>
        protected GenFragment(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, 
            GenContainerFragmentBase parentContainer, FragmentType fragmentType)
        {
            GenDataDef = genDataDef;
            ParentSegment = parentSegment;
            ParentContainer = parentContainer;
            FragmentType = fragmentType;
        }

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