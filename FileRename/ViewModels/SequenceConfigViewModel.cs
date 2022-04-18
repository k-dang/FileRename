using FileRename.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FileRename.ViewModels
{
    public class SequenceConfigViewModel : ViewModelBase, INotifyDataErrorInfo
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

        private string _sequenceStart = "1";
        public string SequenceStart
        {
            get { return _sequenceStart; }
            set
            {
                _sequenceStart = value;
                OnPropertyChanged(nameof(SequenceStart));

                ClearErrors(nameof(SequenceStart));

                bool isNumeric = int.TryParse(SequenceStart, out int ignore);
                if (!isNumeric && SequenceStart.Length > 1)
                {
                    AddError(nameof(SequenceStart), "Limit alphabet characters to 1 length");
                }
            }
        }

        private readonly ConfigStore _configStore;

        private readonly Dictionary<string, List<string>> _propertyNameToErrorsDictionary;

        public bool HasErrors => _propertyNameToErrorsDictionary.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public SequenceConfigViewModel(ConfigStore configStore)
        {
            _configStore = configStore;
            _propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
        }

        public void OnConfigChanged()
        {
            _configStore.ChangeSequenceConfig();
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName == null)
            {
                return new List<string>();
            }

            return _propertyNameToErrorsDictionary.GetValueOrDefault(propertyName, new List<string>());
        }

        private void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyNameToErrorsDictionary.ContainsKey(propertyName))
            {
                _propertyNameToErrorsDictionary.Add(propertyName, new List<string>());
            }

            _propertyNameToErrorsDictionary[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        private void ClearErrors(string propertyName)
        {
            _propertyNameToErrorsDictionary.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
