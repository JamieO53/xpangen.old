using System.Collections.Generic;
using System.Linq;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class ProfileTextPostionList : SortedDictionary<long, ProfileTextPosition>
    {
        public ProfileTextPosition FindAtPosition(int position)
        {
            return
                this.LastOrDefault(
                    p => p.Value.Position.Offset <= position && position <= p.Value.Position.EndPosition)
                    .Value;
        }

        public new void Add(long key, ProfileTextPosition value)
        {
            if (ContainsKey(key)) return;
                // Function parameters get added twice
            base.Add(key, value);
        }
    }

    public class TextPosition
    {
        public int Offset { get; set; }
        public int Length { get; set; }
        public long Key { get { return Offset*ProfileTextPosition.TwoTo32 + Length; } }
        public int EndPosition { get { return Offset + Length; }}

        public TextPosition()
        {
            Offset = 0;
            Length = 0;
        }

        public override string ToString()
        {
            return Offset + ":" + Length;
        }
    }

    public class ProfileTextPosition
    {
        internal const long TwoTo32 = 4294967296;

        public ProfileTextPosition(TextPosition position, TextPosition bodyPosition, TextPosition secondaryBodyPosition,
            Fragment fragment)
        {
            Position = position;
            BodyPosition = bodyPosition;
            SecondaryBodyPosition = secondaryBodyPosition;
            Fragment = fragment;
        }

        public TextPosition Position { get; private set; }
        public TextPosition BodyPosition { get; private set; }
        public TextPosition SecondaryBodyPosition { get; private set; }
        public Fragment Fragment { get; private set; }

        public override string ToString()
        {
            return Position +
                   (BodyPosition.Length != 0 || SecondaryBodyPosition.Length != 0
                       ? "(" + BodyPosition + (SecondaryBodyPosition.Length != 0 ? ";" + SecondaryBodyPosition : "") +
                         ")"
                       : "");
        }
    }
}
