using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.ViewModels;
using System;

namespace imdbdrinks_ratingsmodule
{
    public partial class App : Application
    {
        private Window? m_window;
        public IServiceProvider Services { get; }

        public App()
        {
            this.InitializeComponent();

            // Configure services and DI container
            Services = ConfigureServices();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            // Use DI to get MainWindow with dependencies injected
            m_window = Services.GetRequiredService<MainWindow>();
            m_window.Activate();
        }

        private IServiceProvider ConfigureServices()
        {
            // Load appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            // Add configuration to DI container
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<DatabaseConnection>();

            // Add repositories, services, and view models to DI container
            services.AddSingleton<IReviewRepository, DatabaseReviewRepository>();
            services.AddSingleton<IRatingRepository, DatabaseRatingRepository>();

            services.AddSingleton<RatingService>();
            services.AddSingleton<ReviewService>();

            services.AddSingleton<RatingViewModel>();
            services.AddSingleton<ReviewViewModel>();
            services.AddSingleton<MainViewModel>();

            services.AddTransient<RatingService>();
            services.AddTransient<ReviewService>();

            // Register MainWindow
            services.AddSingleton<MainWindow>();

            return services.BuildServiceProvider();
        }
    }
}
