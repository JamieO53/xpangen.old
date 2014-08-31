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

        private GenFragmentExpander(GenData genData, GenObject genObject, Fragment fragment)
        {
            _genData = genData;
            Fragment = fragment;
            GenObject = genObject;
        }

        private GenData GenData
        {
            get { return _genData; }
        }

        public Fragment Fragment { get; set; }

        protected string Expand()
        {
            using (var s = new MemoryStream(100000))
            {
                var w = new GenWriter(s);
                GenFragmentGenerator.Generate(GenData, w, GenObject, Fragment);
                w.Flush();
                s.Seek(0, SeekOrigin.Begin);
                var r = new StreamReader(s);
                return r.ReadToEnd();
            }
        }

        protected string ExpandSecondary()
        {
            using (var s = new MemoryStream(100000))
            {
                var w = new GenWriter(s);
                GenFragmentGenerator.GenerateSecondary(GenData, w, GenObject, Fragment);
                w.Flush();
                s.Seek(0, SeekOrigin.Begin);
                var r = new StreamReader(s);
                return r.ReadToEnd();
            }
        }

        protected GenObject GenObject { get; private set; }

        public static string Expand(GenData genData, GenObject genObject, Fragment fragment)
        {
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
                    var fn = (Function) fragment;
                    var paramFragments = fn.Body().FragmentList;
                    var param = new string[paramFragments.Count];
                    for (var i = 0; i < paramFragments.Count; i++)
                    {
                        var paramFragment = paramFragments[i];
                        param[i] = Expand(genData, genObject,
                            paramFragment);
                    }
                    return LibraryManager.GetInstance().Execute(fn.FunctionName, param);
                case FragmentType.TextBlock:
                    var sb = new StringBuilder();
                    foreach (var f in ((TextBlock) fragment).Body().FragmentList)
                    {
                        if (f.GetType().Name == "Text")
                            sb.Append(((Text) f).TextValue);
                        else if (f.GetType().Name == "Placeholder")
                            sb.Append(GetPlaceholderValue(f, genObject));
                    }
                    return sb.ToString();
                default:
                    return Create(genData, genObject, fragment).Expand();
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

        private static GenFragmentExpander Create(GenData genData, GenObject genObject, Fragment fragment)
        {
            return new GenFragmentExpander(genData, genObject, fragment);
        }

        public static string ExpandSecondary(GenData genData, GenObject genObject, Fragment fragment)
        {
            if (fragment is ContainerFragment)
                return Create(genData, genObject, fragment).ExpandSecondary();
            return "";
        }
    }
}
