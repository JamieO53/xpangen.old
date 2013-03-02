// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.ViewModel
{
    public class GenDataEditorViewModel: BindableObject
    {
        private GenObjectViewModel _selectedNode;
        public GeData Data { get; set; }
        public GenObjectViewModel SelectedNode
        {
            get
            {
                return _selectedNode;
            }
            set
            {
                if (value != null)
                    value.EstablishContext();
                _selectedNode = value;
            }
        }

    }
}