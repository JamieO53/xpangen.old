﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeGridDataValidator : GeComponent, IGenValidator
    {
        internal GeGridDataValidator(GeData data) : base(data)
        {
        }

        public void Disable()
        {
            //Grid.Enabled := False;
            //Grid.RowCount := 1;
            //Grid.Cells[0, 0] := '';
            //Grid.Cells[1, 0] := '';
            //Grid.Row := 0;
            //Grid.Col := 1;
        }

        public void DisplayObject(bool created)
        {
            throw new NotImplementedException();
        }

        public bool DataChanged { get; set; }
        public GenObject GenObject { get; set; }
        public char Key { get; set; }
        public void Validate()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
