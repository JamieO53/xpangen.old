// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using System.Text;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFragmentExpander : GenBase
    {
        private readonly GenData _genData;

        private GenFragmentExpander(GenData genData, GenFragment genFragment, GenObject genObject, Fragment fragment)
        {
            _genData = genData;
            GenFragment = genFragment;
            Fragment = fragment;
            GenObject = genObject;
        }

        private GenData GenData
        {
            get { return _genData; }
        }

        private GenFragment GenFragment { get; set; }

        public Fragment Fragment { get; set; }

        protected string Expand()
        {
            using (var s = new MemoryStream(100000))
            {
                var w = new GenWriter(s);
                GenFragment genFragment = GenFragment;
                GenFragmentGenerator.Generate(genFragment, GenData, w, GenObject, Fragment);
                w.Flush();
                s.Seek(0, SeekOrigin.Begin);
                var r = new StreamReader(s);
                return r.ReadToEnd();
            }
        }

        protected GenObject GenObject { get; private set; }

        public static string Expand(GenFragment genFragment, GenData genData, GenObject genObject, Fragment fragment)
        {
            if (genFragment.FragmentType != FragmentType.Null && genFragment.FragmentType != FragmentType.Text)
                if (genFragment.FragmentType != FragmentType.Lookup || !((GenLookup)genFragment).NoMatch)
                    Assert(genObject != null, "The genObject must be set");
            FragmentType fragmentType;
            Enum.TryParse(fragment.GetType().Name, out fragmentType);
            switch (fragmentType)
            {
                case FragmentType.Null:
                    return "";
                case FragmentType.Text:
                    return ((Text) fragment).TextValue;
                case FragmentType.Placeholder:
                    return GetPlaceholderValue(fragment, genObject);
                case FragmentType.Function:
                    var fn = ((GenFunction)genFragment);
                    fn.Body.GenObject = genObject;
                    var param = new string[fn.Body.Count];
                    for (var i = 0; i < fn.Body.Count; i++)
                    {
                        var genFragment1 = fn.Body.Fragment[i];
                        genFragment1.GenObject = genObject;
                        param[i] = Expand(genFragment1, genData, genFragment1.GenObject, genFragment1.Fragment);
                    }
                    return LibraryManager.GetInstance().Execute(fn.FunctionName, param);
                case FragmentType.TextBlock:
                    var sb = new StringBuilder();
                    //foreach (var f in ((ContainerFragment) fragment).FragmentBody().FragmentList)
                    //{
                    //    if (f is Text)
                    //        sb.Append(((Text) f).TextValue);
                    //    if (f is Placeholder)
                    //        sb.Append(GetPlaceholderValue(f, genObject));
                    //}
                    foreach (var f in ((GenTextBlock)genFragment).Body.Fragment)
                    {
                        if (f.GetType().Name == "GenTextFragment")
                            sb.Append(((Text)f.Fragment).TextValue);
                        else if (f.GetType().Name == "GenPlaceholder")
                            sb.Append(GetPlaceholderValue(f.Fragment, f.GenObject));
                    }
                    return sb.ToString();
                default:
                    return Create(genFragment, genData, genObject, fragment).Expand();
            }
            
        }

        private static string GetPlaceholderValue(Fragment fragment, GenObject genObject)
        {
            return genObject.GetValue(
                (new GenDataId
                 {
                     ClassName = ((Placeholder) fragment).Class,
                     PropertyName = ((Placeholder) fragment).Property
                 }));
        }

        private static GenFragmentExpander Create(GenFragment genFragment, GenData genData, GenObject genObject, Fragment fragment)
        {
            return new GenFragmentExpander(genData, genFragment, genObject, fragment);
        }
    }
}
