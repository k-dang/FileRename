using FileRename.Services;
using FileRename.Stores;
using FileRename.ViewModels;
using System.Windows;

namespace FileRename
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            var fileService = new FileService();
            var fileStore = new FileStore();
            var configStore = new ConfigStore();
            var mainWindow = new MainWindow(fileStore);

            SequenceConfigViewModel sequenceConfigViewModel = new(configStore);
            AlternatingConfigViewModel alternatingConfigViewModel = new(configStore);

            mainWindow.DataContext = new MainViewModel(sequenceConfigViewModel, alternatingConfigViewModel, fileService, configStore, fileStore);
            mainWindow.Show();

            //base.OnStartup(e);
        }
    }
}
