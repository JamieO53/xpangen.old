﻿// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using System.Text;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.FunctionLibrary;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFragmentExpander
    {
        private readonly GenDataDef _genDataDef;

        private GenFragmentExpander(GenDataDef genDataDef, GenObject genObject, Fragment fragment)
        {
            _genDataDef = genDataDef;
            Fragment = fragment;
            GenObject = genObject;
        }

        private GenDataDef GenDataDef
        {
            get { return _genDataDef; }
        }

        private Fragment Fragment { get; set; }

        private string Expand()
        {
            using (var stream = new MemoryStream(100000))
            {
                using (var writer = new GenWriter(stream))
                {
                    GenFragmentGenerator.Generate(GenDataDef, writer, GenObject, Fragment);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }
        }

        private string ExpandSecondary()
        {
            using (var stream = new MemoryStream(100000))
            {
                using (var writer = new GenWriter(stream))
                {
                    GenFragmentGenerator.GenerateSecondary(GenDataDef, writer, GenObject, Fragment);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }
        }

        private GenObject GenObject { get; set; }

        public static string Expand(GenDataDef genDataDef, GenObject genObject, Fragment fragment)
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
                        param[i] = Expand(genDataDef, genObject, paramFragment);
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
                    return Create(genDataDef, genObject, fragment).Expand();
            }
            
        }

        private static string GetPlaceholderValue(Fragment fragment, GenObject genObject)
        {
            bool notFound;
            var placeholderValue = genObject.GetValue(
                (new GenDataId
                 {
                     ClassName = ((Placeholder) fragment).Class,
                     PropertyName = ((Placeholder) fragment).Property
                 }), out notFound);
            if (notFound) return "";
            return placeholderValue;
        }

        private static GenFragmentExpander Create(GenDataDef genDataDef, GenObject genObject, Fragment fragment)
        {
            return new GenFragmentExpander(genDataDef, genObject, fragment);
        }

        public static string ExpandSecondary(GenDataDef genDataDef, GenObject genObject, Fragment fragment)
        {
            if (fragment is ContainerFragment)
                return Create(genDataDef, genObject, fragment).ExpandSecondary();
            return "";
        }
    }
}
