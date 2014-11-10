using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class ProfileTextPostionList : SortedDictionary<long, ProfileTextPosition>
    {
        public ProfileTextPosition FindAtPosition(int position)
        {
            var key = position*ProfileTextPosition.TwoTo32;
            return this.FirstOrDefault(p => key >= p.Key).Value;
        }
    }

    public class TextPosition
    {
        public int Offset { get; set; }
        public int Length { get; set; }
        public long Key { get { return Offset*ProfileTextPosition.TwoTo32 + Length; } }

        public TextPosition(int offset, int length) : this()
        {
            Offset = offset;
            Length = length;
        }

        public TextPosition()
        {
            Offset = 0;
            Length = 0;
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
    }
}
