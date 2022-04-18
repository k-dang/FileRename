using FileRename.Services;
using System;
using System.IO;
using Xunit;

namespace FileRename.Tests
{
    public class FileServiceTests : IDisposable
    {
        private readonly string testDirectory;
        private readonly string testFile;
        private readonly IFileService _fileService;

        public FileServiceTests()
        {
            // Setup test files
            testDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFolder");
            testFile = testDirectory + "/testFile.txt";
            Directory.CreateDirectory(testDirectory);
            File.WriteAllText(testFile, "Hello World");

            _fileService = new FileService();
        }

        public void Dispose()
        {
            if (testDirectory != null)
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void ReadFiles_Should_Return_Files()
        {
            var files = _fileService.ReadFiles(testDirectory);
            Assert.NotEmpty(files);
        }

        [Fact]
        public void GetFilePathWithSuffix_Should_Contain_Suffix()
        {
            var suffix = "_A";
            var fileName = Path.GetFileNameWithoutExtension(testFile);
            var fileExtension = Path.GetExtension(testFile);
            var filePathWithSuffix = _fileService.GetFilePathWithSuffix(testFile, suffix);

            Assert.Contains(suffix, filePathWithSuffix);
            Assert.Contains(fileName, filePathWithSuffix);
            Assert.Contains(fileExtension, filePathWithSuffix);
        }
    }
}