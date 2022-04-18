namespace FileRename.Core
{
    public class FileToRename
    {
        public string PreviousFilePath { get; set; }

        public string NewFilePath { get; set; }

        public FileToRename(string previousFilePath, string newFilePath)
        {
            PreviousFilePath = previousFilePath;
            NewFilePath = newFilePath;
        }
    }
}
