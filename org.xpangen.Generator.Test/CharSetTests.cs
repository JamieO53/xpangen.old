// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Globalization;
using NUnit.Framework;
using org.xpangen.Generator.Scanner;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests Character Set functionality
    /// </summary>
    [TestFixture]
    public class CharSetTests
    {
        /// <summary>
        /// Verifies a CharSet consisting of a list of characters
        /// </summary>
        [TestCase]
        public void ListOfCharactersTest()
        {
            var cs = new CharSet("abcABC");
            foreach (var x in "abc")
            {
                Assert.IsTrue(cs.Match(x), x + " in [abcABC]");
                Assert.IsTrue(cs.Match(char.ToUpper(x)), char.ToUpper(x) + " in [abcABC]");
            }
            Assert.IsFalse(cs.Match('x'));
        }

        /// <summary>
        /// Verifies a CharSet consisting of a range of characters
        /// </summary>
        [TestCase]
        public void RangeOfCharactersTest()
        {
            var cs = new CharSet("a-c");
            foreach (var x in "abc")
            {
                Assert.IsTrue(cs.Match(x), x + " in [a-c]");
                Assert.IsFalse(cs.Match(char.ToUpper(x)), char.ToUpper(x) + " in [a-c]");
            }
            Assert.IsFalse(cs.Match('x'));
        }

        /// <summary>
        /// Verifies a CharSet consisting of multiple ranges of characters
        /// </summary>
        [TestCase]
        public void MultipleRangeOfCharactersTest()
        {
            var cs = new CharSet("a-cA-C");
            foreach (var x in "abc")
            {
                Assert.IsTrue(cs.Match(x), x + " in [abcABC]");
                Assert.IsTrue(cs.Match(char.ToUpper(x)), char.ToUpper(x) + " in [abcABC]");
            }
            Assert.IsFalse(cs.Match('x'));
        }

        /// <summary>
        /// Verifies a CharSet consisting of escaped special characters
        /// </summary>
        [TestCase]
        public void EscapedCharactersTest()
        {
            var cs = new CharSet(@"\n\r\t\\\-");
            var ca = "\n\r\t\\-".ToCharArray();
            foreach (var x in ca)
            {
                Assert.IsTrue(cs.Match(x), ((Int16)x).ToString(CultureInfo.InvariantCulture));
            }
        }
        
        /// <summary>
        /// Verifies a CharSet consisting of a individual characters and ranges of characters
        /// </summary>
        [TestCase]
        public void CharactersAndRangesOfCharactersTest()
        {
            var cs = new CharSet("aA-Cb0-2c");
            foreach (var x in "abcABC012")
            {
                Assert.IsTrue(cs.Match(x), x + " in [aA-Cb0-2c]");
            }
            Assert.IsFalse(cs.Match('x'));
        }
    }
}
