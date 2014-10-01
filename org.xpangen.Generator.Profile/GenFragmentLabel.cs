using System;
using System.Diagnostics.Contracts;
using System.Text;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFragmentLabel
    {
        public GenFragmentLabel(Fragment fragment)
        {
            Fragment = fragment;
            FragmentType fragmentType;
            Contract.Assert(Enum.TryParse(Fragment.GetType().Name, out fragmentType), "Fragment type invalid: " + Fragment.GetType().Name);
            FragmentType = fragmentType;
        }

        /// <summary>
        ///     A label identifying the fragment for browsing.
        /// </summary>
        /// <returns>The fragment label.</returns>
        public string ProfileLabel()
        {
            return GetProfileLabel();
        }

        private string GetProfileLabel()
        {
            var label = "";
            var fragment = Fragment;
            switch (FragmentType)
            {
                case FragmentType.Profile:
                    label = "Profile";
                    break;
                case FragmentType.Null:
                    break;
                case FragmentType.Text:
                    label = "Text";
                    break;
                case FragmentType.Placeholder:
                    label = Identifier(((Placeholder) fragment).Class, ((Placeholder) fragment).Property);
                    break;
                case FragmentType.Body:
                    break;
                case FragmentType.Segment:
                    label = ((Segment) fragment).Class;
                    break;
                case FragmentType.Annotation:
                    label = "Annotation";
                    break;
                case FragmentType.Block:
                    label = "Block";
                    break;
                case FragmentType.Lookup:
                    label = (((Lookup) fragment).SecondaryBody().FragmentList.Count > 0 ? "~" : "") +
                            Identifier(((Lookup) fragment).Class1, ((Lookup) fragment).Property1) + "=" +
                            Identifier(((Lookup) fragment).Class2, ((Lookup) fragment).Property2);
                    break;
                case FragmentType.Condition:
                {
                    var condition = (Condition) fragment;
                    GenComparison comparison;
                    Contract.Assert(Enum.TryParse(condition.Comparison, out comparison),
                        "Invalid comparison: " + condition.Comparison);
                    var s = new StringBuilder(Identifier(condition.Class1, condition.Property1));
                    var x =
                        ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary.GenComparisonText[
                            (int) comparison];
                    s.Append(x);

                    if (comparison != GenComparison.Exists && comparison != GenComparison.NotExists)
                        s.Append(condition.UseLit != ""
                            ? GenUtilities.StringOrName(condition.Lit)
                            : Identifier(condition.Class2, condition.Property2));

                    label = s.ToString();
                    break;
                }
                case FragmentType.Function:
                    label = ((Function) fragment).FunctionName;
                    break;
                case FragmentType.TextBlock:
                    label = "Text";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return label;
        }

        private Fragment Fragment { get; set; }

        private FragmentType FragmentType { get; set; }

        private static string Identifier(string className, string propertyName)
        {
            return className + '.' + propertyName;
        }
    }
}