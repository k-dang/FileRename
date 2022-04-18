using FileRename.Stores;

namespace FileRename.ViewModels
{
    public class AlternatingConfigViewModel : ViewModelBase
    {
        private string _seperator = "_";
        public string Seperator
        {
            get { return _seperator; }
            set
            {
                _seperator = value;
                OnPropertyChanged(nameof(Seperator));
            }
        }

        private string _charsToAlternate = "A,B";
        public string CharsToAlternate
        {
            get { return _charsToAlternate; }
            set
            {
                _charsToAlternate = value;
                OnPropertyChanged(nameof(CharsToAlternate));
            }
        }

        private readonly ConfigStore _configStore;

        public AlternatingConfigViewModel(ConfigStore configStore)
        {
            _configStore = configStore;
        }

        public void OnConfigChanged()
        {
            _configStore.ChangeAlternatingConfig();
        }
    }
}
