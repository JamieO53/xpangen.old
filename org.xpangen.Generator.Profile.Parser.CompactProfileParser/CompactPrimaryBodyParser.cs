// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Profile.Parser.CompactProfileParser
{
    public class CompactPrimaryBodyParser : CompactPartialBodyParserBase
    {
        public CompactPrimaryBodyParser(GenCompactProfileParser genCompactProfileParser)
        {
            GenCompactProfileParser = genCompactProfileParser;
            IsPrimary = true;
        }

        protected override void AddFragment(GenSegBody body, GenFragment frag)
        {
            Assert(!body.Fragment.Contains(frag), "Fragment added to body again");
            body.Add(frag);
        }
    }
}