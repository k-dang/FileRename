using System.Windows;
using System.Windows.Forms;
using System.Linq;
using System.Windows.Controls;
using FileRename.Stores;
using AdonisUI.Controls;

namespace FileRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        private readonly FileStore _fileStore;
        private FolderBrowserDialog folderBrowserDialog;

        public MainWindow(FileStore fileStore)
        {
            InitializeComponent();
            _fileStore = fileStore;

            folderBrowserDialog = new FolderBrowserDialog();
        }

        private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedFiles = FilesListBox.SelectedItems.Cast<string>().ToList();
            _fileStore.UpdateSelectedFiles(selectedFiles);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            FilesListBox.SelectAll();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            FilesListBox.UnselectAll();
        }

        private void Folder_Select_Click(object sender, RoutedEventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _fileStore.UpdateSelectedFolder(folderBrowserDialog.SelectedPath);
                SelectAllBox.IsChecked = false;
            }
        }
    }
}
