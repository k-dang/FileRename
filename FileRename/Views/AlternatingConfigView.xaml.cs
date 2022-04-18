using FileRename.ViewModels;
using System.Windows.Controls;

namespace FileRename.Views
{
    public partial class AlternatingConfigView : UserControl
    {
        public AlternatingConfigView()
        {
            InitializeComponent();
        }

        private void OnTextBoxChange(object sender, TextChangedEventArgs e)
        {
            var viewModel = (AlternatingConfigViewModel)DataContext;
            if (viewModel == null)
            {
                return;
            }
            viewModel.OnConfigChanged();
        }
    }
}
