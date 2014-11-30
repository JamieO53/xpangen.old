using System.Collections.Generic;
using System.Diagnostics.Contracts;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Editor.Helper
{
    public class FragmentSelection
    {
        private TextSelection _textPrefix;
        private TextSelection _textSuffix;

        public FragmentSelection()
        {
            Fragments = new List<Fragment>();
        }

        public List<Fragment> Fragments { get; private set; }

        public TextSelection TextPrefix
        {
            get { return _textPrefix = _textPrefix ?? new TextSelection(); }
        }

        public TextSelection TextSuffix
        {
            get { return _textSuffix = _textSuffix ?? new TextSelection(); }
        }

        public bool HasPrefix { get { return _textPrefix != null; }}

        public bool HasSuffix { get { return _textSuffix != null; }}

        public string Prefix { get { return HasPrefix ? TextPrefix.AsPrefix : ""; } }
        public string Suffix { get { return HasSuffix ? TextSuffix.AsSuffix : ""; } }
    }

    public class TextSelection
    {
        public Text Text { get; set; }
        int Position { get; set; }

        internal string AsPrefix
        {
            get
            {
                Contract.Requires(Position >= 0);
                Contract.Requires(Position <= Text.TextValue.Length);
                return Text.TextValue.Substring(Position);
            }
        }

        internal string AsSuffix
        {
            get
            {
                Contract.Requires(Position >= 0);
                Contract.Requires(Position <= Text.TextValue.Length);
                return Text.TextValue.Substring(0, Position);
            }
        }
    }
}