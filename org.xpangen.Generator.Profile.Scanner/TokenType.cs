// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Profile.Scanner
{
    public enum TokenType
    {
        Null,
        Segment,
        Block,
        Lookup,
        Condition,
        Function,
        Secondary,
        Close,
        Delimiter,
        Name,
        Unknown
    }
}
