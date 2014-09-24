// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Windows.Forms;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.Utilities
{
    public class TreeViewBuilderBase
    {
        protected TreeViewBuilderBase(GeData data)
        {
            Data = data;
            Dat = data.GenDataBase;
            Def = data.GenDataDef;
        }

        protected GeData Data { get; private set; }
        protected GenDataDef Def { get; private set; }
        protected GenDataBase Dat { get; private set; }

        /// <summary>
        /// Create a new tree node.
        /// </summary>
        /// <param name="iconIndex">The index of the node's icon.</param>
        /// <param name="name">The node's name.</param>
        /// <param name="hint">The node's tool tip text.</param>
        /// <param name="nodeItem">The data linked to the node.</param>
        /// <returns></returns>
        protected static TreeNode CreateTreeNode(int iconIndex, string name, string hint, object nodeItem)
        {
            return new TreeNode {Text = name, ImageIndex = iconIndex, ToolTipText = hint, Tag = nodeItem};
        }
    }
}