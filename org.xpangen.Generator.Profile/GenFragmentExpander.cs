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
        private readonly Fragment _fragment;

        protected GenFragmentExpander(GenData genData, GenFragment genFragment)
        {
            _genData = genData;
            GenFragment = genFragment;
            _fragment = GenFragment.Fragment;
        }

        public GenData GenData
        {
            get { return _genData; }
        }

        protected GenFragment GenFragment { get; set; }

        public Fragment Fragment
        {
            get { return _fragment; }
        }

        protected virtual string Expand()
        {
            using (var s = new MemoryStream(100000))
            {
                var w = new GenWriter(s);
                GenFragmentGenerator.Generate(GenFragment, GenData, w);
                w.Flush();
                s.Seek(0, SeekOrigin.Begin);
                var r = new StreamReader(s);
                return r.ReadToEnd();
            }
        }

        public static string Expand(GenFragment genFragment, GenData genData)
        {
            if (genFragment.FragmentType != FragmentType.Null && genFragment.FragmentType != FragmentType.Text)
                if (genFragment.FragmentType != FragmentType.Lookup || !((GenLookup)genFragment).NoMatch)
                    Assert(genFragment.GenObject != null, "The genObject must be set");
            FragmentType fragmentType;
            var fragment = genFragment.Fragment;
            Enum.TryParse(fragment.GetType().Name, out fragmentType);
            switch (fragmentType)
            {
                case FragmentType.Null:
                    return "";
                case FragmentType.Text:
                    return ((Text) fragment).TextValue;
                case FragmentType.Placeholder:
                    return genFragment.GenObject.GetValue(((GenPlaceholderFragment) genFragment).Id);
                case FragmentType.Function:
                    var fn = ((GenFunction)genFragment);
                    fn.Body.GenObject = genFragment.GenObject;
                    var param = new string[fn.Body.Count];
                    for (var i = 0; i < fn.Body.Count; i++)
                    {
                        fn.Body.Fragment[i].GenObject = genFragment.GenObject;
                        param[i] = Expand(fn.Body.Fragment[i], genData);
                    }
                    return LibraryManager.GetInstance().Execute(fn.FunctionName, param);
                case FragmentType.TextBlock:
                    var sb = new StringBuilder();
                    foreach (var f in ((GenTextBlock) genFragment).Body.Fragment)
                    {
                        if (f.GetType().Name == "GenTextFragment")
                            sb.Append(((Text) f.Fragment).TextValue);
                        else if (f.GetType().Name == "GenPlaceholder")
                            sb.Append(genFragment.GenObject.GetValue(((GenPlaceholderFragment) f).Id));
                    }
                    return sb.ToString();
                default:
                    return Create(genFragment, genData).Expand();
            }
            
        }

        private static GenFragmentExpander Create(GenFragment genFragment, GenData genData)
        {
            return new GenFragmentExpander(genData, genFragment);
        }
    }
}
