namespace org.xpangen.Generator.Profile
{
    /// <summary>
    /// Contains syntactic information specific for a profile fragment type.
    /// </summary>
    public class ProfileFragmentSyntax
    {
        /// <summary>
        /// The profile fragment type.
        /// </summary>
        public FragmentType FragmentType { get; set; }
        /// <summary>
        /// The variant of the syntax for the fragment type.
        /// </summary>
        public int Variant { get; set; }
        /// <summary>
        /// The key identifying the fragment type and variant.
        /// </summary>
        public string Key { get { return FragmentType.ToString() + (Variant == 0 ? "" : Variant.ToString()); } }
        /// <summary>
        /// The format string for decompiling the fragment type and variant.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Create a new <see cref="ProfileFragmentSyntax"/> object.
        /// </summary>
        public ProfileFragmentSyntax()
        {
            Variant = 0;
        }
    }
}
