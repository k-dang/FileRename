using FileRename.Services;
using FileRename.ViewModels;
using FileRename.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using FileRename.Core;

namespace FileRename.Commands
{
    public class RenameCommand : CommandBase
    {
        private MainViewModel _mainViewModel;
        private readonly IFileService _fileService;

        public RenameCommand(
            MainViewModel mainViewModel,
            IFileService fileService)
        {
            _mainViewModel = mainViewModel;
            _fileService = fileService;
        }

        public override void Execute(object? parameter)
        {
            var fileNames = _mainViewModel.FileNames;
            var filePaths = _mainViewModel._filePaths;

            if (parameter == null)
            {
                return;
            }

            IList items = (IList)parameter;
            var selectedFiles = items.Cast<string>().ToList();
            var selectedIndexes = ControlUtil.GetSelectedIndexes(selectedFiles, fileNames);
            var selectedFilesFullPath = filePaths.Where((f, i) => selectedIndexes.IndexOf(i) != -1);

            var mode = _mainViewModel.SelectedRenameMode;
            Queue<FileToRename> unDoQueue = new();
            try
            {
                switch (mode)
                {
                    case "Sequence":
                        {
                            var sequenceConfig = (SequenceConfigViewModel)_mainViewModel.CurrentConfigViewModel;
                            var fileToRenameList = _fileService.GetSequenceFileToRenameList(selectedFilesFullPath, sequenceConfig.Seperator, sequenceConfig.SequenceStart);
                            if (fileToRenameList.Count == 0)
                            {
                                return;
                            }

                            foreach (var fileToRename in fileToRenameList)
                            {
                                Trace.WriteLine($"{fileToRename.PreviousFilePath} -> {fileToRename.NewFilePath}");
                                _fileService.Rename(fileToRename.PreviousFilePath, fileToRename.NewFilePath);
                                unDoQueue.Enqueue(fileToRename);
                            }
                            _mainViewModel.UpdateFileNames();
                            _mainViewModel.IsSelectAllChecked = false;
                            break;
                        }
                    case "Alternating":
                        {
                            var alternatingConfig = (AlternatingConfigViewModel)_mainViewModel.CurrentConfigViewModel;
                            var fileToRenameList = _fileService.GetAlternatingFileToRenameList(selectedFilesFullPath, alternatingConfig.Seperator, alternatingConfig.CharsToAlternate);
                            if (fileToRenameList.Count == 0)
                            {
                                return;
                            }

                            foreach (var fileToRename in fileToRenameList)
                            {
                                Trace.WriteLine($"{fileToRename.PreviousFilePath} -> {fileToRename.NewFilePath}");
                                _fileService.Rename(fileToRename.PreviousFilePath, fileToRename.NewFilePath);
                                unDoQueue.Enqueue(fileToRename);
                            }
                            _mainViewModel.UpdateFileNames();
                            _mainViewModel.IsSelectAllChecked = false;
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // TODO log exception somewhere
                MessageBox.Show("Unable to rename all files in selection");
                while (unDoQueue.Count > 0)
                {
                    var undoItem = unDoQueue.Dequeue();
                    _fileService.Rename(undoItem.NewFilePath, undoItem.PreviousFilePath);
                }
            }
        }
    }
}
