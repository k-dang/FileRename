using FileRename.Core;

namespace FileRename.Services
{
    public interface IFileService
    {
        List<FileToRename> GetSequenceFileToRenameList(IEnumerable<string> filePaths, string separator, string sequenceStart);
        string GetFilePathWithSuffix(string path, string suffix);
        string[] ReadFiles(string targetDirectory);
        void Rename(string sourcePath, string destPath);
        List<FileToRename> GetAlternatingFileToRenameList(IEnumerable<string> filePaths, string separator, string charsToAlternate);
    }
}