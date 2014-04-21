// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Editor.Model;

namespace org.xpangen.Generator.Editor.Helper
{
    public interface IGenDataSettings
    {
        /// <summary>
        /// The Root of the GenData container of the editor's settings.
        /// </summary>
        Root Model { get; }

        /// <summary>
        /// The base file information for the selected file group.
        /// </summary>
        BaseFile BaseFile { get; set; }

        /// <summary>
        /// The most recently selected file group
        /// </summary>
        FileGroup FileGroup { get; set; }

        /// <summary>
        /// The relative path of the selected file group.
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// The full path of the generated file.
        /// </summary>
        string Generated { get; set; }

        /// <summary>
        /// The home directory of the generator data
        /// </summary>
        string HomeDir { get; set; }

        /// <summary>
        /// The name of the most recently generated profile for the selected file group.
        /// </summary>
        string Profile { get; set; }

        /// <summary>
        /// The relative path of the file group's base file.
        /// </summary>
        string BaseFilePath { get; }

        /// <summary>
        /// Searches for the requested file group, and moves it to the top of the 
        /// </summary>
        /// <param name="name">The name of the selected file group</param>
        /// <returns>The selected file group.</returns>
        FileGroup GetFileGroup(string name);

        /// <summary>
        /// Get a list of all the file groups.
        /// </summary>
        /// <returns>The file group list.</returns>
        GenNamedApplicationList<FileGroup> GetFileGroups();

        /// <summary>
        /// Get a list of all the base files.
        /// </summary>
        /// <returns>The base file list.</returns>
        GenNamedApplicationList<BaseFile> GetBaseFiles();

        /// <summary>
        /// Find the specified base file.
        /// </summary>
        /// <param name="name">The name of the sought base file.</param>
        /// <returns>The requested base file</returns>
        BaseFile FindBaseFile(string name);

        /// <summary>
        /// Find the specified file group.
        /// </summary>
        /// <param name="name">The name of the sought file group.</param>
        /// <returns>The requested base file</returns>
        FileGroup FindFileGroup(string name);

        /// <summary>
        /// Check that the minimum structure exists in the settings file.
        /// </summary>
        void Check();
    }
}