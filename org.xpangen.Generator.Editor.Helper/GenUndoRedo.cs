// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Threading;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GenUndoRedo : IGenUndoRedo
    {
        public GenUndoRedo(IGenCommand undoCommand, IGenCommand redoCommand)
        {
            RedoCommand = redoCommand;
            UndoCommand = undoCommand;
        }

        public virtual void Undo()
        {
            if (UndoCommand != null) UndoCommand.Execute();
        }

        public virtual void Redo()
        {
            if (RedoCommand != null) RedoCommand.Execute();
        }

        private IGenCommand UndoCommand { get; set; }

        private IGenCommand RedoCommand { get; set; }

    }
}
