namespace FileRename.ViewModels
{
    public class CustomConfigViewModel : ViewModelBase
    {
        private string _suffix = "_A";
        public string Suffix
        {
            get { return _suffix; }
            set
            {
                _suffix = value;
                OnPropertyChanged(nameof(Suffix));
            }
        }
    }
}
