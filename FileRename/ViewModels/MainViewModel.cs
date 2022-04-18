using FileRename.Commands;
using FileRename.Services;
using FileRename.Stores;
using FileRename.Util;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace FileRename.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _selectedFolderPath;
        public string SelectedFolderPath
        {
            get { return _selectedFolderPath; }
            set
            {
                _selectedFolderPath = value;
                OnPropertyChanged(nameof(SelectedFolderPath));
            }
        }

        public string[] _filePaths;

        private ObservableCollection<string> _fileNames;
        public ObservableCollection<string> FileNames
        {
            get { return _fileNames; }
            set
            {
                _fileNames = value;
                OnPropertyChanged(nameof(FileNames));
            }
        }

        private ObservableCollection<string> _previewFileNames;
        public ObservableCollection<string> PreviewFileNames
        {
            get { return _previewFileNames; }
            set
            {
                _previewFileNames = value;
                OnPropertyChanged(nameof(PreviewFileNames));
            }
        }

        private ViewModelBase _currentConfigViewModel;
        public ViewModelBase CurrentConfigViewModel
        {
            get { return _currentConfigViewModel; }
            set
            {
                _currentConfigViewModel = value;
                OnPropertyChanged(nameof(CurrentConfigViewModel));
            }
        }

        private readonly ObservableCollection<string> _renameModes = new()
        {
            "Sequence",
            "Alternating",
        };
        public ObservableCollection<string> RenameModes
        {
            get { return _renameModes; }
        }

        private string _selectedRenameMode;
        public string SelectedRenameMode
        {
            get { return _selectedRenameMode; }
            set
            {
                UpdateCurrentConfigViewModel(value);
                _selectedRenameMode = value;
            }
        }

        private bool _isSelectAllChecked;
        public bool IsSelectAllChecked
        {
            get { return _isSelectAllChecked; }
            set
            {
                _isSelectAllChecked = value;
                OnPropertyChanged(nameof(IsSelectAllChecked));
            }
        }

        public ICommand RenameCommand { get; }

        private readonly SequenceConfigViewModel _sequenceConfigViewModel;
        private readonly AlternatingConfigViewModel _alternatingConfigViewModel;
        private readonly IFileService _fileService;
        private readonly ConfigStore _configStore;
        private readonly FileStore _fileStore;

        public MainViewModel(
            SequenceConfigViewModel sequenceConfigViewModel,
            AlternatingConfigViewModel alternatingConfigViewModel,
            IFileService fileService, 
            ConfigStore configStore,
            FileStore fileStore)
        {
            _fileService = fileService;
            _configStore = configStore;
            _sequenceConfigViewModel = sequenceConfigViewModel;
            _alternatingConfigViewModel = alternatingConfigViewModel;
            _fileStore = fileStore;

            FileNames = new ObservableCollection<string>();
            PreviewFileNames = new ObservableCollection<string>();

            SelectedRenameMode = "Sequence";
            CurrentConfigViewModel = sequenceConfigViewModel;
            RenameCommand = new RenameCommand(this, _fileService);

            _configStore.SequenceConfigChanged += UpdatePreviewFileNames;
            _configStore.AlternatingConfigChanged += UpdatePreviewFileNames;
            _fileStore.SelectedFilesChanged += UpdatePreviewFileNames;
            _fileStore.SelectedFolderChanged += UpdateSelectedFolderAndFiles;
        }

        public void UpdateCurrentConfigViewModel(string selectedRenameMode)
        {
            CurrentConfigViewModel = selectedRenameMode switch
            {
                "Sequence" => _sequenceConfigViewModel,
                "Alternating" => _alternatingConfigViewModel,
                _ => new CustomConfigViewModel(),
            };
        }

        public void UpdateSelectedFolderAndFiles()
        {
            SelectedFolderPath = _fileStore.SelectedFolder;
            UpdateFileNames();
        }

        public void UpdateFileNames()
        {
            if (string.IsNullOrWhiteSpace(_fileStore.SelectedFolder))
            {
                return;
            }

            var fullPathFiles = _fileService.ReadFiles(_fileStore.SelectedFolder);
            _filePaths = fullPathFiles;

            var justFiles = fullPathFiles.Select(f => Path.GetFileName(f));
            FileNames = new ObservableCollection<string>(justFiles);
        }

        private void UpdatePreviewFileNames()
        {
            var selectedFiles = _fileStore.SelectedFiles;
            if (selectedFiles == null || selectedFiles.Count == 0)
            {
                PreviewFileNames = new ObservableCollection<string>();
                return;
            }

            var selectedIndexes = ControlUtil.GetSelectedIndexes(selectedFiles, FileNames);
            var selectedFilesFullPath = _filePaths.Where((f, i) => selectedIndexes.IndexOf(i) != -1);

            switch (SelectedRenameMode)
            {
                case "Sequence":
                    {
                        var sequenceConfig = (SequenceConfigViewModel)CurrentConfigViewModel;
                        var fileToRenameList = _fileService.GetSequenceFileToRenameList(selectedFilesFullPath, sequenceConfig.Seperator, sequenceConfig.SequenceStart);
                        var previewFileNames = fileToRenameList.Select(f => Path.GetFileName(f.NewFilePath));

                        PreviewFileNames = new ObservableCollection<string>(previewFileNames);
                        break;
                    }
                case "Alternating":
                    {
                        var alternatingConfig = (AlternatingConfigViewModel)CurrentConfigViewModel;
                        var fileToRenameList = _fileService.GetAlternatingFileToRenameList(selectedFilesFullPath, alternatingConfig.Seperator, alternatingConfig.CharsToAlternate);
                        var previewFileNames = fileToRenameList.Select(f => Path.GetFileName(f.NewFilePath));

                        PreviewFileNames = new ObservableCollection<string>(previewFileNames);
                        break;
                    }
                default:
                    {

                    }
                    break;
            }
        }
    }
}
