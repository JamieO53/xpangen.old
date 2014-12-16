using System.Collections.Generic;
using System.Diagnostics.Contracts;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Editor.Helper
{
    public class FragmentSelection
    {
        private TextSelection _textPrefix;
        private TextSelection _textSuffix;
        private TextSelection _textInfix;

        public FragmentSelection(int start, int end)
        {
            Fragments = new List<Fragment>();
            Start = start;
            End = end;
        }

        public void SetPrefix(Text text, int position)
        {
            Contract.Requires(text != null);
            Contract.Requires(position > 0);
            Contract.Requires(position < text.TextValue.Length);

            TextPrefix.Text = text;
            TextPrefix.Position = position;
            TextPrefix.Length = text.TextValue.Length - position;
        }
        
        public void SetSuffix(Text text, int position)
        {
            Contract.Requires(text != null);
            Contract.Requires(position > 0);
            Contract.Requires(position < text.TextValue.Length);

            TextSuffix.Text = text;
            TextSuffix.Position = 0;
            TextSuffix.Length = position;
        }
        
        public void SetInfix(Text text, int startPosition, int endPosition)
        {
            Contract.Requires(text != null);
            Contract.Requires(startPosition > 0);
            Contract.Requires(endPosition < text.TextValue.Length);
            Contract.Requires(startPosition < endPosition);

            TextInfix.Text = text;
            TextInfix.Position = startPosition;
            TextInfix.Length = endPosition - startPosition;
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

        public TextSelection TextInfix
        {
            get { return _textInfix = _textInfix ?? new TextSelection(); }
        }

        public int Start { get; private set; }
        public int End { get; private set; }
        public bool HasPrefix { get { return _textPrefix != null; }}
        public bool HasSuffix { get { return _textSuffix != null; }}
        public bool HasInfix { get { return _textInfix != null; }}
        public string Prefix { get { return HasPrefix ? _textPrefix.AsPrefix : ""; } }
        public string Suffix { get { return HasSuffix ? _textSuffix.AsSuffix : ""; } }
        public string Infix { get { return HasInfix ? _textInfix.AsInfix : ""; } }
        public string ProfileText { get; set; }
    }

    public class TextSelection
    {
        public Text Text { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }

        internal string AsPrefix
        {
            get
            {
                Contract.Requires(Position > 0);
                Contract.Requires(Length > 0);
                Contract.Requires(Position + Length == Text.TextValue.Length);
                return Text.TextValue.Substring(Position, Length);
            }
        }

        internal string AsSuffix
        {
            get
            {
                Contract.Requires(Position == 0);
                Contract.Requires(Length > 0);
                Contract.Requires(Length < Text.TextValue.Length);
                return Text.TextValue.Substring(Position, Length);
            }
        }

        internal string AsInfix
        {
            get
            {
                Contract.Requires(Position > 0);
                Contract.Requires(Length > 0);
                Contract.Requires(Position + Length < Text.TextValue.Length);
                return Text.TextValue.Substring(Position, Length);
            }
        }
    }
}