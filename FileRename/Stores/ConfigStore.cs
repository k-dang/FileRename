using System;

namespace FileRename.Stores
{
    public class ConfigStore
    {
        public event Action SequenceConfigChanged;
        public event Action AlternatingConfigChanged;

        public void ChangeSequenceConfig()
        {
            SequenceConfigChanged?.Invoke();
        }

        public void ChangeAlternatingConfig()
        {
            AlternatingConfigChanged?.Invoke();
        }
    }
}
