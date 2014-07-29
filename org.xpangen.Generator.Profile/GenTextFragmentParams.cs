using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenTextFragmentParams : GenFragmentParams
    {
        public string Text { get; set; }

        /// <summary>
        ///     Parameters for creating a GenFragment
        /// </summary>
        /// <param name="genDataDef">The definition of the data being generated.</param>
        /// <param name="parentSegment">The class segment this fragment belongs to.</param>
        /// <param name="parentContainer">The container fragment conataining this fragment.</param>
        /// <param name="text"></param>
        public GenTextFragmentParams(IGenDataDef genDataDef, GenContainerFragmentBase parentSegment,
            GenContainerFragmentBase parentContainer, string text) : base(genDataDef, parentSegment, parentContainer)
        {
            Text = text;
        }
    }
}