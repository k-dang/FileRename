using System;
using System.Collections.Generic;

namespace FileRename.Stores
{
    public class FileStore
    {
        public List<string>? SelectedFiles { get; set; }

        public event Action SelectedFilesChanged;

        public string SelectedFolder { get; set; }

        public event Action SelectedFolderChanged;

        public void UpdateSelectedFiles(List<string> selectedFiles)
        {
            SelectedFiles = selectedFiles;
            SelectedFilesChanged?.Invoke();
        }

        public void UpdateSelectedFolder(string selectedFolder)
        {
            SelectedFolder = selectedFolder;
            SelectedFolderChanged?.Invoke();
        }
    }
}
