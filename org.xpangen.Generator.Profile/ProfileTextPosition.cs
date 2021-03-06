﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class ProfileTextPostionList : List<ProfileTextPosition>
    {
        private Dictionary<Fragment, ProfileTextPosition> _fragmenPositions =
            new Dictionary<Fragment, ProfileTextPosition>();
        /// <summary>
        /// Matches the shortest fragment at the selected position.
        /// </summary>
        /// <param name="position">The selected position.</param>
        /// <returns>The text postion of the matched fragment. If no fragment is matched, a null is returned.</returns>
        public ProfileTextPosition FindAtPosition(int position)
        {
            var minLength = int.MaxValue;
            foreach (var value in this)
            {
                if (value.Position.Offset <= position && position < value.Position.Offset + value.Position.Length)
                    if (value.Position.Length < minLength)
                        minLength = value.Position.Length;
            }
            if (minLength == int.MaxValue)
                foreach (var value in this)
                {
                    if (value.Position.Offset + value.Position.Length == position && value.Position.Length < minLength)
                        minLength = value.Position.Length;
                }
            foreach (var value in this)
            {
                if (value.Position.Offset <= position && position <= value.Position.Offset + value.Position.Length &&
                    value.Position.Length == minLength)
                {
                    var container = value.Fragment as ContainerFragment;
                    if (container == null || position != value.BodyPosition.EndPosition ||
                        container.Body().FragmentList.Count <= 0) return value;
                    var fragment = container.Body().FragmentList[container.Body().FragmentList.Count - 1];
                    foreach (var value1 in this)
                        if (value1.Fragment == fragment) return value1;
                    return value;
                }
            }
            return null;
        }

        public ProfileTextPosition GetFragmentPosition(Fragment fragment)
        {
            Contract.Requires(ContainsFragment(fragment));
            return _fragmenPositions[fragment];
        }

        public bool ContainsFragment(Fragment fragment)
        {
            return _fragmenPositions.ContainsKey(fragment);
        }

        public new void Add(ProfileTextPosition value)
        {
            Contract.Ensures(ContainsFragment(value.Fragment));
            if (ContainsValue(value)) return;
                // Function parameters get added twice
            base.Add(value);
            _fragmenPositions.Add(value.Fragment, value);
        }

        private bool ContainsValue(ProfileTextPosition value)
        {
            return this.Any(item => value.Position.Key == item.Position.Key && value.Fragment == item.Fragment);
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
