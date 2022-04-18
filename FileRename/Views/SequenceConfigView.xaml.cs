using FileRename.ViewModels;
using System.Windows.Controls;

namespace FileRename.Views
{
    /// <summary>
    /// Interaction logic for ModeConfigView.xaml
    /// </summary>
    public partial class SequenceConfigView : UserControl
    {
        public SequenceConfigView()
        {
            InitializeComponent();
        }

        private void OnTextBoxChange(object sender, TextChangedEventArgs e)
        {
            var viewModel = (SequenceConfigViewModel)DataContext;
            if (viewModel == null)
            {
                return;
            }
            viewModel.OnConfigChanged();
        }
    }
}
