using FileRename.Services;
using FileRename.Stores;
using FileRename.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace FileRename
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            var fileService = ServiceProvider.GetRequiredService<IFileService>();
            var configStore = ServiceProvider.GetRequiredService<ConfigStore>();
            var fileStore = ServiceProvider.GetRequiredService<FileStore>();

            SequenceConfigViewModel sequenceConfigViewModel = new(configStore);
            AlternatingConfigViewModel alternatingConfigViewModel = new(configStore);

            mainWindow.DataContext = new MainViewModel(sequenceConfigViewModel, alternatingConfigViewModel, fileService, configStore, fileStore);
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ConfigStore>();
            services.AddSingleton<FileStore>();
        }
    }
}
