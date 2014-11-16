// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GenMultiUndoRedo: IGenUndoRedo
    {
        private readonly List<IGenUndoRedo> _commands = new List<IGenUndoRedo>();

        public void Undo()
        {
            for (var index = _commands.Count - 1; index >= 0; index--)
                _commands[index].Undo();
        }

        public void Redo()
        {
            foreach (var command in _commands)
                command.Redo();
        }

        public IGenUndoRedo Add(IGenUndoRedo undoRedo)
        {
            _commands.Add(undoRedo);
            return undoRedo;
        }
    }
}
