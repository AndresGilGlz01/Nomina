using Microsoft.Extensions.DependencyInjection;
using Nomina.Models;
using Nomina.Views;
using System;
using System.Windows;

namespace Nomina
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            services.AddDbContext<NominaContext>();
            services.AddSingleton<MainView>();

            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainView>();
            mainWindow.Show();
        }
    }
}
