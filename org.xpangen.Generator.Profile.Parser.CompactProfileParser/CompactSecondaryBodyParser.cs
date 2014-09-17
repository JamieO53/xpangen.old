// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Diagnostics.Contracts;

namespace org.xpangen.Generator.Profile.Parser.CompactProfileParser
{
    public class CompactSecondaryBodyParser : CompactPartialBodyParserBase
    {
        public CompactSecondaryBodyParser(GenCompactProfileParser genCompactProfileParser)
        {
            GenCompactProfileParser = genCompactProfileParser;
            IsPrimary = false;
        }

        protected override void AddFragment(GenSegBody body, GenFragment frag)
        {
            Contract.Assert(!body.SecondaryFragment.Contains(frag), "Fragment added to body again");
            body.AddSecondary(frag);
        }
    }
}