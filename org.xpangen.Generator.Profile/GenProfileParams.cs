using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenProfileParams : GenFragmentParams
    {
        public GenProfileParams(GenDataDef genDataDef) : base(genDataDef, null, null)
        {
            ProfileDefinition = new ProfileDefinition();
            ProfileDefinition.Setup();
            Fragment = ProfileDefinition.Profile();
            FragmentType = FragmentType.Profile;
        }

        private ProfileDefinition ProfileDefinition { get; set; }
    }
}