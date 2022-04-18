using FileRename.Core;

namespace FileRename.Services
{
    public class FileService : IFileService
    {
        public FileService() { }

        public string[] ReadFiles(string targetDirectory)
        {
            return Directory.GetFiles(targetDirectory);
        }

        public string GetFilePathWithSuffix(string path, string suffix)
        {
            var sourceDirectory = Path.GetDirectoryName(path);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);

            var destPath = $"{sourceDirectory}\\{fileNameWithoutExtension}{suffix}{extension}";

            return destPath;
        }

        public void Rename(string sourcePath, string destPath)
        {
            Directory.Move(sourcePath, destPath);
        }

        public List<FileToRename> GetSequenceFileToRenameList(IEnumerable<string> filePaths, string separator, string sequenceStart)
        {
            if (string.IsNullOrWhiteSpace(sequenceStart))
            {
                return new List<FileToRename>();
            }

            // handle sequences of double digits but not characters
            bool isNumberSequence = int.TryParse(sequenceStart, out int sequenceNumber);
            if (!isNumberSequence && sequenceStart.Length > 1)
            {
                return new List<FileToRename>();
            }

            LinkedListNode<string>? current = new(string.Empty);
            if (!isNumberSequence)
            {
                var seqStartChar = sequenceStart.ToCharArray()[0];
                var alphabet = "abcdefghijklmnopqrstuvwxyz";
                if (Char.IsUpper(seqStartChar))
                {
                    alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                }
                var splitIndex = alphabet.IndexOf(sequenceStart);
                var reArrangedAlpha = alphabet[splitIndex..] + alphabet[..splitIndex];
                var characterList = reArrangedAlpha.ToCharArray().Select(c => c.ToString()).ToList();
                LinkedList<string> newSequence = new(characterList);
                current = newSequence.First ?? current;
            }

            var fileToRenameList = new List<FileToRename>();
            foreach (var filePath in filePaths)
            {
                var suffix = separator;
                if (isNumberSequence)
                {
                    suffix += sequenceNumber.ToString();
                    sequenceNumber++;
                }
                else
                {
                    suffix += current?.Value;
                    // Loop around to front if at end
                    current = current?.Next ?? current?.List?.First;
                }

                var newFilePath = GetFilePathWithSuffix(filePath, suffix);
                var fileToRename = new FileToRename(filePath, newFilePath);
                fileToRenameList.Add(fileToRename);
            }

            return fileToRenameList;
        }

        public List<FileToRename> GetAlternatingFileToRenameList(IEnumerable<string> filePaths, string separator, string charsToAlternate)
        {
            if (string.IsNullOrWhiteSpace(charsToAlternate))
            {
                return new List<FileToRename>();
            }

            var split = charsToAlternate.Split(",");
            LinkedList<string> chars = new(split);
            LinkedListNode<string>? current = chars.First;

            var fileToRenameList = new List<FileToRename>();
            foreach (var filePath in filePaths)
            {
                var suffix = separator + current?.Value;
                var newFilePath = GetFilePathWithSuffix(filePath, suffix);
                var fileToRename = new FileToRename(filePath, newFilePath);
                fileToRenameList.Add(fileToRename);

                current = current?.Next ?? current?.List?.First;
            }

            return fileToRenameList;
        }
    }
}
