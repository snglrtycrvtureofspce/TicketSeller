using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketSeller.DBModel;
using TicketSeller.View;
using TicketSeller.ViewModel;

namespace TicketSeller
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TicketDbContext>(options =>
                options.UseSqlite("Data Source = app.db")
            );

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainView>();
            services.AddSingleton<AuthenticationView>();
            services.AddSingleton<AuthenticationViewModel>();
        }
        
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainView>();
            var mainViewModel = _serviceProvider.GetService<MainViewModel>();
            mainWindow!.DataContext = mainViewModel;
            mainWindow.Show();
        }
    }
}